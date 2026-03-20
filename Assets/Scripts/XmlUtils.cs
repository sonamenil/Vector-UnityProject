using Core._Common;
using System;
using System.IO;
using System.Xml;
using UnityEngine;

public static class XmlUtils
{
    public enum OpenXmlType
    {
        Normal = 0,
        ForcedResourced = 1,
        ForcedExternal = 2
    }

    public const string Comment = "#comment";

    public static bool IsNodeComment(XmlNode node)
    {
        return node.NodeType == XmlNodeType.Comment;
    }

    public static int ParseInt(this XmlAttribute attr, int defVal = 0)
    {
        if (attr == null)
        {
            return defVal;
        }
        return int.Parse(attr.Value);
    }

    public static long ParseLong(XmlAttribute attr, long defVal = 0L)
    {
        if (attr == null)
        {
            return defVal;
        }
        return long.Parse(attr.Value);
    }

    public static uint ParseUint(XmlAttribute attr, uint defVal = 0u)
    {
        if (attr == null)
        {
            return defVal;
        }
        return uint.Parse(attr.Value);
    }

    public static float ParseFloat(this XmlAttribute attr, float defVal = 0f)
    {
        if (attr == null)
        {
            return defVal;
        }
        return float.Parse(attr.Value);
    }

    public static double ParseDouble(this XmlAttribute attr, double defVal = 0.0)
    {
        if (attr == null)
        {
            return defVal;
        }
        return double.Parse(attr.Value);
    }

    public static bool ParseBool(this XmlAttribute attr, bool defVal = false)
    {
        if (attr == null)
        {
            return defVal;
        }
        return int.Parse(attr.Value) > 0;
    }

    public static string ParseString(this XmlAttribute attr, string defVal = null)
    {
        if (attr == null)
        {
            return defVal;
        }
        return attr.Value;
    }

    public static T ParseEnum<T>(XmlAttribute attr, T defVal)
    {
        if (attr == null)
        {
            return defVal;
        }
        try
        {
            return (T)Enum.Parse(typeof(T), attr.Value, true);
        }
        catch
        {
            return defVal;
        }
    }

    public static XmlDocument OpenXMLDocumentFromBytes(byte[] pBytes, bool pIgnoreComments = true)
    {
        XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
        xmlReaderSettings.IgnoreComments = pIgnoreComments;
        try
        {
            MemoryStream stream = new MemoryStream(pBytes);
            using (XmlReader p_reader = XmlReader.Create(stream, xmlReaderSettings))
            {
                return OpenXMLDocument(p_reader);
            }
        }
        catch
        {
            return null;
        }
    }

    public static T DeserializeXMLDocument<T>(string path, string file = "", OpenXmlType openType = OpenXmlType.Normal, bool pIgnoreComments = true)
    {
        return default(T);
    }

    public static XmlDocument OpenXMLDocument(string path, string file = "", OpenXmlType openType = OpenXmlType.Normal, bool pIgnoreComments = true)
    {
        string fullPath = System.IO.Path.Combine(path, file);
        var xmlReaderSettings = new XmlReaderSettings();
        xmlReaderSettings.IgnoreComments = pIgnoreComments;
        XmlDocument document = null;
        switch (openType)
        {
            case OpenXmlType.Normal:
                if (path.StartsWith(Application.streamingAssetsPath))
                {
                    if (!File.Exists(fullPath))
                    {
                        DebugUtils.Dialog("FILE DOESNT EXIST: " + fullPath, true, true);
                        return null;
                    }
                    var text0 = System.IO.File.ReadAllText(fullPath);
                    document = XmlUtils.OpenXMLDocFromString(text0, xmlReaderSettings);
                    break;
                }
                var text = ResourceManager.GetTextFromResources(fullPath);
                document = XmlUtils.OpenXMLDocFromString(text, xmlReaderSettings);
                break;
            case OpenXmlType.ForcedResourced:
                var text1 = ResourceManager.GetTextFromResources(fullPath);
                document = XmlUtils.OpenXMLDocFromString(text1, xmlReaderSettings);
                break;
            case OpenXmlType.ForcedExternal:
                var text2 = System.IO.File.ReadAllText(fullPath);
                document = XmlUtils.OpenXMLDocFromString(text2, xmlReaderSettings);
                break;
            default:
                var input = File.Open(fullPath, FileMode.Open);
                var reader = XmlReader.Create(input);
                document = XmlUtils.OpenXMLDocument(reader);
                break;
        }
        return document;
    }

    public static XmlElement OpenXMLElementFromString(string content, bool pIgnoreComments = true)
    {
        if (string.IsNullOrEmpty(content))
        {
            return null;
        }
        XmlDocument doc = OpenXMLDocumentFromString(content);
        if (doc != null)
        {
            return doc.DocumentElement;
        }
        return null;
    }

    public static XmlDocument OpenXMLDocumentFromString(string content, bool pIgnoreComments = true)
    {
        using (XmlReader p_reader = XmlReader.Create(new StringReader(content), new XmlReaderSettings()))
        {
            XmlDocument xmlDocument = OpenXMLDocument(p_reader);
            if (xmlDocument == null)
            {
                DebugUtils.Dialog("Error read XML", true);
            }
            return xmlDocument;
        }
    }

    public static XmlDocument OpenXMLDocFromTextAsset(TextAsset pText, bool pIgnoreComments = true)
    {
        return null;
    }

    private static XmlDocument OpenXMLDocFromString(string pData, XmlReaderSettings pSettings)
    {
        using (XmlReader p_reader = XmlReader.Create(new StringReader(pData), pSettings))
        {
            XmlDocument xmlDocument = OpenXMLDocument(p_reader);
            if (xmlDocument == null)
            {
                DebugUtils.Dialog("Error read XML", true);
            }
            return xmlDocument;
        }
    }

    private static XmlDocument OpenXMLDocument(XmlReader pReader)
    {
        try
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(pReader);
            return xmlDocument;
        }
        catch
        {
            return null;
        }
    }

    public static void CopyXmlFromResources(string pFrom, string pTo)
    {
    }

    public static void CopyXmlAndTrimSpaces(string pFrom, string pTo)
    {
    }

    public static void TrimSpacesFromXmlInDirectory(string pPath, bool pRecursively = false)
    {
    }

    public static bool IsFileValid(string pPath)
    {
        return false;
    }
}
