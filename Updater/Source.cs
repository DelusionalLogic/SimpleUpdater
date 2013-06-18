using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class Source
    {
        private readonly string url;

        public Source(string url)
        {
            this.url = url;
        }

        public async Task<bool> execute()
        {
            Console.WriteLine("Parsing source {0}", url);
            List<UFile> files = XMLParser.parseXML(url);
            foreach (var uFile in files)
            {
                await uFile.download();
            }
            return false;
        }
    }
}
