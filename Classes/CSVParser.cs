using CsvHelper;
using System.Globalization;
using System.Text.RegularExpressions;


namespace Renamer
{
    internal class CSVParser
    {
        #region Felder

        private readonly string ShowTitel;
        private readonly string AppDir;
        private readonly string SeasonNr;
        private readonly string PathToListTXT;
        private readonly string PathToListCSV;
        private readonly string PathToAllShowCSV;

        #endregion Felder

        #region Konstruktor
        public CSVParser()
        {
            AppDir = (AppDomain.CurrentDomain.BaseDirectory);
            PathToListTXT = (AppDir + "\\list.txt");
            PathToListCSV = (AppDir + "\\list.csv");
            PathToAllShowCSV = (AppDir + "\\allshows.csv");
        }

        public CSVParser(string showTitel)
        {
            AppDir = (AppDomain.CurrentDomain.BaseDirectory);
            PathToListTXT = (AppDir + "\\list.txt");
            PathToListCSV = (AppDir + "\\list.csv");
            PathToAllShowCSV = (AppDir + "\\allshows.csv");

            if (string.IsNullOrWhiteSpace(showTitel))   throw new ArgumentNullException(nameof(showTitel), "Show Titel");            
            ShowTitel = showTitel;
        }

        public CSVParser(string showTitel, string episodeNr)
        {
            AppDir = (AppDomain.CurrentDomain.BaseDirectory);
            PathToListTXT = (AppDir + "\\list.txt");
            PathToListCSV = (AppDir + "\\list.csv");
            PathToAllShowCSV = (AppDir + "\\allshows.csv");

            if (string.IsNullOrWhiteSpace(showTitel))   throw new ArgumentNullException(nameof(showTitel), "Show Titel");
            if (string.IsNullOrWhiteSpace(episodeNr))   throw new ArgumentNullException(nameof(episodeNr), "Episode Number");

            ShowTitel = showTitel;
            SeasonNr = episodeNr;
        }

        #endregion Konstruktor

        #region Public()

        public static void Cout(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            RenamerMainForm.Cout(text);
        }

        public static bool CheckforFiles(string fileUrl)
        {
            return new CSVParser().CheckForFile(fileUrl);
        }

        public static int GetSeasonSize(string showTitel)
        { 
            return  new CSVParser(showTitel).SearchSeasonSize();    
        }

        public static bool GetTXTFile(string showTitel)
        {
            return new CSVParser(showTitel).ConvertAllSeasonToTXT();
        }

        public static bool GetSeasonTXTFile(string showTitel, string seasonNr)
        {
            return new CSVParser(showTitel, seasonNr).ConvertSelectedSeasonToTXT();
        }

        public static List<CSVEpisodes> AllEpisodesBuffer(string showTitel)
        {
            return new CSVParser(showTitel).GetAllEpisodeBuffer();
        }

        public static List<CSVAllShows> GetEPGuideDB()
        {
            return new CSVParser().GetAllShowBuffer();
        }

        public static void CleanCSVs()
        {
           new CSVParser().CleanCSV();
        }

        public static string GetEpsiodeCsvUrls(string showTitel)
        {
            return new CSVParser(showTitel).GetEpsiodeCsvUrl();
        }

        #endregion Public()

        #region Private()

        private void CleanCSV()
        {
            if (!File.Exists(PathToAllShowCSV))
            {
                return; 
            }
            try
            {
                File.Delete(PathToAllShowCSV);
            }
            catch (IOException ioExp)
            {
                MessageBox.Show(ioExp.Message);
                return;
            }

            if (!File.Exists(PathToListCSV))
            {
                return;
            }
            try
            {
                File.Delete(PathToListCSV);
            }
            catch (IOException ioExp)
            {
                MessageBox.Show(ioExp.Message);
                return;
            }
        }

        private void CleanListTxt()
        {
            if (!File.Exists(PathToListTXT))
            {
                return;
            }
            try
            {
                File.Delete(PathToListTXT);
            }
            catch (IOException ioExp)
            {
                MessageBox.Show(ioExp.Message);
                return;
            }
        }

        private bool CheckForFile(string fileUrl)
        {
            if (File.Exists(fileUrl))
            {
                return true;
            }

            return false;
        }

        private int SearchSeasonSize()
        {
            List<CSVEpisodes> EpisodeBuffer = AllEpisodesBuffer(ShowTitel);
            if (EpisodeBuffer == null)
            {
                return 0;
            }

            List<int> ResultsInt = new List<int>();
            foreach (var item in EpisodeBuffer)
            {
                ResultsInt.Add(Convert.ToInt32(item.Season));
            }
            if (ResultsInt == null)
            {
                return 0;
            }
            int Result = ResultsInt.Max();
            return Result;            
        }

        private string GetEpsiodeCsvUrl()
        {
            List<CSVAllShows> allshowsBuffer = GetAllShowBuffer();
            CSVAllShows _SearchTitel = allshowsBuffer.Find(x => x.Titel == ShowTitel);
            if (_SearchTitel == null)
            {
                Cout("Cound not find " + ShowTitel);
                return null;
            }

            string MazeNr = _SearchTitel.TVmaze;
            string PathToEpisodeMaze = ("https://epguides.com/common/exportToCSVmaze.asp?maze=" + MazeNr);
            return PathToEpisodeMaze;
        }

        private List<CSVAllShows> GetAllShowBuffer()
        {
            if (!File.Exists(PathToAllShowCSV))
            {
                return null;
            }

            List<CSVAllShows> allshowsBuffer = new();
            var lines = File.ReadAllLines(PathToAllShowCSV).Where(arg => !string.IsNullOrWhiteSpace(arg));
            File.WriteAllLines(PathToAllShowCSV, lines);

            using (StreamReader _StreamReader = File.OpenText(PathToAllShowCSV))
            {
                var csv = new CsvReader(_StreamReader, CultureInfo.InvariantCulture);
                csv.Read();
                csv.ReadHeader();
                string[] header = csv.Context.Reader.HeaderRecord;
                var columnExtractedTitle = "title";
                var columnExtractedDirectory = "directory";
                var columnExtractedTvrage = "tvrage";
                var columnExtractedID = "TVmaze";
                var columnExtractedStartDate = "start date";
                var columnExtractedEndDate = "end date";
                var columnExtractedNoE = "number of episodes";
                var columnExtractedRunTime = "run time";
                var columnExtractedNetwork = "network";
                var columnExtractedCountry = "country";
                var columnExtractedOnhiatus = "onhiatus";
                var columnExtractedOnhiatusdesc = "onhiatusdesc";
                int extractedIndexTitle = Array.IndexOf(header, columnExtractedTitle);
                int extractedIndexDirectory = Array.IndexOf(header, columnExtractedDirectory);
                int extractedIndexTvrage = Array.IndexOf(header, columnExtractedTvrage);
                int extractedIndexID = Array.IndexOf(header, columnExtractedID);
                int extractedIndexStartDate = Array.IndexOf(header, columnExtractedStartDate);
                int extractedIndexEndDate = Array.IndexOf(header, columnExtractedEndDate);
                int extractedIndexNoE = Array.IndexOf(header, columnExtractedNoE);
                int extractedIndexRunTime = Array.IndexOf(header, columnExtractedRunTime);
                int extractedIndexNetwork = Array.IndexOf(header, columnExtractedNetwork);
                int extractedIndexCountry = Array.IndexOf(header, columnExtractedCountry);
                int extractedIndexOnhiatus = Array.IndexOf(header, columnExtractedOnhiatus);
                int extractedIndexOnhiatusdesc = Array.IndexOf(header, columnExtractedOnhiatusdesc);

                while (csv.Read())
                {
                    string[] row = csv.Context.Reader.Parser.Record;

                    string Title = row[extractedIndexTitle];
                    string Directory = row[extractedIndexDirectory];
                    string Tvrage = row[extractedIndexTvrage];
                    string ID = row[extractedIndexID];
                    string StartDate = row[extractedIndexStartDate];
                    string EndDate = row[extractedIndexEndDate];
                    string NoE = row[extractedIndexNoE];
                    string RunTime = row[extractedIndexRunTime];
                    string Network = row[extractedIndexNetwork];
                    string Country = row[extractedIndexCountry];
                    string Onhiatus = row[extractedIndexOnhiatus];
                    string Onhiatusdesc = row[extractedIndexOnhiatusdesc];

                    allshowsBuffer.Add(new CSVAllShows
                    {
                        Titel = Title,
                        Directory = Directory,
                        Tvrage = Tvrage,
                        TVmaze = ID,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        NumberOfEpisodes = NoE,
                        RunTime = RunTime,
                        Network = Network,
                        Country = Country,
                        Onhiatus = Onhiatus,
                        Onhiatusdesc = Onhiatusdesc,
                    });

                }
            }
                
            return allshowsBuffer;
            
        }

        private List<CSVEpisodes> GetAllEpisodeBuffer()
        {
            if (!File.Exists(PathToListCSV))
            {
                return null;
            }
            
            if (!CleanCsvFromHtml())
            {
                return null;
            }           

            List<CSVEpisodes> allEpisodeBuffer = new();
            using (StreamReader _StreamReader = File.OpenText(PathToListCSV))
            {
                var csv = new CsvReader(_StreamReader, CultureInfo.InvariantCulture);
                csv.Read();
                csv.ReadHeader();
                string[] header = csv.Context.Reader.HeaderRecord;

                var columnExtractedNumber = "number";
                var columnExtractedSeason = "season";
                var columnExtractedEpisode = "episode";
                var columnExtractedAirdate = "airdate";
                var columnExtractedTitle = "title";
                var columnExtractedTvmazeLink = "tvmaze link";

                int extractedIndexNumber = Array.IndexOf(header, columnExtractedNumber);
                int extractedIndexSeason = Array.IndexOf(header, columnExtractedSeason);
                int extractedIndexEpisode = Array.IndexOf(header, columnExtractedEpisode);
                int extractedIndexAirdate = Array.IndexOf(header, columnExtractedAirdate);
                int extractedIndexTitle = Array.IndexOf(header, columnExtractedTitle);
                int extractedIndexTvmazeLink = Array.IndexOf(header, columnExtractedTvmazeLink);

                while (csv.Read())
                {
                    string[] row = csv.Context.Reader.Parser.Record;

                    string Season = row[extractedIndexSeason];
                    string Episode = row[extractedIndexEpisode];
                    string Airdate = row[extractedIndexAirdate];
                    string Title = row[extractedIndexTitle];
                    string TvMazeLink = row[extractedIndexTvmazeLink];

                    allEpisodeBuffer.Add(new CSVEpisodes
                    {
                        EPNumber = Title,
                        Season = Season,
                        Episode = Episode,
                        Airdate = Airdate,
                        Title = Title,
                        TvmazeLink = TvMazeLink,
                    });

                }
            }

            return allEpisodeBuffer;
        }

        private bool ConvertAllSeasonToTXT()
        {
            CleanListTxt();

            List<string> episodeNames = new();
            List<CSVEpisodes> EpisodeListBuffer = GetAllEpisodeBuffer();

            if (EpisodeListBuffer == null || EpisodeListBuffer.Count == 0)
            {
                Cout("Could not find the buffer");
                return false;
            }

            foreach (var item in EpisodeListBuffer)
            {
                string pattern = @"^\\.+";
                string pattern2 = @"[\\\\/:*?\<>|]";
                string unfiltered = item.Title;
                string filtered1 = Regex.Replace(unfiltered, pattern, string.Empty);
                string filtered2 = Regex.Replace(filtered1, pattern2, string.Empty);
                episodeNames.Add(filtered2);
            }
            string[] result = episodeNames.ToArray();
            WriteAllLines(PathToListTXT, result);

            CleanCSV();

            if (!CheckForFile(PathToListTXT))
            {
                return false;
            }

            return true;
        }

        private bool ConvertSelectedSeasonToTXT()
        {
            CleanListTxt();

            if (!int.TryParse(SeasonNr, out int sNr))
            {
                Cout("Could not Parse the Season Nr. Try Again!");
                return false;
            }

            string ConfSeasonNr = sNr.ToString();
            List<string> episodeNames = new();
            List<CSVEpisodes> EpisodeListBuffer = GetAllEpisodeBuffer();
            List<CSVEpisodes> SelectedEpisodes = EpisodeListBuffer.FindAll(x => x.Season == ConfSeasonNr);

            if (SelectedEpisodes == null || SelectedEpisodes.Count == 0)
            {
                Cout("Could not find the Season Nr. Try Again!");
                return false;
            }

            foreach (var item in SelectedEpisodes)
            {
                string pattern = @"^\\.+";
                string pattern2 = @"[\\\\/:*?\<>|]";
                string unfiltered = item.Title;
                string filtered1 = Regex.Replace(unfiltered, pattern, string.Empty);
                string filtered2 = Regex.Replace(filtered1, pattern2, string.Empty);
                episodeNames.Add(filtered2);
            }
            string[] result = episodeNames.ToArray();
            WriteAllLines(PathToListTXT, result);

            CleanCSV();

            if (!CheckForFile(PathToListTXT))
            {
                return false;
            }

            return true;
        }

        private bool CleanCsvFromHtml()
        {
            if (!File.Exists(PathToListCSV))
            {
                return false;
            }

            string pattern = @"<(.|\n)*?>";
            string pattern2 = @"(List Output)";
            List<string> linebuffer = new();

            var _lines = File.ReadAllLines(PathToListCSV).Where(arg => !string.IsNullOrWhiteSpace(arg));

            foreach (var line in _lines)
            {
                string result = Regex.Replace(line, pattern, string.Empty);
                string finalresultstring = Regex.Replace(result, pattern2, string.Empty);
                linebuffer.Add(finalresultstring);
            }

            string[] finalresult = linebuffer.ToArray();
            File.WriteAllLines(PathToListCSV, finalresult);

            var _lines2 = File.ReadAllLines(PathToListCSV).Where(arg => !string.IsNullOrWhiteSpace(arg));
            File.WriteAllLines(PathToListCSV, _lines2);
            return true;
        }

        private static void WriteAllLines(string path, params string[] lines)
        {
            if (path == null)   throw new ArgumentNullException(nameof(path));
            if (lines == null)  throw new ArgumentNullException(nameof(lines));

            using var stream = File.OpenWrite(path);
                stream.SetLength(0);
                using var writer = new StreamWriter(stream);
                    if (lines.Length > 0)
                    {
                        for (var i = 0; i < lines.Length - 1; i++)
                        {
                            writer.WriteLine(lines[i]);
                        }
                        writer.Write(lines[^1]);
                    }
        }

        #endregion Private()

    }
}
