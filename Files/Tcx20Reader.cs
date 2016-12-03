using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Altidude.Files
{
   public class Tcx20Reader
   {
      private const string TcxSchemaPath = @".\Schemas\Tcx20.xsd";

      public static Tcx20.TrainingCenterDatabase_t Open(string path)
      {
         //var settings = new XmlReaderSettings {CheckCharacters = true, CloseInput = true};
         //settings.Schemas.Add(null, TcxSchemaPath);
         //settings.ValidationType = ValidationType.Schema;
         //settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
         //settings.ValidationEventHandler += new ValidationEventHandler(settings_ValidationEventHandler);

         Tcx20.TrainingCenterDatabase_t tcx;

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

            var serializer = new XmlSerializer(typeof(Tcx20.TrainingCenterDatabase_t));
            tcx = (Tcx20.TrainingCenterDatabase_t)serializer.Deserialize(xmlReader);

         }

         return tcx;
      }

      public static Tcx20.TrainingCenterDatabase_t Open(Stream stream)
      {
         return Open(stream, false);
      }

      public static Tcx20.TrainingCenterDatabase_t Open(Stream stream, bool validate)
      {
         Tcx20.TrainingCenterDatabase_t tcx;

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

            XmlSerializer serializer = new XmlSerializer(typeof(Tcx20.TrainingCenterDatabase_t));
            tcx = (Tcx20.TrainingCenterDatabase_t)serializer.Deserialize(xmlReader);

         }

         return tcx;
      }

      private static XmlReader CreateXmlReader(Stream stream, bool validate)
      {
         XmlReader reader;

         if (validate)
         {
            var settings = new XmlReaderSettings {CheckCharacters = true, CloseInput = true};

            settings.Schemas.Add(null, TcxSchemaPath);
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