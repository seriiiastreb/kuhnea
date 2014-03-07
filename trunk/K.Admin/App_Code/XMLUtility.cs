using System;
using System.Xml;
/// <summary>
/// Summary description for XMLUtility
/// </summary>
public class XMLUtility
{
    public static XmlAttribute CreateAttribute(XmlDocument _xmlDocument, string _attributeName, string _value)
    {
        XmlAttribute _attribute = _xmlDocument.CreateAttribute(_attributeName);
        _attribute.Value = _value;

        return _attribute;
    }

    public static XmlElement CreateXmlNode(XmlDocument _xmlDocument, string _nodeName)
    {
        XmlElement _node = _xmlDocument.CreateElement(_nodeName);
        return _node;
    }

    public static XmlElement CreateXmlNode(XmlDocument _xmlDocument, string _nodeName, string _nodeInnerText)
    {
        XmlElement _node = _xmlDocument.CreateElement(_nodeName);
        _node.InnerText = _nodeInnerText;
        return _node;
    }

    public static XmlElement CreateXmlNode(XmlDocument _xmlDocument, string _prefix, string _nodeName, string _nodeInnerText, string _rootLink)
    {
        XmlElement _node = _xmlDocument.CreateElement(_prefix, _nodeName, _rootLink);
        _node.InnerText = _nodeInnerText;
        return _node;
    }

    public static XmlElement CreateXmlNode(XmlDocument _xmlDocument, string _nodeName, XmlNode _childNode)
    {
        XmlElement _node = _xmlDocument.CreateElement(_nodeName);
        _node.AppendChild(_childNode);
        return _node;
    }
}