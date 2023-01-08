namespace Renamer
{
    partial class RenamerMainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenamerMainForm));
            this.Btn_Search = new System.Windows.Forms.Button();
            this.TxB_SeriesSearch = new System.Windows.Forms.TextBox();
            this.CmB_SelectShow = new System.Windows.Forms.ComboBox();
            this.CmB_SelectSeason = new System.Windows.Forms.ComboBox();
            this.Btn_SelectShow = new System.Windows.Forms.Button();
            this.Btn_GenSeasonEPNameList = new System.Windows.Forms.Button();
            this.Btn_RenameFilesWithList = new System.Windows.Forms.Button();
            this.TxB_Cout = new System.Windows.Forms.TextBox();
            this.PgB_Main = new System.Windows.Forms.ProgressBar();
            this.TxB_FileExtension = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Btn_Search
            // 
            this.Btn_Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Btn_Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Search.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Btn_Search.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Btn_Search.Location = new System.Drawing.Point(413, 10);
            this.Btn_Search.Name = "Btn_Search";
            this.Btn_Search.Size = new System.Drawing.Size(122, 31);
            this.Btn_Search.TabIndex = 0;
            this.Btn_Search.Text = "Search";
            this.Btn_Search.UseVisualStyleBackColor = false;
            this.Btn_Search.Click += new System.EventHandler(this.Btn_Search_Click);
            // 
            // TxB_SeriesSearch
            // 
            this.TxB_SeriesSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TxB_SeriesSearch.Location = new System.Drawing.Point(12, 10);
            this.TxB_SeriesSearch.Name = "TxB_SeriesSearch";
            this.TxB_SeriesSearch.Size = new System.Drawing.Size(395, 31);
            this.TxB_SeriesSearch.TabIndex = 2;
            this.TxB_SeriesSearch.Click += new System.EventHandler(this.TxB_SeriesSearch_Click);
            // 
            // CmB_SelectShow
            // 
            this.CmB_SelectShow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CmB_SelectShow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmB_SelectShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CmB_SelectShow.FormattingEnabled = true;
            this.CmB_SelectShow.Location = new System.Drawing.Point(12, 58);
            this.CmB_SelectShow.MaxDropDownItems = 100;
            this.CmB_SelectShow.Name = "CmB_SelectShow";
            this.CmB_SelectShow.Size = new System.Drawing.Size(395, 33);
            this.CmB_SelectShow.TabIndex = 4;
            this.CmB_SelectShow.TextChanged += new System.EventHandler(this.CmB_SelectShow_TextChanged);
            // 
            // CmB_SelectSeason
            // 
            this.CmB_SelectSeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CmB_SelectSeason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmB_SelectSeason.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CmB_SelectSeason.FormattingEnabled = true;
            this.CmB_SelectSeason.Location = new System.Drawing.Point(12, 107);
            this.CmB_SelectSeason.MaxDropDownItems = 100;
            this.CmB_SelectSeason.Name = "CmB_SelectSeason";
            this.CmB_SelectSeason.Size = new System.Drawing.Size(173, 33);
            this.CmB_SelectSeason.TabIndex = 5;
            // 
            // Btn_SelectShow
            // 
            this.Btn_SelectShow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Btn_SelectShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_SelectShow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Btn_SelectShow.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Btn_SelectShow.Location = new System.Drawing.Point(413, 58);
            this.Btn_SelectShow.Name = "Btn_SelectShow";
            this.Btn_SelectShow.Size = new System.Drawing.Size(122, 33);
            this.Btn_SelectShow.TabIndex = 6;
            this.Btn_SelectShow.Text = "Select";
            this.Btn_SelectShow.UseVisualStyleBackColor = false;
            this.Btn_SelectShow.Click += new System.EventHandler(this.Btn_SelectShow_Click);
            // 
            // Btn_GenSeasonEPNameList
            // 
            this.Btn_GenSeasonEPNameList.BackColor = System.Drawing.Color.Silver;
            this.Btn_GenSeasonEPNameList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_GenSeasonEPNameList.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Btn_GenSeasonEPNameList.Location = new System.Drawing.Point(191, 107);
            this.Btn_GenSeasonEPNameList.Name = "Btn_GenSeasonEPNameList";
            this.Btn_GenSeasonEPNameList.Size = new System.Drawing.Size(216, 34);
            this.Btn_GenSeasonEPNameList.TabIndex = 10;
            this.Btn_GenSeasonEPNameList.Text = "Get Season EP Names";
            this.Btn_GenSeasonEPNameList.UseVisualStyleBackColor = false;
            this.Btn_GenSeasonEPNameList.Click += new System.EventHandler(this.Btn_GenSeasonEPNameList_Click);
            // 
            // Btn_RenameFilesWithList
            // 
            this.Btn_RenameFilesWithList.BackColor = System.Drawing.Color.Silver;
            this.Btn_RenameFilesWithList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_RenameFilesWithList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Btn_RenameFilesWithList.ForeColor = System.Drawing.Color.Blue;
            this.Btn_RenameFilesWithList.Location = new System.Drawing.Point(413, 107);
            this.Btn_RenameFilesWithList.Name = "Btn_RenameFilesWithList";
            this.Btn_RenameFilesWithList.Size = new System.Drawing.Size(122, 34);
            this.Btn_RenameFilesWithList.TabIndex = 11;
            this.Btn_RenameFilesWithList.Text = "Rename Files";
            this.Btn_RenameFilesWithList.UseVisualStyleBackColor = false;
            this.Btn_RenameFilesWithList.Click += new System.EventHandler(this.Btn_RenameFilesWithList_Click);
            // 
            // TxB_Cout
            // 
            this.TxB_Cout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.TxB_Cout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxB_Cout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.TxB_Cout.ForeColor = System.Drawing.Color.Lime;
            this.TxB_Cout.Location = new System.Drawing.Point(70, 157);
            this.TxB_Cout.Name = "TxB_Cout";
            this.TxB_Cout.ReadOnly = true;
            this.TxB_Cout.Size = new System.Drawing.Size(465, 24);
            this.TxB_Cout.TabIndex = 12;
            this.TxB_Cout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxB_Cout.DoubleClick += new System.EventHandler(this.TxB_Cout_DoubleClick);
            // 
            // PgB_Main
            // 
            this.PgB_Main.ForeColor = System.Drawing.Color.Blue;
            this.PgB_Main.Location = new System.Drawing.Point(12, 196);
            this.PgB_Main.Name = "PgB_Main";
            this.PgB_Main.Size = new System.Drawing.Size(523, 16);
            this.PgB_Main.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.PgB_Main.TabIndex = 13;
            // 
            // TxB_FileExtension
            // 
            this.TxB_FileExtension.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TxB_FileExtension.Location = new System.Drawing.Point(12, 153);
            this.TxB_FileExtension.MaxLength = 3;
            this.TxB_FileExtension.Name = "TxB_FileExtension";
            this.TxB_FileExtension.Size = new System.Drawing.Size(52, 31);
            this.TxB_FileExtension.TabIndex = 14;
            this.TxB_FileExtension.Click += new System.EventHandler(this.TxB_FileExtension_Click);
            // 
            // RenamerMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(545, 224);
            this.Controls.Add(this.TxB_FileExtension);
            this.Controls.Add(this.PgB_Main);
            this.Controls.Add(this.TxB_Cout);
            this.Controls.Add(this.Btn_RenameFilesWithList);
            this.Controls.Add(this.Btn_GenSeasonEPNameList);
            this.Controls.Add(this.Btn_SelectShow);
            this.Controls.Add(this.CmB_SelectSeason);
            this.Controls.Add(this.CmB_SelectShow);
            this.Controls.Add(this.TxB_SeriesSearch);
            this.Controls.Add(this.Btn_Search);
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RenamerMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Renamer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button Btn_Search;
        private TextBox TxB_SeriesSearch;
        private ComboBox CmB_SelectShow;
        private ComboBox CmB_SelectSeason;
        private Button Btn_SelectShow;
        private Button Btn_GenSeasonEPNameList;
        private Button Btn_RenameFilesWithList;
        public TextBox TxB_Cout;
        public ProgressBar PgB_Main;
        private TextBox TxB_FileExtension;
    }
}