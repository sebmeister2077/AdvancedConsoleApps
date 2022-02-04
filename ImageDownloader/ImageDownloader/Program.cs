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
            DownloadImages("https://prnt.sc/", "1008lu", "zzzzzz", passChars, );
        }

        private static void DownloadImages(string downloadUrl, string startingValue, string endValue, char[] passChars, uint? maxAmount)
        {
            uint imageCount = 0;
            if(!maxAmount.HasValue)
                maxAmount = uint.MaxValue;
            ImageDownloader imgDownloader = new ImageDownloader();

            while (imageCount < maxAmount)
            {
                bool downloadSuccess = imgDownloader.DownloadImageFromUrl(downloadUrl + startingValue, @"D:\Images\");

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
