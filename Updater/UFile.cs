using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class UFile
    {
        private readonly string fileName;
        private readonly string hash;
        private readonly string[] urls;

        private int lastDots;

        public UFile(string fileName, string hash, string[] urls)
        {
            this.fileName = fileName;
            this.urls = urls;
            this.hash = hash;
        }

        public async Task<bool> download()
        {

            Console.WriteLine();
            Console.WriteLine("Updating {0}", fileName);
            Console.Write("Checking if sum match {0}: ", hash);

            if (File.Exists(fileName) && calcMD5(fileName) == hash)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("OK!");
                Console.ForegroundColor = ConsoleColor.Gray;
                return true;
            }
            bool error = true;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong hash, Updating");
            Console.ForegroundColor = ConsoleColor.Gray;

            foreach (var url in urls)
            {
                Console.WriteLine("Downloading from {0}", url);

                var downloader = new Downloader();
                var progress = new Progress<int>();

                progress.ProgressChanged += onProgressChanged;
                DownloadFileResult result = await downloader.downloadFile(url, fileName, progress);

                Console.WriteLine("Downloaded {0} bytes", result.bytesCount);
                if (result.error)
                {
                    Console.WriteLine("Url {0} not working", url);
                    continue;
                }
                error = false;
                break;
            }
            if (error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No working url found!");
                Console.ForegroundColor = ConsoleColor.Gray;
                return false;
            }
            return true;
        }

        private void onProgressChanged(object s, int e)
        {
            int dots = e/5;
            for (int i = 0; i < dots - lastDots; i++)
            {
                Console.Write(".");
            }
            lastDots = dots;
        }

        private string calcMD5(string filename)
        {
            using(var md5 = MD5.Create())
            {
                using(var stream = File.OpenRead(filename))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }
        }
    }
}
