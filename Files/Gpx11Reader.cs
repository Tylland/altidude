using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files
{
    public class Gpx11Reader
    {
        private const string GpxSchemaPath = @".\Schemas\gpx11.xsd";

        public static Gpx11.gpxType Open(string path)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CheckCharacters = true;
            settings.CloseInput = true;
            settings.Schemas.Add(null, GpxSchemaPath);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(settings_ValidationEventHandler);

            Gpx11.gpxType gpx = null;

            using (XmlReader xmlReader = XmlReader.Create(path))
            {
                //if (xmlReader != null)
                //{
                //  while (xmlReader.Read())
                //  {
                //    //empty loop - if Read fails it will raise an error via the 
                //    //ValidationEventHandler wired to the XmlReaderSettings object
                //  }

                //  //explicitly call Close on the XmlReader to reduce strain on the GC
                //  xmlReader.Close();
                //}

                XmlSerializer serializer = new XmlSerializer(typeof(Gpx11.gpxType));
                gpx = (Gpx11.gpxType)serializer.Deserialize(xmlReader);

            }

            return gpx;
        }

        public static Gpx11.gpxType Open(Stream stream)
        {
            return Open(stream, false);
        }

        public static Gpx11.gpxType Open(Stream stream, bool validate)
        {
            Gpx11.gpxType gpx = null;

            using (XmlReader xmlReader = CreateXmlReader(stream, validate))
            {
                //if (xmlReader != null)
                //{
                //  while (xmlReader.Read())
                //  {
                //    //empty loop - if Read fails it will raise an error via the 
                //    //ValidationEventHandler wired to the XmlReaderSettings object
                //  }

                //  //explicitly call Close on the XmlReader to reduce strain on the GC
                //  xmlReader.Close();
                //}

                XmlSerializer serializer = new XmlSerializer(typeof(Gpx11.gpxType));
                gpx = (Gpx11.gpxType)serializer.Deserialize(xmlReader);

            }

            return gpx;
        }

        private static XmlReader CreateXmlReader(Stream stream, bool validate)
        {
            XmlReader reader = null;

            if (validate)
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.CheckCharacters = true;
                settings.CloseInput = true;
                settings.Schemas.Add(null, GpxSchemaPath);
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.ValidationEventHandler += new ValidationEventHandler(settings_ValidationEventHandler);

                reader = XmlReader.Create(stream, settings);
            }
            else
                reader = XmlReader.Create(stream);

            return reader;
        }

        static void settings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Debug.WriteLine(e.Message);
        }
    }
}
