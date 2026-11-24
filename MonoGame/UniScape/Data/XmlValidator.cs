using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace UniScape.Data
{
    // Checks that an XML file is well-formed and valid against its XSD.
    // The XmlReader runs in validation mode and we collect every error with its line number
    // instead of stopping at the first one. The XML file itself is only read.
    public static class XmlValidator
    {
        // Returns the list of problems, empty means the document is valid
        public static List<string> Validate(string xmlPath, string xsdPath)
        {
            using var file = new StreamReader(xmlPath);
            return Validate(file, xsdPath);
        }

        // Same thing for XML text that isn't in a file, like a string in memory
        public static List<string> Validate(TextReader xml, string xsdPath)
        {
            var errors = new List<string>();

            var schemas = new XmlSchemaSet();
            // Without a resolver the include of assets.xsd inside floor.xsd isn't found
            schemas.XmlResolver = new XmlUrlResolver();
            schemas.Add(null, xsdPath); // null means use the targetNamespace from the schema file

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemas,
            };
            settings.ValidationEventHandler += (sender, e) =>
                errors.Add($"line {e.Exception.LineNumber}: {e.Message}");

            try
            {
                using XmlReader reader = XmlReader.Create(xml, settings);
                while (reader.Read())
                {
                    // Nothing to do here, reading the nodes is what runs the validation
                }
            }
            catch (XmlException e)
            {
                errors.Add($"not well-formed: {e.Message}"); // e.g. a tag that's never closed
            }

            return errors;
        }
    }
}
