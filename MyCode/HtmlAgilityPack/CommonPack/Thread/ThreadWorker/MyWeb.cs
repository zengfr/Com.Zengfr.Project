﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
//request = MyWebRequest.Create(uri, request/*to Keep-Alive*/, KeepAlive);
//MyWebResponse response = request.GetResponse();
//byte[] RecvBuffer = new byte[10240];
//int nBytes, nTotalBytes = 0;
//while((nBytes = response.socket.Receive(RecvBuffer, 0, 
//                10240, SocketFlags.None)) > 0)
//{
//    nTotalBytes += nBytes;
//    ...
//    if(response.KeepAlive && nTotalBytes >= response.ContentLength 
//                          && response.ContentLength > 0)
//        break;
//}
//if(response.KeepAlive == false)
//    response.Close();
//
//Just replace the GetResponseStream with a direct access to the socket member of the MyWebResponse class. To do that, I did a simple trick to make the socket next read, to start, after the reply header, by reading one byte at a time to tell header completion, as in the following code:
//
///* reading response header */
//Header = "";
//byte[] bytes = new byte[10];
//while(socket.Receive(bytes, 0, 1, SocketFlags.None) > 0)
//{
//    Header += Encoding.ASCII.GetString(bytes, 0, 1);
//    if(bytes[0] == '\n' && Header.EndsWith("\r\n\r\n"))
//        break;
//}

    public class MyWebRequest
    {
        public MyWebRequest(Uri uri, bool bKeepAlive)
        {
            Headers = new WebHeaderCollection();
            RequestUri = uri;
            Headers["Host"] = uri.Host;
			
			Headers["User-Agent"] = " Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9) Gecko/2008052906 Firefox/3.0";
            //Headers["Accept-Encoding"] = "gzip,deflate";
			//User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)/IE6
            KeepAlive = bKeepAlive;
            if (KeepAlive)
                Headers["Connection"] = "Keep-Alive";
            Method = "GET";
        }
        public static MyWebRequest Create(string url, MyWebRequest AliveRequest, bool bKeepAlive)
        {
          return  Create(new Uri(url), AliveRequest, bKeepAlive);
        }
        public static MyWebRequest Create(Uri uri, MyWebRequest AliveRequest, bool bKeepAlive)
        {
            if (bKeepAlive &&
                AliveRequest != null &&
                AliveRequest.response != null &&
                AliveRequest.response.KeepAlive &&
                AliveRequest.response.socket.Connected &&
                AliveRequest.RequestUri.Host == uri.Host)
            {
                AliveRequest.RequestUri = uri;
                return AliveRequest;
            }
            return new MyWebRequest(uri, bKeepAlive);
        }
        public MyWebResponse GetResponse()
        {
            if (response == null || response.socket == null || response.socket.Connected == false)
            {
                response = new MyWebResponse();
                response.Connect(this);
                response.SetTimeout(Timeout);
            }
            response.SendRequest(this);
            response.ReceiveHeader();
            return response;
        }

        public int Timeout;
        public WebHeaderCollection Headers;
        public string Header;
        public Uri RequestUri;
        public string Method;
        public MyWebResponse response;
        public bool KeepAlive;
    }
    public class MyWebResponse
    {
        public MyWebResponse()
        {
        }
        public void Connect(MyWebRequest request)
        {
            ResponseUri = request.RequestUri;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remoteEP = new IPEndPoint(Dns.Resolve(ResponseUri.Host).AddressList[0], ResponseUri.Port);
            socket.Connect(remoteEP);
        }
        public void SendRequest(MyWebRequest request)
        {
            ResponseUri = request.RequestUri;

            request.Header = request.Method + " " + ResponseUri.PathAndQuery + " HTTP/1.0\r\n" + request.Headers;
            socket.Send(Encoding.ASCII.GetBytes(request.Header));
        }
        public void SetTimeout(int Timeout)
        {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, Timeout * 1000);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, Timeout * 1000);
        }
        public void ReceiveHeader()
        {
            Header = "";
            Headers = new WebHeaderCollection();

            byte[] bytes = new byte[10];
            while (socket.Receive(bytes, 0, 1, SocketFlags.None) > 0)
            {
                Header += Encoding.ASCII.GetString(bytes, 0, 1);
                if (bytes[0] == '\n' && Header.EndsWith("\r\n\r\n"))
                    break;
            }
            MatchCollection matches = new Regex("[^\r\n]+").Matches(Header.TrimEnd('\r', '\n'));
            for (int n = 1; n < matches.Count; n++)
            {
                string[] strItem = matches[n].Value.Split(new char[] { ':' }, 2);
                if (strItem.Length > 0)
                    Headers[strItem[0].Trim()] = strItem[1].Trim();
            }
            // check if the page should be transfered to another location
            if (matches.Count > 0 && (
                matches[0].Value.IndexOf(" 302 ") != -1 ||
                matches[0].Value.IndexOf(" 301 ") != -1))
                // check if the new location is sent in the "location" header
                if (Headers["Location"] != null)
                {
                    try { ResponseUri = new Uri(Headers["Location"]); }
                    catch { ResponseUri = new Uri(ResponseUri, Headers["Location"]); }
                }
            ContentType = Headers["Content-Type"];
            if (Headers["Content-Length"] != null)
                ContentLength = int.Parse(Headers["Content-Length"]);
            KeepAlive = (Headers["Connection"] != null && Headers["Connection"].ToLower() == "keep-alive") ||
                        (Headers["Proxy-Connection"] != null && Headers["Proxy-Connection"].ToLower() == "keep-alive");
        }
        public void Close()
        {
            socket.Close();
        }
        public Uri ResponseUri;
        public string ContentType;
        public int ContentLength;
        public WebHeaderCollection Headers;
        public string Header;
        public Socket socket;
        public bool KeepAlive;
    }