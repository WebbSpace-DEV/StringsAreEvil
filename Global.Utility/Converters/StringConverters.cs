using System.Globalization;
using System.Text;

namespace Global.Utility.Converters;

public static class StringConverters
{
    public static string CleanFileName(string s)
    {
        StringBuilder sb = new StringBuilder(s);
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            sb.Replace(c.ToString(), String.Empty);
        }
        return sb.ToString();
    }

    public static string ToProper(string s)
    {
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        return textInfo.ToTitleCase(s.ToLower());
    }
}
