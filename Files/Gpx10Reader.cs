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
  public class Gpx10Reader
  {
    private const string GpxSchemaPath = @"C:\temp\gps\gpx10.xsd";

    public static Gpx10.gpx Open(string path)
    {
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.CheckCharacters = true;
      settings.CloseInput = true;
      settings.Schemas.Add(null, GpxSchemaPath);
      settings.ValidationType = ValidationType.Schema;
      settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
      settings.ValidationEventHandler += new ValidationEventHandler(settings_ValidationEventHandler);

      Gpx10.gpx gpx = null;

      using (XmlReader xmlReader = XmlReader.Create(path, settings))
      {
        if (xmlReader != null)
        {

#if false
          while (xmlReader.Read())
          {
            //empty loop - if Read fails it will raise an error via the 
            //ValidationEventHandler wired to the XmlReaderSettings object
          }

          //explicitly call Close on the XmlReader to reduce strain on the GC
          xmlReader.Close();
#else
            XmlSerializer serializer = new XmlSerializer(typeof(Gpx10.gpx));
          gpx = (Gpx10.gpx)serializer.Deserialize(xmlReader);
#endif
        }
      }

      return gpx;
    }

    static void settings_ValidationEventHandler(object sender, ValidationEventArgs e)
    {
      Debug.WriteLine(e.Message);
    }

    public static Gpx10.gpx Open(Stream stream)
    {
       Gpx10.gpx gpx = null;

       using (XmlReader reader = XmlReader.Create(stream))
       {

          XmlSerializer serializer = new XmlSerializer(typeof(Gpx10.gpx));
          gpx = (Gpx10.gpx)serializer.Deserialize(reader);
       }

       return gpx;
    }

  }
}
