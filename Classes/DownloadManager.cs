namespace Renamer
{
    public delegate void ProgressChangedHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage);

    public class DownloadManager : HttpClient
    {
        #region Felder

        private readonly string _DownloadUrl;
        private readonly string _DestinationFilePath;

        #endregion Felder

        #region Events

        public event ProgressChangedHandler ProgressChanged;

        #endregion Events

        #region Konstruktor

        public DownloadManager(string downloadUrl, string destinationFilePath)
        {
            _DownloadUrl = downloadUrl;
            _DestinationFilePath = destinationFilePath;
        }

        #endregion Konstruktor

        #region Public()

        public async Task StartDownload()
        {
            using var response = await GetAsync(_DownloadUrl, HttpCompletionOption.ResponseHeadersRead);
            await DownloadFileFromHttpResponseMessage(response);
        }

        public async Task StartDownload(CancellationToken cancellationToken)
        {
            using var response = await GetAsync(_DownloadUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            await DownloadFileFromHttpResponseMessage(response);
        }

        #endregion Public()

        #region Private()

        private async Task DownloadFileFromHttpResponseMessage(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            long? totalBytes = response.Content.Headers.ContentLength;
            using var contentStream = await response.Content.ReadAsStreamAsync();
            await ProcessContentStream(totalBytes, contentStream);
        }

        private async Task ProcessContentStream(long? totalDownloadSize, Stream contentStream)
        {
            long totalBytesRead = 0L;
            long readCount = 0L;
            byte[] buffer = new byte[8192];
            bool isMoreToRead = true;

            using (FileStream fileStream = new(_DestinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                do
                {
                    int bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        isMoreToRead = false;
                        continue;
                    }

                    await fileStream.WriteAsync(buffer, 0, bytesRead);

                    totalBytesRead += bytesRead;
                    readCount += 1;

                    if (readCount % 10 == 0)
                        TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                }
                while (isMoreToRead);
            }
            TriggerProgressChanged(totalDownloadSize, totalBytesRead);
        }

        private void TriggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
        {
            if (ProgressChanged == null)
                return;

            double? progressPercentage = null;
            if (totalDownloadSize.HasValue)
                progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);

            ProgressChanged(totalDownloadSize, totalBytesRead, progressPercentage);
        }

        #endregion Private()
    }
}
