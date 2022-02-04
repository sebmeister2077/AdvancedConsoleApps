using System.Linq;
using System.Text;

namespace WebImageDownloader
{
    static class Program
    {
        //the characters you want to build a string with
        static char[] passChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        static bool finish = false;
        static string downloadUrl = "https://prnt.sc/";
        static string startingValue = "1008lu";

        static void Main(string[] args)
        {
            while (!finish)
            {
                //Process.Start("chrome.exe", domain + value);
                //Regex imgReg = new Regex(@"https:\/\/i\.imgur\.com\/[0-9a-zA-Z]{7}.[a-z]{3}");
                //Console.WriteLine(imgReg.Match("https://i.imgur.com/djB6j0B.png").Success);
                ImageDownloader imgDownloader = new ImageDownloader();
                imgDownloader.DownloadImageFromUrl(downloadUrl + startingValue, @"D:\Images\");
                //WebClient client = new WebClient();
                //client.DownloadFile(domain + value, basePath+$"image{x}");

                if (startingValue == "zzzzzz")
                    break;
                startingValue = GoNext(startingValue);
            }
        }
        static public char NextChar(char c) => passChars[passChars.ToList().IndexOf(c) + 1];
        static public string GoNext(string text)
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
                    str[x] = NextChar(str[x]);
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
