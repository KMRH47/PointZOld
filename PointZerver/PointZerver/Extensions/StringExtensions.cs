using System.Collections.Generic;

namespace PointZerver.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Scans a string for the passed character separator.<br/>
        /// When the character is found the amount of times specified, return that string array.
        /// </summary>
        /// <param name="separator">The character that defines where to separate the string.</param>
        /// <param name="separationCount">The amount of times to separate the string.</param>
        /// <returns></returns>
        // ReSharper disable once InvalidXmlDocComment
        public static string[] SplitTo(this string s, char separator, int separationCount)
        {
            List<string> strings = new();
            string currentString = "";
            int hits = 0;

            foreach (char c in s)
            {
                if (c == separator)
                {
                    strings.Add(currentString);
                    currentString = string.Empty;
                    hits++;
                    if (hits == separationCount) break;
                }
                else currentString += c;
            }

            return strings.ToArray();
        }

        public static string SplitFirst(this string s, char separator)
        {
            string message = "";

            foreach (char c in s)
            {
                if (c != separator)
                {
                    message += c;
                }
                else break;
            }

            return message;
        }

        public static string TakeFrom(this string s, char separator, int startIndex)
        {
            string currentString = "";

            for (int i = startIndex; i < s.Length; i++)
            {
                currentString += s[i];
            }

            return currentString;
        }

        public static string TakeFromToNext(this string s, char separator, int startIndex)
        {
            string currentString = "";

            for (int i = startIndex; i < s.Length; i++)
            {
                if (s[i] != separator)
                {
                    currentString += s[i];
                }
                else break;
            }

            return currentString;
        }
    }
}