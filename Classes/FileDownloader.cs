using System.Security.Policy;

namespace Renamer
{
    internal class FileDownloader
    {
        #region Felder

        private readonly string _url;
        private readonly string _fullPathWhereToSave;

        #endregion Felder

        #region Kunstroktor
        public FileDownloader(string url, string fullPathWhereToSave)
        {
            if (string.IsNullOrEmpty(url))                  throw new ArgumentNullException(nameof(url), "No Url Found");
            if (string.IsNullOrEmpty(fullPathWhereToSave))  throw new ArgumentNullException(nameof(fullPathWhereToSave), "No Path Found");

            _url = url;
            _fullPathWhereToSave = fullPathWhereToSave;
        }

        #endregion Kunstroktor

        #region Public()

        public static bool DownloadFile(string url, string fullPathWhereToSave)
        {
            return new FileDownloader(url, fullPathWhereToSave).StartDownload();
        }

        #endregion Public()

        #region Private()

        private bool StartDownload()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fullPathWhereToSave));

            if (File.Exists(_fullPathWhereToSave))
            {
                try
                {
                    File.Delete(_fullPathWhereToSave);
                }
                catch (FormatException e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
            }

            HttpClient client = new();
            var GetTask = client.GetAsync(_url);
            CancellationToken WebCommsTimeout = default;
            GetTask.Wait(WebCommsTimeout); // WebCommsTimeout is in milliseconds

            if (!GetTask.Result.IsSuccessStatusCode)
            {
                return false;
            }

            using (var fs = new FileStream(_fullPathWhereToSave, FileMode.CreateNew))
            {
                var ResponseTask = GetTask.Result.Content.CopyToAsync(fs);
                ResponseTask.Wait(WebCommsTimeout);
            }

            if (!File.Exists(_fullPathWhereToSave))
            {
                return false;
            }

            return true;
        }

        #endregion Private()
    }
}
