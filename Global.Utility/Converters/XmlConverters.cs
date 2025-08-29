using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace Global.Utility.Converters;

public static class XmlConverters
{
    public static string Beautify(string xml)
    {
        StringBuilder sb = new StringBuilder();

        XmlDocument doc = new XmlDocument();
        try
        {
            doc.LoadXml(xml);

            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = Encoding.Unicode,
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    doc.Save(writer);
                }
                sb.Append(Encoding.Unicode.GetString(ms.ToArray()));
            }

            Debug.WriteLine(String.Format("\n{0}\n", sb.ToString()));
        }
        catch (XmlException ex)
        {
            Debug.WriteLine(ex);
        }

        return sb.ToString();
    }

    public static string Serialize(string xml)
    {
        StringBuilder sb = new StringBuilder();

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        sb.Append(JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None));

        Debug.WriteLine(String.Format("\n{0}\n", JsonConverters.Beautify(sb.ToString())));

        return sb.ToString();
    }

    public static string Validate(string xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        using (MemoryStream ms = new MemoryStream())
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.Unicode,
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(ms, settings))
            {
                doc.Save(writer);
            }
        }

        return doc.OuterXml;
    }
}
