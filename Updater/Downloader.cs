using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class Downloader
    {
        private readonly WebClient wc = new WebClient();
        private int complete;
        private long totalBytes;
        private IProgress<int> progress; 

        public Downloader()
        {
            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
            wc.DownloadFileCompleted += wc_DownloadFileCompleted;
        }

        public async Task<DownloadFileResult> downloadFile(string url, string path, IProgress<int> progress)
        {
            this.progress = progress;
            totalBytes = 0;
            complete = 0;

            wc.DownloadFileAsync(new Uri(url), path);

            while (complete == 0)
                await Task.Yield();
            return new DownloadFileResult{bytesCount = totalBytes, error = complete == 2};
        }

        void wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            complete = e.Error == null ? 1 : 2;
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if(progress != null)
                progress.Report((int) (e.BytesReceived/(double)e.TotalBytesToReceive*100));
            totalBytes = e.TotalBytesToReceive;
        }
    }

    public struct DownloadFileResult
    {
        public long bytesCount { get; set; }
        public bool error { get; set; }
    }
}
