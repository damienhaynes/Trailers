using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Trailers.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Find any kind of whitespace (e.g. tabs, newlines, etc.) and replace them with a single space.
        /// </summary>
        public static string ReplaceMultiSpaceWithSingleWhiteSpace(this string words)
        {
            return Regex.Replace(words, @"\s+", " ");
        }
    }
}
