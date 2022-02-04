using System.Linq;

namespace WebImageDownloader
{
    public static class CharGen
    {
        public static char[] GenerateChars(CharSet type)
        {
            switch (type)
            {
                case CharSet.Numbers:
                    return new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                case CharSet.LettersLowerCase:
                    return new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
                case CharSet.LettersUpperCase:
                    return new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

                default:
                    return new char[0];
            }
        }

        public static char[] CombineChars(this char[] charS1, char[] charS2) => charS1.Concat(charS2).ToArray();
    }

    public enum CharSet
    {
        Numbers,
        LettersLowerCase,
        LettersUpperCase,
    }
}
