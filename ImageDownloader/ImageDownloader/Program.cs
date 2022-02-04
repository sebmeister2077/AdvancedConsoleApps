using System;
using System.Linq;
using System.Text;

namespace WebImageDownloader
{
    static class Program
    {
        static void Main(string[] args)
        {
            //the characters you want to build a code with
            char[] passChars = GenerateWantedChars();
            DownloadImages("https://prnt.sc/", @"D:\Images\", "1008lu", "zzzzzz", passChars , new ExtraOptions());
        }

        /// <summary>
        /// Downloads images from a url
        /// </summary>
        private static void DownloadImages(string downloadUrl, string savePath, string startingValue, string endValue, char[] passChars, ExtraOptions options)
        {
            uint imageCount = 0;
            ImageDownloader imgDownloader = new ImageDownloader();

            while (imageCount < options.MaxAmount)
            {
                bool downloadSuccess = imgDownloader.DownloadImageFromUrl(downloadUrl + startingValue, savePath,options.ToDownloaderOptions());

                if (downloadSuccess)
                    imageCount++;


                if (startingValue == endValue)
                    break;
                startingValue = GoNext(startingValue, passChars);
            }
        }

        static public char NextChar(char c, char[] passChars) => passChars[passChars.ToList().IndexOf(c) + 1];

        static public string GoNext(string text, char[] passChars)
        {
            char[] str = text.ToCharArray();
            int charsLen = passChars.Length;
            int x = text.Length - 1;
            if (str.All(c => c == passChars[charsLen - 1]))
            {
                StringBuilder bob = new StringBuilder();
                for (int i = 0; i < text.Length + 1; i++)
                    bob.Append(passChars[0].ToString());
                return bob.ToString();
            }
            while (x >= 0)
            {
                if (str[x] != passChars[charsLen - 1])
                {
                    str[x] = NextChar(str[x], passChars);
                    return str.CharsToString();
                }
                else
                    str[x] = passChars[0];
                x--;
            }
            return "";
        }

        public static char[] GenerateWantedChars()
        {
            return CharGen.GenerateChars(CharSet.Numbers).CombineChars(CharGen.GenerateChars(CharSet.LettersLowerCase));
        }

        public class ExtraOptions
        {
            public uint MaxAmount { get; set; }

            public string LogsFilePath { get; set; }

            public ExtraOptions()
            {
                LogsFilePath = "";
                MaxAmount = uint.MaxValue;
            }

            public ExtraOptions(string LogsFilePath) : this()
            {
                this.LogsFilePath = LogsFilePath;
            }

            public ExtraOptions(uint MaxAmount) : this()
            {
                this.MaxAmount = MaxAmount;
            }

            public ExtraOptions(uint MaxAmount, string LogsFilePath)
            {
                this.LogsFilePath = LogsFilePath;
                this.MaxAmount = MaxAmount;
            }

            public ImageDownloaderOptions ToDownloaderOptions()
            {
                return new ImageDownloaderOptions(this.LogsFilePath);
            }
        }

    }
    public static class StringExtension
    {
        public static string CharsToString(this char[] chars)
        {
            StringBuilder bob = new StringBuilder();
            foreach (char c in chars)
                bob.Append(c);
            return bob.ToString();
        }
    }
}
