using System;
using System.Linq;

namespace InterviewTask.Helpers
{
    /// <summary>
    /// Contains helper methods to get random strings
    /// </summary>
    public class StringHelper
    {
        private readonly Random _random = new Random();
        private const string Chars = "abcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// Gets a random alphanumeric string
        /// </summary>
        /// <param name="length"></param>
        /// <returns>A random alphanumeric string</returns>
        public string GetRandomString(int length)
        {
            return new string(Enumerable.Repeat(Chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Gets a random alphanumeric email
        /// </summary>
        /// <param name="length"></param>
        /// <returns>A random alphanumeric email</returns>
        public string GetRandomEmail(int length)
        {
            return new string(Enumerable.Repeat(Chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray()) + "@" + GetRandomString(4) + "." + GetRandomString(3);
        }
    }
}
