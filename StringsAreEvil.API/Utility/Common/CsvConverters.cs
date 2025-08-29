using GlobalCommon=Global.Utility.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StringsAreEvil.API.Utility.Common;

public static class CsvConverters
{
    public static string Generate<T>(List<T> items) where T : class
    {
        var csv = "";
        var properties = typeof(T).GetProperties()
            .Where(n =>
                n.PropertyType == typeof(string)
                || n.PropertyType == typeof(bool)
                || n.PropertyType == typeof(char)
                || n.PropertyType == typeof(byte)
                || n.PropertyType == typeof(decimal)
                || n.PropertyType == typeof(int)
                || n.PropertyType == typeof(DateTime)
                || n.PropertyType == typeof(DateTime?));

        using (var sw = new StringWriter())
        {
            var header = properties
                .Select(n => n.Name)
                .Aggregate((a, b) => String.Format("{0}{1}{2}", a, GlobalCommon.Constants.Delimiter, b));

            sw.WriteLine(String.Format("{0}", header.Replace(GlobalCommon.Constants.Delimiter, ",")));

            foreach (var item in items)
            {
                var row = properties
                    .Select(n => n.GetValue(item, null))
                    .Select(n => n == null ? "null" : n.ToString()).Aggregate((a, b) => String.Format("{0}{1}{2}", a, GlobalCommon.Constants.Delimiter, b));

                sw.WriteLine(String.Format("\"{0}\"", row.Replace(GlobalCommon.Constants.Delimiter, "\",\"")));
            }

            csv = sw.ToString();
        }
        return csv;
    }
}
