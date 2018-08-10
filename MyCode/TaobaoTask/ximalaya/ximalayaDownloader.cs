using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaobaoTask.ximalaya.entity;

namespace TaobaoTask.ximalaya
{
    public class ximalayaDownloader
    {
        public class Args
        {
            public string baseDir;
            public int albumId;
            public string albumName;
        }
        private static string baseUrl = "http://www.ximalaya.com/";
        ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 4 };
        public void downloadAlbum(int albumId, string albumName)
        {
            downloadAlbum(null, albumId, albumName);
        }
        public void downloadAlbum(string baseDir, int albumId, string albumName)
        {
            var args = new Args { baseDir = baseDir, albumId = albumId, albumName = albumName };
            downloadAlbum(args);
        }
        private void downloadAlbum(Args args)
        {
            var restClient = RestClientUtil.GetRestClient(baseUrl);
            downloadAlbum(restClient, args, 1);
        }
        private void downloadAlbum(RestSharp.IRestClient restClient, Args args, int pageNum)
        {
            string url = string.Format("revision/album/getTracksList?albumId={0}&pageNum={1}", args.albumId, pageNum);
            string json = string.Empty;
            var resp = RestClientUtil.Get(restClient, url);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var root = JsonConvert.DeserializeObject<getTracksListRoot>(resp.Content);
                if (root != null && root.data != null)
                {
                    if (root.data.tracks.Any())
                    {

                        CountdownEvent cde = new CountdownEvent(root.data.tracks.Count);

                        Parallel.ForEach(root.data.tracks, parallelOptions, track =>
                       {
                           try
                           {
                               downloadTrack(restClient, args, track);
                           }
                           catch (Exception ex)
                           {
                               Console.WriteLine(string.Format("{0},{1},{2},{3}", args.albumId, track.index, track.title, ex.StackTrace));
                           }
                           cde.Signal();
                       });
                        System.Threading.Thread.Sleep(1000 * 10);
                        cde.Wait();
                        downloadAlbum(restClient, args, root.data.pageNum + 1);
                    }


                }
            }
        }
        private void downloadTrack(RestSharp.IRestClient restClient, Args args, TracksItem track)
        {
            string url = string.Format("revision/play/tracks?trackIds={0}", track.trackId);
            var resp = RestClientUtil.Get(restClient, url);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var mp4src = RestClientUtil.RegexGetString("", resp.Content, "\"src\":\"(.*?)\"", 1);
                if (!string.IsNullOrEmpty(mp4src))
                {
                    downloadMp4(args, track, mp4src);
                }
            }
        }
        private void downloadMp4(Args args, TracksItem track, string mp4src)
        {
            var url = mp4src;
            var baseUrl = "http://audio.xmcdn.com";
            var restClient2 = RestClientUtil.GetRestClient(baseUrl, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            var resp = RestClientUtil.Get(restClient2, url.Replace(baseUrl, ""));
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var bytes = resp.RawBytes;
                string fileExt = Path.GetExtension(mp4src);
                string filePath = String.Format("{0}\\{1}\\{2}_{3}\\{4}_{5}_{6}{7}", @"E:\MUSIC\ximalaya\\", args.baseDir ?? string.Empty,
                    args.albumId, args.albumName,
                    track.index.ToString().PadLeft(3, '0'), track.trackId, getFilePath(track.title), fileExt);
                string dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                if (!File.Exists(filePath))
                {
                    File.WriteAllBytes(filePath, bytes);
                }
                Console.WriteLine(string.Format("save:{0},{1},{2},{3}", resp.StatusCode, args.albumId, track.index, track.title));
            }
            else
            {
                Console.WriteLine(string.Format("err :{0},{1},{2},{3}", resp.StatusCode, args.albumId, track.index, track.title));
            }
        }
        private string getFilePath(string file)
        {
            var chars = Path.GetInvalidFileNameChars();
            foreach (var c in chars)
            {
                file = file.Replace(c, '_');
            }
            return file;
        }
    }

}
