namespace Renamer
{
    public partial class RenamerMainForm : Form
    {
        #region Felder

        private const string TutMsg = "Input Show Titel Here";
        private const string ListGenErrorMsg = "Generating List.txt failed";
        private const string RenameSTitelErrorMsg = "Input Show Titel and Select a Season";
        private const string NoListFoundErrorMsg = "No List.txt Found, Generate it first.";
        private const string DialogBoxTitel = "Rename Files to This?";
        private const string RenameSuccessMsg = "Files Renamed!";
        private const string ListGeneratedSuccessMsg = "EP List.txt generated!";
        private const string PathAllShowtxt = "https://epguides.com/common/allshows.txt";
        private readonly string AppDir;
        private readonly string PathToListTXT;
        private readonly string PathToListCSV;
        private readonly string PathToAllShowCSV;

        public static RenamerMainForm gui;

        #endregion Felder

        #region Konstruktor

        public RenamerMainForm()
        {
            InitializeComponent();
            TxB_SeriesSearch.ForeColor = Color.Gray;
            TxB_SeriesSearch.Text = TutMsg;
            gui = this;
            AppDir = (AppDomain.CurrentDomain.BaseDirectory);
            PathToListTXT = (AppDir + "\\list.txt");
            PathToListCSV = (AppDir + "\\list.csv");
            PathToAllShowCSV = (AppDir + "\\allshows.csv");
        }

        #endregion Konstruktor

        #region Public()

        public static void Cout(string text)
        {
            if (gui.TxB_Cout.InvokeRequired)
            {
                gui.TxB_Cout.Invoke(new Action(() => gui.TxB_Cout.Text = text));
            }
            else
            {
                gui.TxB_Cout.Text = text;
            }
        }

        #endregion Public()

        #region Buttons

        private async void Btn_Search_Click(object sender, EventArgs e)
        {
            GuiActivated(false);

            if (!string.IsNullOrWhiteSpace(CmB_SelectShow.Text))
            {
                CmB_SelectShow.Items.Clear();
                CmB_SelectShow.Text = "";
            }

            if (string.IsNullOrWhiteSpace(TxB_SeriesSearch.Text) || TxB_SeriesSearch.Text.Equals(TutMsg))
            {
                Cout("Input a Show Titel");
                GuiActivated(true);
                return;
            }

            CmB_SelectSeason.Items.Clear();

            try
            {
                await DownloadWebFile(PathAllShowtxt, PathToAllShowCSV);

                if (!CSVParser.CheckforFiles(PathToAllShowCSV))
                {
                    GuiActivated(true);
                    return;
                }

                List<CSVAllShows> buffer = CSVParser.GetEPGuideDB();

                /* Case Sensetive */
                //List<CSVAllShows> results = buffer.Where(x => x.Titel.Contains(TxB_SeriesSearch.Text)).ToList();

                /* Case Insensetive */
                List<CSVAllShows> results = buffer.Where(x => x.Titel.ToLower().Contains(TxB_SeriesSearch.Text.ToLower())).ToList();

                foreach (var item in results)
                {
                    CmB_SelectShow.Items.Add(item.Titel);
                }

                int resultCount = results.Count;
                if (resultCount == 1)
                {
                    Cout("Found " + resultCount + " match!");
                }
                else
                {
                    Cout("Found " + resultCount + " matches!");
                }

                if (CmB_SelectShow.Items.Count == 0)
                {
                    GuiActivated(true);
                    return;
                }

                CmB_SelectShow.SelectedIndex = 0;
                CSVParser.CleanCSVs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            GuiActivated(true);
        }

        private async void Btn_SelectShow_Click(object sender, EventArgs e)
        {
            GuiActivated(false);

            if (!string.IsNullOrWhiteSpace(CmB_SelectSeason.Text))
            {
                CmB_SelectSeason.Items.Clear();
                CmB_SelectSeason.Text = "";
            }

            if (string.IsNullOrWhiteSpace(CmB_SelectShow.Text))
            {
                Cout(RenameSTitelErrorMsg);
                GuiActivated(true);
                return;
            }

            try
            {   
                await DownloadWebFile(PathAllShowtxt, PathToAllShowCSV);
                if (!CSVParser.CheckforFiles(PathToAllShowCSV))
                {
                    GuiActivated(true);
                    return;
                }

                string UrlToEpisodeCSV = CSVParser.GetEpsiodeCsvUrls(CmB_SelectShow.Text);

                await DownloadWebFile(UrlToEpisodeCSV, PathToListCSV);
                if (!CSVParser.CheckforFiles(PathToListCSV))
                {
                    GuiActivated(true);
                    return;
                }

                int seasonSize = CSVParser.GetSeasonSize(CmB_SelectShow.Text);
                if (seasonSize == 0)
                {
                    GuiActivated(true);
                    return;
                }

                for (int i = 0; i < seasonSize; i++)
                {
                    if (i <= 8)
                    {
                        CmB_SelectSeason.Items.Add("Season 0" + (i + 1));
                    }
                    else
                    {
                        CmB_SelectSeason.Items.Add("Season " + (i + 1));
                    }
                }

                if (seasonSize == 1)
                {
                    Cout("Found " + seasonSize + " Season!");
                }
                else
                {
                    Cout("Found " + seasonSize + " Seasons!");
                }

                if (CmB_SelectSeason.Items.Count == 0)
                {
                    GuiActivated(true);
                    return;
                }

                CmB_SelectSeason.SelectedIndex = 0;
                CSVParser.CleanCSVs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            GuiActivated(true);
        }

        private async void Btn_GenSeasonEPNameList_Click(object sender, EventArgs e)
        {
            GuiActivated(false);

            if (string.IsNullOrWhiteSpace(CmB_SelectShow.Text) || string.IsNullOrWhiteSpace(CmB_SelectSeason.Text))
            {
                GuiActivated(true);
                return;
            }

            try
            {
                await DownloadWebFile(PathAllShowtxt, PathToAllShowCSV);
                if (!CSVParser.CheckforFiles(PathToAllShowCSV))
                {
                    GuiActivated(true);
                    return;
                }

                string UrlToEpisodeCSV = CSVParser.GetEpsiodeCsvUrls(CmB_SelectShow.Text);

                await DownloadWebFile(UrlToEpisodeCSV, PathToListCSV);
                if (!CSVParser.CheckforFiles(PathToListCSV))
                {
                    GuiActivated(true);
                    return;
                }

                int sNr = (CmB_SelectSeason.SelectedIndex + 1);

                if (sNr <= 9)
                {
                    string seasonNr = "0" + Convert.ToString(sNr);

                    if (!CSVParser.GetSeasonTXTFile(CmB_SelectShow.Text, seasonNr))
                    {
                        Cout(ListGenErrorMsg);
                        GuiActivated(true);
                        return;
                    }

                    Cout(ListGeneratedSuccessMsg);
                }
                else
                {
                    string seasonNr = Convert.ToString(sNr);

                    if (!CSVParser.GetSeasonTXTFile(CmB_SelectShow.Text, seasonNr))
                    {
                        GuiActivated(true);
                        Cout(ListGenErrorMsg);
                        return;
                    }

                    Cout(ListGeneratedSuccessMsg);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            GuiActivated(true);
        }

        private void Btn_RenameFilesWithList_Click(object sender, EventArgs e)
        {
            GuiActivated(false);

            if (string.IsNullOrWhiteSpace(TxB_SeriesSearch.Text) || string.IsNullOrWhiteSpace(CmB_SelectSeason.Text))
            {
                GuiActivated(true);
                Cout(RenameSTitelErrorMsg);
                return;
            }

            if (!CSVParser.CheckforFiles(PathToListTXT))
            {
                GuiActivated(true);
                Cout(NoListFoundErrorMsg);
                return;
            }

            int sNr = CmB_SelectSeason.SelectedIndex + 1;
            string sNumber;

            if (sNr <= 9)
            {
                sNumber = "0" + Convert.ToString(sNr);
            }
            else
            {
                sNumber = Convert.ToString(sNr);
            }

            List<string> fileName = FileNameHandler.FileNamePreview(TxB_SeriesSearch.Text, sNumber);
            var message = string.Join(Environment.NewLine, fileName);

            DialogResult dialogResult = MessageBox.Show(message, DialogBoxTitel, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (sNr <= 9)
                {
                    string seasonNr = "0" + Convert.ToString(sNr);
                    FileNameHandler.RenameFilesWithList(TxB_SeriesSearch.Text, seasonNr);
                    Cout(RenameSuccessMsg);
                }
                else
                {
                    string seasonNr = Convert.ToString(sNr);
                    FileNameHandler.RenameFilesWithList(TxB_SeriesSearch.Text, seasonNr);
                    Cout(RenameSuccessMsg);
                }

                GuiActivated(true);
            }
            else if (dialogResult == DialogResult.No)
            {
                GuiActivated(true);
                return;
            }

            GuiActivated(true);
        }

        #endregion Buttons

        #region Events

        private void TxB_SeriesSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxB_SeriesSearch.Text) && TxB_SeriesSearch.Text.Equals(TutMsg))
            {
                TxB_SeriesSearch.Text = "";
                TxB_SeriesSearch.ForeColor = Color.Black;
            }
        }

        #endregion Events

        #region Private()

        private static void SetProgressBarValue(int progressPercentage, long totalFileSize, long totalBytesDownloaded)
        {
            if (gui.PgB_Main.InvokeRequired)
            {
                Cout(Convert.ToString(progressPercentage) + "%");
                gui.PgB_Main.Invoke(new Action(() => gui.PgB_Main.Value = progressPercentage));
                if (gui.PgB_Main.Value == 100)
                {
                    gui.PgB_Main.Invoke(new Action(() => gui.PgB_Main.Value = 0));
                }
            }
            else
            {
                Cout(Convert.ToString(progressPercentage) + "%");
                gui.PgB_Main.Value = progressPercentage;
                if (gui.PgB_Main.Value == 100)
                {
                    gui.PgB_Main.Value = 0;
                }
            }
        }

        private static async Task DownloadWebFile(string url, string fullPath)
        {
             using var client = new DownloadManager(url, fullPath);
                client.ProgressChanged += (totalFileSize, totalBytesDownloaded, progressPercentage) =>
                {
                    SetProgressBarValue((int)progressPercentage, (long)totalFileSize, totalBytesDownloaded);
                };

            await client.StartDownload();            
        }

        private static void GuiActivated(bool state)
        {
            if (gui.Btn_Search.InvokeRequired)
            {
                gui.Btn_Search.Invoke(new Action(() => gui.Btn_Search.Enabled = state));
            }
            else
            {
                gui.Btn_Search.Enabled = state;
            }

            if (gui.Btn_SelectShow.InvokeRequired)
            {
                gui.Btn_SelectShow.Invoke(new Action(() => gui.Btn_SelectShow.Enabled = state));
            }
            else
            {
                gui.Btn_SelectShow.Enabled = state;
            }

            if (gui.Btn_GenSeasonEPNameList.InvokeRequired)
            {
                gui.Btn_GenSeasonEPNameList.Invoke(new Action(() => gui.Btn_GenSeasonEPNameList.Enabled = state));
            }
            else
            {
                gui.Btn_GenSeasonEPNameList.Enabled = state;
            }

            if (gui.Btn_RenameFilesWithList.InvokeRequired)
            {
                gui.Btn_RenameFilesWithList.Invoke(new Action(() => gui.Btn_RenameFilesWithList.Enabled = state));
            }
            else
            {
                gui.Btn_RenameFilesWithList.Enabled = state;
            }
        }

        #endregion Private()
    }
}