using NCrawler;
using NCrawler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;
using NCrawler.HtmlProcessor;
using NCrawler.HtmlProcessor.Extensions;
using NCrawler.Extensions;
using NCrawler.HtmlProcessor.Extensions;
using NCrawler.Utils;
namespace cc
{
    public class HtmlDocumentExProcessorPipelineStep : ContentCrawlerRules, IPipelineStep
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(HtmlDocumentExProcessorPipelineStep));
        public HtmlDocumentExProcessorPipelineStep(int useBloomFilterCapacity, int maxDegreeOfParallelism)
            : this(useBloomFilterCapacity, maxDegreeOfParallelism, null, null)
		{

        }
        public HtmlDocumentExProcessorPipelineStep(int useBloomFilterCapacity,
			int maxDegreeOfParallelism,
			Dictionary<string, string> filterTextRules,
			Dictionary<string, string> filterLinksRules)
			: base(filterTextRules, filterLinksRules)
		{
			MaxDegreeOfParallelism = maxDegreeOfParallelism;
            UseBloomFilter = useBloomFilterCapacity>0;
            if (UseBloomFilter)
            {
                BloomFilter = new BloomFilter.Filter<string>(useBloomFilterCapacity);
            }
        }
        public int MaxDegreeOfParallelism { get; protected set; }
        public bool UseBloomFilter { get; protected set; }
        public BloomFilter.Filter<string> BloomFilter { get; protected set; }
        protected virtual string NormalizeLink(string baseUrl, string link)
        {
            return link.NormalizeUrl(baseUrl);
        }

        private static bool IsHtmlContent(string contentType)
        {
            return contentType.StartsWith("text/html", StringComparison.OrdinalIgnoreCase);
        }

        public void Process(Crawler crawler, PropertyBag propertyBag)
        {
            try
            {
                log.DebugFormat("FIND URL:{0}", propertyBag.Step.Uri);
                string content = string.Empty;
                using (var response = propertyBag.GetResponse())
                {
                    var encoding = Encoding.UTF8;
                    if (!String.IsNullOrWhiteSpace(propertyBag.ContentEncoding))
                    {
                        encoding = Encoding.GetEncoding(propertyBag.ContentEncoding);
                    }
                    if (!String.IsNullOrWhiteSpace(propertyBag.CharacterSet))
                    {
                        encoding = Encoding.GetEncoding(propertyBag.CharacterSet);
                    }
                    using (var streamReader = new StreamReader(response, encoding, true))
                    {
                        content = streamReader.ReadToEnd();
                    }
                }
                propertyBag["Content"].Value = content;
                /*-------------------------------------------------------------------------------------*/
                string baseUrl = propertyBag.ResponseUri.GetLeftPart(UriPartial.Path);
                propertyBag["BaseUrl"].Value = baseUrl;
                /*-------------------------------------------------------------------------------------*/
                HtmlDocument htmlDoc = new HtmlDocument
                {
                    OptionAddDebuggingAttributes = false,
                    OptionAutoCloseOnEnd = true,
                    OptionFixNestedTags = true,
                    OptionReadEncoding = true
                };
                htmlDoc.LoadHtml(content);
                propertyBag["HtmlDoc"].Value = htmlDoc;
                /*-------------------------------------------------------------------------------------*/
                // Extract Links
                DocumentWithLinks links = htmlDoc.GetLinks();
                foreach (string link in links.Links.Union(links.References))
                {
                    if (link.IsNullOrEmpty())
                    {
                        continue;
                    }

                    string decodedLink = ExtendedHtmlUtility.HtmlEntityDecode(link);
                    string normalizedLink = NormalizeLink(baseUrl, decodedLink);
                    if (normalizedLink.IsNullOrEmpty())
                    {
                        continue;
                    }
                    log.DebugFormat("FIND Link:{0}", normalizedLink);
                    if (UseBloomFilter && BloomFilter!=null)
                    {
                        if (!BloomFilter.Contains(normalizedLink))
                        {
                            BloomFilter.Add(normalizedLink);
                            crawler.AddStep(new Uri(normalizedLink), propertyBag.Step.Depth + 1);
                        }
                        else
                        {
                            log.DebugFormat("BloomFilter:{0}", normalizedLink);
                        }
                    }
                    else
                    {
                        crawler.AddStep(new Uri(normalizedLink), propertyBag.Step.Depth + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Exception:{0}", ex.ToString());
            }
        }

        
    }
}
