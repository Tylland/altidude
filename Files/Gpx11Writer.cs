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
    public class Gpx11Writer
    {
        public static void SaveTo(Gpx11.gpxType gpx, Stream stream)
        {
            using (var xmlWriter= XmlWriter.Create(stream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Gpx11.gpxType));
                serializer.Serialize(stream, gpx);
            }
        }

    }
}
