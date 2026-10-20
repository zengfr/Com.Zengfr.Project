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
    public class ximalayaDownloaderV2
    {
        public class Args
        {
            public string baseDir;
            public int albumId;
            public string albumName;
            public string xmsign;
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
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            downloadAlbum(restClient, args, 1);
        }
        private void downloadAlbum(RestSharp.IRestClient restClient, Args args, int pageNum)
        {
            args.xmsign = getxmsign();// "5f0dbf011777bb4c27f098ff1409a89f(52)1563202708483(51)1563202709081";
            string url = string.Format("revision/play/album?albumId={0}&pageNum={1}&sort=0&pageSize=30", args.albumId, pageNum);
            string json = string.Empty;
            RestClientUtil.SetDefaultHeader(restClient, "xm-sign", args.xmsign);
            var resp = RestClientUtil.Get(restClient, url, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var root = JsonConvert.DeserializeObject<playalbumlist>(resp.Content);
                if (root != null && root.data != null)
                {
                    if (root.data.tracksAudioPlay.Any())
                    {

                        CountdownEvent cde = new CountdownEvent(root.data.tracksAudioPlay.Count);

                        Parallel.ForEach(root.data.tracksAudioPlay, parallelOptions, track =>
                       {
                           try
                           {
                               downloadTrack(restClient, args, track);
                           }
                           catch (Exception ex)
                           {
                               Console.WriteLine(string.Format("{0},{1},{2},{3}", args.albumId, track.index, track.trackName, ex.StackTrace));
                           }
                           cde.Signal();
                       });
                        System.Threading.Thread.Sleep(333);
                        Console.WriteLine("Wait Start");
                        cde.Wait();
                        Console.WriteLine("Wait End");
                        downloadAlbum(restClient, args, root.data.pageNum + 1);
                    }


                }
            }
        }
        private void downloadTrack(RestSharp.IRestClient restClient, Args args, TracksAudioPlayItem track)
        {

            var mp4src = track.src;
            if (!string.IsNullOrEmpty(mp4src))
            {
                downloadMp4(args, track, mp4src);
            }

        }
        private void downloadMp4(Args args, TracksAudioPlayItem track, string mp4src)
        {
            var url = mp4src;
            string fileExt = Path.GetExtension(mp4src);
            string filePath = String.Format("{0}\\{1}\\{2}_{3}\\{4}_{5}_{6}{7}", @"E:\MUSIC\ximalaya\\", args.baseDir ?? string.Empty,
                args.albumId, args.albumName,
                track.index.ToString().PadLeft(3, '0'), track.trackId, getFilePath(track.trackName), fileExt);
            if ((!File.Exists(filePath)) || getFileSize(filePath) < 1)
            {
                var baseUrl = url;
                var restClient2 = RestClientUtil.GetRestClient(baseUrl, "text/html,application/xhtml+xml,application/xml,audio/x-m4a;q=0.9,image/webp,image/apng,*/*;q=0.8", true);
                Console.WriteLine(string.Format("get:{0},{1},{2},{3}", args.albumId, track.index, track.trackName, url));
                RestClientUtil.SetDefaultHeader(restClient2, "xm-sign", getxmsign());
                var resp = RestClientUtil.Get(restClient2, url.Replace(baseUrl, ""), null);
                if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var bytes = resp.RawBytes;
                    string dir = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    var fNotExists = (!File.Exists(filePath)) || getFileSize(filePath) < 1;
                    if (fNotExists)
                    {
                        File.WriteAllBytes(filePath, bytes);
                    }
                    Console.WriteLine(string.Format("save:{0},{1},{2},{3},{4},{5}", resp.StatusCode, bytes.LongLength, fNotExists, args.albumId, track.index, track.trackName));
                }
                else
                {
                    Console.WriteLine(string.Format("err :{0},{1},{2},{3},{4}", resp.StatusCode, resp.ErrorMessage, args.albumId, track.index, track.trackName));
                }
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
        private long getFileSize(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            return fileInfo.Length;
        }
        public static string getxmsign()
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "ximalaya\\xmsign.js";
            //加载js文件的所有内容 保存到变量str2中
            string str2 = System.IO.File.ReadAllText(path);
            //调用js方法,如果该js方法有参数,就传呗
            var time = getTime();
            string fun = string.Format(@"python('{0}')", time);
            //执行，并得到返回结果
            string result = JsUtil.ExecuteScript(fun, str2);
            return result;
        }
        public static string getTime()
        {
            var url = "revision/time";
            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            var resp = RestClientUtil.Get(restClient, url, null);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return resp.Content;
            }
            return string.Empty;
        }
    }

}
