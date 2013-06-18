using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Updating files");

            var sources = new List<Source>();
            using(var sr = new StreamReader("sources.list"))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    sources.Add(new Source(line));
                }
            }
            Console.WriteLine("Found {0} source(s)", sources.Count);

            foreach (var source in sources)
            {
                source.execute().Wait();
            }
            Console.WriteLine("Update complete");
            Console.Read();
        }
    }
}
