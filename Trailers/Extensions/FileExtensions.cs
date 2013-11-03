using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Trailers.Extensions
{
    public static class FileExtensions
    {
        public static string ToCleanFileName(this string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
    }
}
