using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace WebImageDownloader
{
    public class ImageDownloader
    {
        public bool DownloadImageFromUrl(string url, string folderImagesPath, string logFilePath = "")
        {
            var uri = new Uri(url);
            var pages = new List<HtmlNode> { LoadHtmlDocument(uri) };
            bool writeLogs = !string.IsNullOrEmpty(logFilePath);

            //pages.AddRange(LoadOtherPages(pages[0], url));    
            Regex imgReg = new Regex(@"src=\Dhttps:\/\/i\.imgur\.com\/[0-9a-zA-Z]{7}.[a-z]{3}");
            string data = pages[0].OuterHtml;
            bool noImageFound = !imgReg.IsMatch(data);
            if (noImageFound)
                return false;

            string imgurl = imgReg.Match(data).Value.Substring(5);
            DownloadImage(folderImagesPath, new Uri(imgurl), new WebClient());

            if (writeLogs)
                WriteToLog(logFilePath, url.Substring(16), imgurl.Substring(20, 7));

            return true;
        }

        private static void DownloadImage(string folderImagesPath, Uri url, WebClient webClient)
        {
            webClient.DownloadFile(url, Path.Combine(folderImagesPath, Path.GetFileName(url.ToString())));
        }

        private static IEnumerable<HtmlNode> LoadOtherPages(HtmlNode firstPage, string url)
        {
            return Enumerable.Range(1, DiscoverTotalPages(firstPage))
                             .AsParallel()
                             .Select(i => LoadHtmlDocument(new Uri(url)));
        }

        public static void WriteToLog(string logFilePath, string imageCode, string imageName)
        {
            StreamWriter stw = new StreamWriter(logFilePath, true);
            stw.WriteLine(imageCode + ":" + imageName);
            stw.Dispose();
        }

        private static int DiscoverTotalPages(HtmlNode documentNode)
        {
            var cv = documentNode.SelectNodes("//div[@class='catalogItemList__numsInWiev']");
            if (cv == null)
                return 0;
            var totalItemsDescription = cv.First().InnerText.Trim();
            var totalItems = int.Parse(Regex.Match(totalItemsDescription, @"\d+$").ToString());
            var totalPages = (int)Math.Ceiling(totalItems / 50d);
            return totalPages;
        }

        private static HtmlNode LoadHtmlDocument(Uri uri)
        {
            var doc = new HtmlDocument();
            var wc = new WebClient();
            wc.Headers.Add("User-Agent: Other");
            doc.LoadHtml(wc.DownloadString(uri));

            var documentNode = doc.DocumentNode;
            return documentNode;
        }
    }
}
