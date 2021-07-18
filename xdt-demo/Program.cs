using System;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Web.XmlTransform;

namespace xdt_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load("catalog.xml");

            ApplyTransforms(xmlDocument, xmlDocument);

            Console.WriteLine(PrettyXml(xmlDocument));
            Console.ReadKey();
        }

        static void ApplyTransforms(XmlDocument xmlDocument, XmlDocument includesXmlDocument)
        {
            var includeNodes = includesXmlDocument.SelectNodes("//includes/include").Cast<XmlNode>();

            foreach (var includeNode in includeNodes)
            {
                var filename = includeNode.Attributes["filename"].Value;
                var xmlTransformation = new XmlTransformation(filename);
                xmlTransformation.Apply(xmlDocument);

                var includesXml = new XmlDocument();
                includesXml.Load(filename);
                ApplyTransforms(xmlDocument, includesXml);
            }
        }
        
        static string PrettyXml(XmlDocument element)
        {
            var stringBuilder = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = true
            };

            using var xmlWriter = XmlWriter.Create(stringBuilder, settings);
            element.Save(xmlWriter);

            return stringBuilder.ToString();
        }
    }
}