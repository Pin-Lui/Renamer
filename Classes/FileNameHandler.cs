namespace Renamer
{
    internal class FileNameHandler
    {
        #region Felder

        private readonly string ShowTitel;
        private readonly string AppDir;
        private readonly string SeasonNr;
        private readonly string PathToListTXT;
        private const string FileExtension = ".mkv";

        #endregion Felder

        #region Kunstruktor
        public FileNameHandler(string showTitel, string seasonNr)
        {
            AppDir = (AppDomain.CurrentDomain.BaseDirectory);
            PathToListTXT = (AppDir + "\\list.txt");

            if (string.IsNullOrEmpty(showTitel))    throw new ArgumentNullException(nameof(showTitel), "Show Titel");
            if (string.IsNullOrEmpty(seasonNr))     throw new ArgumentNullException(nameof(seasonNr), "Season Number");

            ShowTitel = showTitel;
            SeasonNr = seasonNr;
        }

        #endregion Kunstruktor

        #region Public()
        public static List<string> FileNamePreview(string showTitel, string seasonNr)
        {
            return new FileNameHandler(showTitel, seasonNr).GetFileNamePreview();
        }

        public static void RenameFilesWithList(string showTitel, string seasonNr)
        {
            new FileNameHandler(showTitel, seasonNr).FileRenameWithList();
        }

        #endregion Public()

        #region Private()

        private static string[] FilterFileNames(FileInfo[] Info)
        {
            FileInfo[] infos = Info;
            int arraySize = infos.GetLength(0);
            int counter = 0;

            string[] result = new string[arraySize];

            foreach (FileInfo f in infos)
            {
                if (f.Extension == FileExtension)
                {
                    result[counter] = f.Name;
                    counter++;
                }
            }

            string[] finalResult = result.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return finalResult;
        }

        private List<string> GetFileNamePreview()
        {
            List<string> result = new();
            string searchDirectory = AppDir;
            string textFileLine = "";
            int totalLines = (TotalLines(PathToListTXT) - 1);
            int counter = 0;
            int counter2 = 0;
            int episodeCounter = 0;

            DirectoryInfo d = new(searchDirectory);
            FileInfo[] fileInfos = d.GetFiles();

            string[] filteredStrings = FilterFileNames(fileInfos);
            string[] episodeNames = new string[(totalLines + 1)];

            using (FileStream fStream = File.OpenRead(PathToListTXT))
            {
                using TextReader reader = new StreamReader(fStream);
                while (!string.IsNullOrEmpty(textFileLine = reader.ReadLine()))
                {
                    episodeNames[counter2] = textFileLine;
                    counter2++;
                }
            }

            foreach (string f in filteredStrings)
            {
                if (counter <= (totalLines))
                {
                    if (counter <= 8)
                    {
                        try
                        {
                            // Move the files and rename them 1-9
                            result.Add(ShowTitel + " S" + SeasonNr + " E0" + (counter + 1) + " - " + episodeNames[episodeCounter] + FileExtension);
                            counter++;
                            episodeCounter++;
                        }
                        catch (FormatException e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            // Move the files and rename them 10-x
                            result.Add(ShowTitel + " S" + SeasonNr + " E" + (counter + 1) + " - " + episodeNames[episodeCounter] + FileExtension);
                            counter++;
                            episodeCounter++;
                        }
                        catch (FormatException e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                }
            }

            return result;
        }

        private void FileRenameWithList()
        {
            string searchDirectory = AppDir;
            string textFileLine = "";
            int totalLines = (TotalLines(PathToListTXT) - 1);
            int counter = 0;
            int counter2 = 0;
            int episodeCounter = 0;

            DirectoryInfo d = new(searchDirectory);
            FileInfo[] infos = d.GetFiles();

            string[] filteredStrings = FilterFileNames(infos);
            string[] episodeNames = new string[(totalLines + 1)];

            using (FileStream fStream = File.OpenRead(PathToListTXT))
            {
                using TextReader reader = new StreamReader(fStream);
                    while (!string.IsNullOrEmpty(textFileLine = reader.ReadLine()))
                    {
                        episodeNames[counter2] = textFileLine;
                        counter2++;
                    }
            }

            foreach (string f in filteredStrings)
            {
                if (counter <= (totalLines))
                {
                    if (counter <= 8)
                    {
                        try
                        {
                            // Move the files and rename them 1-9
                            File.Move(searchDirectory + f, searchDirectory + f.Replace(f, ShowTitel + " S" + SeasonNr + " E0" + (counter + 1) + " - " + episodeNames[episodeCounter] + FileExtension));
                            counter++;
                            episodeCounter++;
                        }
                        catch (FormatException e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            // Move the files and rename them 10-x
                            File.Move(searchDirectory + f, searchDirectory + f.Replace(f, ShowTitel + " S" + SeasonNr + " E" + (counter + 1) + " - " + episodeNames[episodeCounter] + FileExtension));
                            counter++;
                            episodeCounter++;
                        }
                        catch (FormatException e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                }
            }
        }

        private static int TotalLines(string filePath)
        {
            using StreamReader r = new(filePath);
                int i = 0;
                while (r.ReadLine() != null) { i++; }
                return i;
        }

        #endregion Private()
    }
}
