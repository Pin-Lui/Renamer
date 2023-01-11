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

            if (string.IsNullOrWhiteSpace(showTitel)) throw new ArgumentNullException(nameof(showTitel), "Show Titel");
            ShowTitel = showTitel;
        }

        public CSVParser(string showTitel, string episodeNr)
        {
            AppDir = (AppDomain.CurrentDomain.BaseDirectory);
            PathToListTXT = (AppDir + "\\list.txt");
            PathToListCSV = (AppDir + "\\list.csv");
            PathToAllShowCSV = (AppDir + "\\allshows.csv");

            if (string.IsNullOrWhiteSpace(showTitel)) throw new ArgumentNullException(nameof(showTitel), "Show Titel");
            if (string.IsNullOrWhiteSpace(episodeNr)) throw new ArgumentNullException(nameof(episodeNr), "Episode Number");

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
            return new CSVParser(showTitel).SearchSeasonSize();
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
            // Return if the file does not exist
            if (!File.Exists(PathToListTXT))
            {
                return;
            }

            // Clear the contents of the file by writing an empty string to it            
            try
            {
                File.WriteAllText(PathToListTXT, string.Empty);
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
            // Retrieve the list of episodes for the specified show
            List<CSVEpisodes> EpisodeBuffer = AllEpisodesBuffer(ShowTitel);

            // Return 0 if the list is null or empty
            if (EpisodeBuffer == null || EpisodeBuffer.Count == 0)
            {
                return 0;
            }

            // Find the maximum season number using LINQ's Max() method
            int maxSeason = EpisodeBuffer.Max(item => Convert.ToInt32(item.Season));

            return maxSeason;
        }

        private string GetEpsiodeCsvUrl()
        {
            List<CSVAllShows> allshowsBuffer = GetAllShowBuffer();

            // Use FirstOrDefault() to find the first show with the specified title, if it exists
            CSVAllShows searchShow = allshowsBuffer.FirstOrDefault(show => show.Titel == ShowTitel);

            // Return null if the show was not found
            if (searchShow == null)
            {
                Cout("Cound not find " + ShowTitel);
                return null;
            }

            // Construct the URL using the TVmaze number of the show
            string MazeNr = searchShow.TVmaze;
            string PathToEpisodeMaze = ("https://epguides.com/common/exportToCSVmaze.asp?maze=" + MazeNr);
            return PathToEpisodeMaze;

        }

        private List<CSVAllShows> GetAllShowBuffer()
        {
            // Return null if the file does not exist
            if (!File.Exists(PathToAllShowCSV))
            {
                return null;
            }

            // Initialize the list of shows
            List<CSVAllShows> allshowsBuffer = new();

            // Use CsvHelper to read the CSV file and parse the records
            using (var reader = new StreamReader(PathToAllShowCSV))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Read the header record and store the indices of the relevant columns
                csv.Read();
                csv.ReadHeader();
                int indexTitle = csv.GetFieldIndex("title");
                int indexDirectory = csv.GetFieldIndex("directory");
                int indexTvrage = csv.GetFieldIndex("tvrage");
                int indexID = csv.GetFieldIndex("TVmaze");
                int indexStartDate = csv.GetFieldIndex("start date");
                int indexEndDate = csv.GetFieldIndex("end date");
                int indexNoE = csv.GetFieldIndex("number of episodes");
                int indexRunTime = csv.GetFieldIndex("run time");
                int indexNetwork = csv.GetFieldIndex("network");
                int indexCountry = csv.GetFieldIndex("country");
                int indexOnhiatus = csv.GetFieldIndex("onhiatus");
                int indexOnhiatusdesc = csv.GetFieldIndex("onhiatusdesc");

                // Read the rest of the records
                while (csv.Read())
                {
                    // Create a new show object and populate its fields from the CSV record
                    CSVAllShows show = new()
                    {
                        Titel = csv.GetField(indexTitle),
                        Directory = csv.GetField(indexDirectory),
                        Tvrage = csv.GetField(indexTvrage),
                        TVmaze = csv.GetField(indexID),
                        StartDate = csv.GetField(indexStartDate),
                        EndDate = csv.GetField(indexEndDate),
                        NumberOfEpisodes = csv.GetField(indexNoE),
                        RunTime = csv.GetField(indexRunTime),
                        Network = csv.GetField(indexNetwork),
                        Country = csv.GetField(indexCountry),
                        Onhiatus = csv.GetField(indexOnhiatus),
                        Onhiatusdesc = csv.GetField(indexOnhiatusdesc)
                    };

                    // Add the show to the list
                    allshowsBuffer.Add(show);
                }
            }

            return allshowsBuffer;
        }

        private List<CSVEpisodes> GetAllEpisodeBuffer()
        {
            // Check if the CSV file exists
            if (!File.Exists(PathToListCSV))
            {
                return null;
            }

            // Delete the CSV file of HTML characters
            if (!CleanCsvFromHtml())
            {
                return null;
            }

            // Create a list to store the episodes
            var allEpisodeBuffer = new List<CSVEpisodes>();

            // Open the CSV file using CSVHelper
            using (var reader = new StreamReader(PathToListCSV))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Read the header row
                csv.Read();
                csv.ReadHeader();

                // Read each row of the CSV file
                while (csv.Read())
                {
                    // Create a new CSVEpisodes object to store the data for the row
                    CSVEpisodes episode = new()
                    {
                        EPNumber = csv.GetField("number"),
                        Season = csv.GetField("season"),
                        Episode = csv.GetField("episode"),
                        Airdate = csv.GetField("airdate"),
                        Title = csv.GetField("title"),
                        TvmazeLink = csv.GetField("tvmaze link"),
                    };

                    // Add the episode to the list
                    allEpisodeBuffer.Add(episode);
                }
            }

            // Return the list of episodes
            return allEpisodeBuffer;
        }

        private bool ConvertAllSeasonToTXT()
        {
            // Clean the list of TXT files
            CleanListTxt();

            // Get the list of episodes from the CSV file
            List<CSVEpisodes> episodeListBuffer = GetAllEpisodeBuffer();

            // Check if the episode list is null or empty
            if (episodeListBuffer == null || episodeListBuffer.Count == 0)
            {
                Cout("Could not find the buffer");
                return false;
            }

            // Create a list to store the filtered episode names
            var episodeNames = new List<string>();

            // Iterate through each episode in the list
            foreach (var item in episodeListBuffer)
            {
                // Remove unwanted characters from the episode title
                string pattern = @"^\\.+";
                string pattern2 = @"[\\\\/:*?\<>|]";
                string unfiltered = item.Title;
                string filtered1 = Regex.Replace(unfiltered, pattern, string.Empty);
                string filtered2 = Regex.Replace(filtered1, pattern2, string.Empty);

                // Add the filtered episode name to the list
                episodeNames.Add(filtered2);
            }

            // Convert the list of episode names to an array
            string[] result = episodeNames.ToArray();

            // Write the episode names to a TXT file
            WriteAllLines(PathToListTXT, result);

            // Delete the CSV file
            CleanCSV();

            // Check if the TXT file was created successfully
            if (!CheckForFile(PathToListTXT))
            {
                return false;
            }

            return true;
        }

        private bool ConvertSelectedSeasonToTXT()
        {
            // Clean the list of TXT files
            CleanListTxt();

            // Try to parse the season number from the SeasonNr string
            if (!int.TryParse(SeasonNr, out int sNr))
            {
                // If parsing fails, print an error message and return false
                Cout("Could not Parse the Season Nr. Try Again!");
                return false;
            }

            // Convert the season number to a string
            string ConfSeasonNr = sNr.ToString();

            // Get the list of episodes from the CSV file
            List<CSVEpisodes> episodeListBuffer = GetAllEpisodeBuffer();

            // Find all episodes in the specified season
            List<CSVEpisodes> selectedEpisodes = episodeListBuffer.FindAll(x => x.Season == ConfSeasonNr);

            // Check if the selected episodes list is null or empty
            if (selectedEpisodes == null || selectedEpisodes.Count == 0)
            {
                // If the list is empty, print an error message and return false
                Cout("Could not find the Season Nr. Try Again!");
                return false;
            }

            // Create a list to store the filtered episode names
            List<string> episodeNames = new();

            // Iterate through each selected episode
            foreach (var item in selectedEpisodes)
            {
                // Remove leading dots and unwanted characters from the episode title
                string pattern = @"^\\.+";
                string pattern2 = @"[\\\\/:*?\<>|]";
                string unfiltered = item.Title;
                string filtered1 = Regex.Replace(unfiltered, pattern, string.Empty);
                string filtered2 = Regex.Replace(filtered1, pattern2, string.Empty);

                // Add the filtered episode name to the list
                episodeNames.Add(filtered2);
            }

            // Convert the list of episode names to an array
            string[] result = episodeNames.ToArray();

            // Write the episode names to a TXT file
            WriteAllLines(PathToListTXT, result);

            // Delete the CSV file
            CleanCSV();

            // Check if the TXT file was created successfully
            if (!CheckForFile(PathToListTXT))
            {
                return false;
            }

            return true;
        }

        private bool CleanCsvFromHtml()
        {
            // Check if the CSV file exists
            if (!File.Exists(PathToListCSV))
            {
                // If it doesn't, return false
                return false;
            }

            // Initialize a pattern to match HTML tags
            string pattern = @"<(.|\n)*?>";

            // Initialize a pattern to match the string "List Output"
            string pattern2 = @"(List Output)";

            // Initialize a list to store the modified lines
            List<string> linebuffer = new();

            // Read all the lines from the CSV file and filter out empty lines
            var _lines = File.ReadAllLines(PathToListCSV).Where(arg => !string.IsNullOrWhiteSpace(arg));

            // Loop through the lines
            foreach (var line in _lines)
            {
                // Remove the HTML tags from the line
                string result = Regex.Replace(line, pattern, string.Empty);

                // Remove the string "List Output" from the line
                string finalresultstring = Regex.Replace(result, pattern2, string.Empty);

                // Add the modified line to the list
                linebuffer.Add(finalresultstring);
            }

            // Convert the list to an array
            string[] finalresult = linebuffer.ToArray();

            // Write the array to the CSV file
            File.WriteAllLines(PathToListCSV, finalresult);

            // Read all the lines from the CSV file and filter out empty lines
            var _lines2 = File.ReadAllLines(PathToListCSV).Where(arg => !string.IsNullOrWhiteSpace(arg));

            // Write the filtered lines to the CSV file
            File.WriteAllLines(PathToListCSV, _lines2);

            // Return true
            return true;
        }

        private static void WriteAllLines(string path, params string[] lines)
        {
            // Check if the path is null
            if (path == null)
            {
                // If it is, throw an ArgumentNullException
                throw new ArgumentNullException(nameof(path));
            }

            // Check if the lines array is null
            if (lines == null)
            {
                // If it is, throw an ArgumentNullException
                throw new ArgumentNullException(nameof(lines));
            }

            // Open a stream for writing to the file
            using var stream = File.OpenWrite(path);
            // Set the length of the stream to 0
            stream.SetLength(0);

            // Create a StreamWriter to write to the stream
            using var writer = new StreamWriter(stream);
            // Write all the lines to the stream
            writer.Write(string.Join(Environment.NewLine, lines));
        }

        #endregion Private()

    }
}
