using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Updater
{
    class XMLParser
    {
        public static List<UFile> parseXML(string fileName)
        {
            XDocument doc = XDocument.Load(fileName);
            IEnumerable<XElement> files = doc.Root.Elements();
            
            var Ufiles = new List<UFile>();
            foreach (var xfile in files)
            {
                IEnumerable<XElement> xUrls = xfile.Element("mirrors").Elements();
                var uFile = new UFile(xfile.Element("fileName").Value, xfile.Element("hash").Value, xUrls.Select(xUrl => xUrl.Value).ToArray());
                Ufiles.Add(uFile);
            }
            return Ufiles;
        }
    }
}
