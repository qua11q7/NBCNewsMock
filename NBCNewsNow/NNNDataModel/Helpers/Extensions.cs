using System;
using System.Collections.Generic;
using System.Linq;

namespace NNNDataModel.Helpers
{
    public static class Extensions {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || list.Count() == 0;
        }

        public static string ToLengthyString(this string str, int length = 8)
        {
            if (length < 4)
                length = 4;
            else if (length > 30)
                length = 30;

            if (str.IsNullOrEmpty())
                return string.Join("", Enumerable.Repeat(" ", length));

            if (str.Length < length)
            {
                int emptyCount = length - str.Length;
                int startCount = emptyCount / 2;
                int endCount = emptyCount - startCount;
                string startString = Enumerable.Repeat(" ", startCount).Aggregate("");
                string endString = Enumerable.Repeat(" ", endCount).Aggregate("");
                return startString + str + endString;
            }
            else if (str.Length > length)
            {
                return str.Substring(0, length);
            }
            else
            {
                return str;
            }
        }

        public static string Aggregate(this IEnumerable<string> idList, string delimeter = ",")
        {
            if (idList.IsNullOrEmpty())
                return "";

            return idList.Aggregate((id1, id2) => $"{id1}{delimeter}{id2}");
        }

        public static string ToStr(this Exception ex)
        {
            return $"Message: {ex.Message}\r\nSource: {ex.Source}\r\nTrace: {ex.StackTrace}\r\nSite: {ex.TargetSite}";
        }

    }
}
