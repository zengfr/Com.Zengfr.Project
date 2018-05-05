using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using log4net;
using NCrawler;
using NCrawler.Extensions;
using NCrawler.HtmlProcessor;
using NCrawler.HtmlProcessor.Extensions;
using NCrawler.Utils;

namespace  Craw
{
   public class CHtmlDocumentProcessor: HtmlDocumentProcessor
    {
        protected static log4net.ILog logger = LogManager.GetLogger(typeof(CHtmlDocumentProcessor));
        protected static Regex charsetRegex = new Regex(@"charset\s*=[\s""']*([^\s""'/>]*)",RegexOptions.Compiled | RegexOptions.CultureInvariant|RegexOptions.IgnoreCase);
        protected static bool IsHtmlContent(string contentType)
        {
            return contentType.StartsWith("text/html", StringComparison.OrdinalIgnoreCase);
        }
        protected static Encoding DetectEncodingByHTML(string html)
        {
            if (!string.IsNullOrWhiteSpace(html))
            {
                if (html.Length > 500)
                    html = html.Substring(0, 500);
                   var m = charsetRegex.Match(html);
                if (m.Success)
                {
                   return Encoding.GetEncoding(m.Groups[1].Value);
                }
            }
            return null;
        }
        public override void Process(Crawler crawler, PropertyBag propertyBag)
        {
            try
            {
                ProcessEx(crawler, propertyBag);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Exception:{0},{1}",ex.Message, propertyBag.Step.Uri);
                    
            }
}
        protected void ProcessEx(Crawler crawler, PropertyBag propertyBag)
        {
            AspectF.Define
                .NotNull(crawler, nameof(crawler))
                .NotNull(propertyBag, nameof(propertyBag));

            if (propertyBag.StatusCode != HttpStatusCode.OK)
            {
                return;// Task.FromResult(true);
            }

            if (!IsHtmlContent(propertyBag.ContentType))
            {
                return;// Task.FromResult(true);
            }

            HtmlDocument htmlDoc = new HtmlDocument
            {
                OptionAddDebuggingAttributes = false,
                OptionAutoCloseOnEnd = true,
                OptionFixNestedTags = true,
                OptionReadEncoding = true
            };
            Encoding documentEncoding;
            using (Stream ms = propertyBag.GetResponse())
            {
                documentEncoding = htmlDoc.DetectEncoding(ms);
                ms.Seek(0, SeekOrigin.Begin);
                if (!documentEncoding.IsNull())
                {
                    htmlDoc.Load(ms, documentEncoding, true);
                }
                else
                {
                    htmlDoc.Load(ms, true);
                    //--
                    documentEncoding = DetectEncodingByHTML(htmlDoc.DocumentNode.OuterHtml);
                    if (documentEncoding == null)
                    {
                        documentEncoding =Encoding.GetEncoding(propertyBag.CharacterSet);
                    }
                    if (documentEncoding == null)
                    {
                        documentEncoding = Encoding.GetEncoding("utf-8");
                    } 
                    ms.Seek(0, SeekOrigin.Begin);
                    htmlDoc.Load(ms, documentEncoding, true); 
                }
                propertyBag["DetectEncoding"].Value = documentEncoding.WebName;
            }
            documentEncoding = null;

            string originalContent = htmlDoc.DocumentNode.OuterHtml;
            if (HasTextStripRules || HasSubstitutionRules)
            {
                string content = StripText(originalContent);
                content = Substitute(content, propertyBag.Step);
                using (TextReader tr = new StringReader(content))
                {
                    htmlDoc.Load(tr);
                }
                content = null;
            }
            originalContent = null;

            propertyBag["HtmlDoc"].Value = htmlDoc;

            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//title");
            // Extract Title
            if (!nodes.IsNull())
            {
                propertyBag.Title = string.Join(";", nodes.
                    Select(n => n.InnerText).
                    ToArray()).Trim();
            }

            // Extract Meta Data
            nodes = htmlDoc.DocumentNode.SelectNodes("//meta[@content and @name]");
            if (!nodes.IsNull())
            {
                propertyBag["Meta"].Value = (
                    from entry in nodes
                    let name = entry.Attributes["name"]
                    let content = entry.Attributes["content"]
                    where !name.IsNull() && !name.Value.IsNullOrEmpty() && !content.IsNull() && !content.Value.IsNullOrEmpty()
                    select $"{name.Value}: {content.Value}").ToArray();
            }

            // Extract text
            propertyBag.Text = htmlDoc.ExtractText().Trim();
            if (HasLinkStripRules || HasTextStripRules)
            {
                string content = StripLinks(originalContent);
                using (TextReader tr = new StringReader(content))
                {
                    htmlDoc.Load(tr);
                }
                content = null;
            }

            string baseUrl = propertyBag.ResponseUri.GetLeftPart(UriPartial.Path);

            // Extract Head Base
            nodes = htmlDoc.DocumentNode.SelectNodes("//head/base[@href]");
            if (!nodes.IsNull())
            {
                baseUrl = nodes
                    .Select(entry => new { entry, href = entry.Attributes["href"] })
                    .Where(arg => !arg.href.IsNull()
                        && !arg.href.Value.IsNullOrEmpty()
                        && Uri.IsWellFormedUriString(arg.href.Value, UriKind.RelativeOrAbsolute))
                    .Select(t =>
                    {
                        if (Uri.IsWellFormedUriString(t.href.Value, UriKind.Relative))
                        {
                            return propertyBag.ResponseUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped) + t.href.Value;
                        }

                        return t.href.Value;
                    })
                    .AddToEnd(baseUrl)
                    .FirstOrDefault();
            }

            // Extract Links
            DocumentWithLinks links = htmlDoc.GetLinks();
            var nextDepth = propertyBag.Step.Depth + 1;
            foreach (string link in links.Links.Union(links.References))
            {
                if (link.IsNullOrEmpty())
                {
                    continue;
                }

                string decodedLink = string.Empty;
                try
                {
                    decodedLink = ExtendedHtmlUtility.HtmlEntityDecode(link);
                    if (decodedLink.StartsWith("//"))
                    {
                        decodedLink = propertyBag.Step.Uri.Scheme + ":" + decodedLink;
                    }
                    string normalizedLink = NormalizeLink(baseUrl, decodedLink);
                    if (normalizedLink.IsNullOrEmpty()|| normalizedLink==propertyBag.Step.Uri.ToString())
                    {
                        continue;
                    }
                  
                    crawler.AddStep(new Uri(normalizedLink), nextDepth);
                    //logger.DebugFormat("Add:{0,2},{1}", nextDepth, normalizedLink);
                    //crawler.Crawl(new Uri(normalizedLink), propertyBag);
                    normalizedLink = null;
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Exception:{0},{1},{2}",ex.Message, propertyBag.Step.Uri, decodedLink);
                    continue;
                }
                decodedLink = null;
            }
            links = null;nodes = null;baseUrl = null; originalContent = null;
            return;// Task.FromResult(true);
        }
    }
}
