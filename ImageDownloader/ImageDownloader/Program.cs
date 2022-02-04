using System;
using System.Linq;
using System.Text;

namespace WebImageDownloader
{
    static class Program
    {

        static void Main(string[] args)
        {
            //the characters you want to build a string with
            char[] passChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            DownloadImages("https://prnt.sc/", "1008lu", "zzzzzz", passChars);
        }

        private static void DownloadImages(string downloadUrl, string startingValue, string endValue, char[] passChars)
        {
            bool finish = false;
            while (!finish)
            {
                ImageDownloader imgDownloader = new ImageDownloader();
                imgDownloader.DownloadImageFromUrl(downloadUrl + startingValue, @"D:\Images\");

                if (startingValue == endValue)
                    break;
                startingValue = GoNext(startingValue, passChars);
            }
        }

        static public char NextChar(char c, char[] passChars) => passChars[passChars.ToList().IndexOf(c) + 1];

        static public string GoNext(string text,char[] passChars)
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
