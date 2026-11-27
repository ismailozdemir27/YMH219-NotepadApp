namespace NotepadApp.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Kenar Çubuğu
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlSidebarHeader;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblNoteCountBadge;
        private System.Windows.Forms.Panel pnlSidebarSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbCategoryFilter;
        private System.Windows.Forms.ListBox lstNotes;
        private System.Windows.Forms.Panel pnlSidebarFooter;
        private System.Windows.Forms.Button btnNewNote;
        private System.Windows.Forms.TableLayoutPanel tblActionButtons;
        private System.Windows.Forms.Button btnPin;
        private System.Windows.Forms.Button btnDuplicate;
        private System.Windows.Forms.Button btnDelete;

        // Ana İçerik Alanı
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tlpNoteHeader;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.CheckBox chkPin;
        private System.Windows.Forms.Button btnThemeToggle;

        // Araç Çubuğu
        private System.Windows.Forms.ToolStrip tsFormatting;
        private System.Windows.Forms.ToolStripComboBox tscmbFontFamily;
        private System.Windows.Forms.ToolStripComboBox tscmbFontSize;
        private System.Windows.Forms.ToolStripButton tsbtnBold;
        private System.Windows.Forms.ToolStripButton tsbtnItalic;
        private System.Windows.Forms.ToolStripButton tsbtnUnderline;
        private System.Windows.Forms.ToolStripButton tsbtnStrike;
        private System.Windows.Forms.ToolStripSeparator tsSep1;
        private System.Windows.Forms.ToolStripButton tsbtnTextColor;
        private System.Windows.Forms.ToolStripButton tsbtnHighlightColor;
        private System.Windows.Forms.ToolStripSeparator tsSep2;
        private System.Windows.Forms.ToolStripButton tsbtnBullet;
        private System.Windows.Forms.ToolStripButton tsbtnAlignLeft;
        private System.Windows.Forms.ToolStripButton tsbtnAlignCenter;
        private System.Windows.Forms.ToolStripButton tsbtnAlignRight;
        private System.Windows.Forms.ToolStripSeparator tsSep3;
        private System.Windows.Forms.ToolStripButton tsbtnInsertImage;
        private System.Windows.Forms.ToolStripButton tsbtnFindReplace;
        private System.Windows.Forms.ToolStripButton tsbtnTTS;
        private System.Windows.Forms.ToolStripButton tsbtnLock;
        private System.Windows.Forms.ToolStripButton tsbtnStats;
        private System.Windows.Forms.ToolStripSeparator tsSep4;
        private System.Windows.Forms.ToolStripDropDownButton tsddbExport;
        private System.Windows.Forms.ToolStripMenuItem miExportTxt;
        private System.Windows.Forms.ToolStripMenuItem miExportRtf;
        private System.Windows.Forms.ToolStripMenuItem miExportMd;
        private System.Windows.Forms.ToolStripMenuItem miExportHtml;
        private System.Windows.Forms.ToolStripButton tsbtnImport;

        // Editör ve Menü
        private System.Windows.Forms.RichTextBox rtbEditor;
        private System.Windows.Forms.ContextMenuStrip cmsEditor;
        private System.Windows.Forms.ToolStripMenuItem miCut;
        private System.Windows.Forms.ToolStripMenuItem miCopy;
        private System.Windows.Forms.ToolStripMenuItem miPaste;
        private System.Windows.Forms.ToolStripSeparator miSep1;
        private System.Windows.Forms.ToolStripMenuItem miFind;
        private System.Windows.Forms.ToolStripMenuItem miSpeak;
        private System.Windows.Forms.ToolStripSeparator miSep2;
        private System.Windows.Forms.ToolStripMenuItem miSelectAll;

        // Durum Çubuğu
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblWordCount;
        private System.Windows.Forms.ToolStripStatusLabel lblCharCount;
        private System.Windows.Forms.ToolStripStatusLabel lblReadingTime;
        private System.Windows.Forms.ToolStripStatusLabel lblSaveStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblTTSStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStudentInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Kenar Çubuğu
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlSidebarHeader = new System.Windows.Forms.Panel();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.lblNoteCountBadge = new System.Windows.Forms.Label();
            this.pnlSidebarSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbCategoryFilter = new System.Windows.Forms.ComboBox();
            this.lstNotes = new System.Windows.Forms.ListBox();
            this.pnlSidebarFooter = new System.Windows.Forms.Panel();
            this.btnNewNote = new System.Windows.Forms.Button();
            this.tblActionButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnPin = new System.Windows.Forms.Button();
            this.btnDuplicate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();

            // Ana İçerik
            this.pnlContent = new System.Windows.Forms.Panel();
            this.tlpNoteHeader = new System.Windows.Forms.TableLayoutPanel();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.chkPin = new System.Windows.Forms.CheckBox();
            this.btnThemeToggle = new System.Windows.Forms.Button();

            // Araç Çubuğu
            this.tsFormatting = new System.Windows.Forms.ToolStrip();
            this.tscmbFontFamily = new System.Windows.Forms.ToolStripComboBox();
            this.tscmbFontSize = new System.Windows.Forms.ToolStripComboBox();
            this.tsbtnBold = new System.Windows.Forms.ToolStripButton();
            this.tsbtnItalic = new System.Windows.Forms.ToolStripButton();
            this.tsbtnUnderline = new System.Windows.Forms.ToolStripButton();
            this.tsbtnStrike = new System.Windows.Forms.ToolStripButton();
            this.tsSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnTextColor = new System.Windows.Forms.ToolStripButton();
            this.tsbtnHighlightColor = new System.Windows.Forms.ToolStripButton();
            this.tsSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnBullet = new System.Windows.Forms.ToolStripButton();
            this.tsbtnAlignLeft = new System.Windows.Forms.ToolStripButton();
            this.tsbtnAlignCenter = new System.Windows.Forms.ToolStripButton();
            this.tsbtnAlignRight = new System.Windows.Forms.ToolStripButton();
            this.tsSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnInsertImage = new System.Windows.Forms.ToolStripButton();
            this.tsbtnFindReplace = new System.Windows.Forms.ToolStripButton();
            this.tsbtnTTS = new System.Windows.Forms.ToolStripButton();
            this.tsbtnLock = new System.Windows.Forms.ToolStripButton();
            this.tsbtnStats = new System.Windows.Forms.ToolStripButton();
            this.tsSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbExport = new System.Windows.Forms.ToolStripDropDownButton();
            this.miExportTxt = new System.Windows.Forms.ToolStripMenuItem();
            this.miExportRtf = new System.Windows.Forms.ToolStripMenuItem();
            this.miExportMd = new System.Windows.Forms.ToolStripMenuItem();
            this.miExportHtml = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnImport = new System.Windows.Forms.ToolStripButton();

            // Editör ve Sağ Tık Menüsü
            this.rtbEditor = new System.Windows.Forms.RichTextBox();
            this.cmsEditor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.miSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miFind = new System.Windows.Forms.ToolStripMenuItem();
            this.miSpeak = new System.Windows.Forms.ToolStripMenuItem();
            this.miSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miSelectAll = new System.Windows.Forms.ToolStripMenuItem();

            // Durum Çubuğu
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblWordCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCharCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblReadingTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSaveStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTTSStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStudentInfo = new System.Windows.Forms.ToolStripStatusLabel();

            this.pnlSidebar.SuspendLayout();
            this.pnlSidebarHeader.SuspendLayout();
            this.pnlSidebarSearch.SuspendLayout();
            this.pnlSidebarFooter.SuspendLayout();
            this.tblActionButtons.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.tlpNoteHeader.SuspendLayout();
            this.tsFormatting.SuspendLayout();
            this.cmsEditor.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // ----------------------------------------------------
            // pnlSidebar
            // ----------------------------------------------------
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Width = 280;
            this.pnlSidebar.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSidebar.Controls.Add(this.lstNotes);
            this.pnlSidebar.Controls.Add(this.pnlSidebarFooter);
            this.pnlSidebar.Controls.Add(this.pnlSidebarSearch);
            this.pnlSidebar.Controls.Add(this.pnlSidebarHeader);

            // pnlSidebarHeader
            this.pnlSidebarHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSidebarHeader.Height = 55;
            this.pnlSidebarHeader.Controls.Add(this.lblAppTitle);
            this.pnlSidebarHeader.Controls.Add(this.lblNoteCountBadge);

            this.lblAppTitle.Text = "Notlarım";
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.Location = new System.Drawing.Point(0, 10);
            this.lblAppTitle.AutoSize = true;

            this.lblNoteCountBadge.Text = "0 Not";
            this.lblNoteCountBadge.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNoteCountBadge.Location = new System.Drawing.Point(185, 14);
            this.lblNoteCountBadge.Size = new System.Drawing.Size(85, 20);
            this.lblNoteCountBadge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblNoteCountBadge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoteCountBadge.AutoSize = false;

            // pnlSidebarSearch
            this.pnlSidebarSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSidebarSearch.Height = 72;
            this.pnlSidebarSearch.Controls.Add(this.txtSearch);
            this.pnlSidebarSearch.Controls.Add(this.cmbCategoryFilter);

            this.txtSearch.Location = new System.Drawing.Point(0, 5);
            this.txtSearch.Width = 260;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSearch.PlaceholderText = "Notlarda ara... (Ctrl+F)";

            this.cmbCategoryFilter.Location = new System.Drawing.Point(0, 36);
            this.cmbCategoryFilter.Width = 260;
            this.cmbCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 9F);

            // lstNotes
            this.lstNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstNotes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstNotes.ItemHeight = 62;
            this.lstNotes.IntegralHeight = false;
            this.lstNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstNotes.Font = new System.Drawing.Font("Segoe UI", 9F);

            // pnlSidebarFooter
            this.pnlSidebarFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSidebarFooter.Height = 88;
            this.pnlSidebarFooter.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pnlSidebarFooter.Controls.Add(this.tblActionButtons);
            this.pnlSidebarFooter.Controls.Add(this.btnNewNote);

            this.btnNewNote.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNewNote.Height = 38;
            this.btnNewNote.Text = "+ Yeni Not";
            this.btnNewNote.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnNewNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewNote.Cursor = System.Windows.Forms.Cursors.Hand;

            this.tblActionButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tblActionButtons.Height = 36;
            this.tblActionButtons.ColumnCount = 3;
            this.tblActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tblActionButtons.Controls.Add(this.btnPin, 0, 0);
            this.tblActionButtons.Controls.Add(this.btnDuplicate, 1, 0);
            this.tblActionButtons.Controls.Add(this.btnDelete, 2, 0);

            this.btnPin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPin.Text = "★ Sabitle";
            this.btnPin.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPin.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnDuplicate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDuplicate.Text = "Kopyala";
            this.btnDuplicate.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnDuplicate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDuplicate.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Text = "Çöpe At";
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;

            // ----------------------------------------------------
            // pnlContent
            // ----------------------------------------------------
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Controls.Add(this.rtbEditor);
            this.pnlContent.Controls.Add(this.tsFormatting);
            this.pnlContent.Controls.Add(this.tlpNoteHeader);

            // tlpNoteHeader
            this.tlpNoteHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpNoteHeader.Height = 56;
            this.tlpNoteHeader.ColumnCount = 4;
            this.tlpNoteHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpNoteHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tlpNoteHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlpNoteHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tlpNoteHeader.RowCount = 1;
            this.tlpNoteHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.tlpNoteHeader.Controls.Add(this.txtTitle, 0, 0);
            this.tlpNoteHeader.Controls.Add(this.cmbCategory, 1, 0);
            this.tlpNoteHeader.Controls.Add(this.chkPin, 2, 0);
            this.tlpNoteHeader.Controls.Add(this.btnThemeToggle, 3, 0);

            this.txtTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTitle.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            this.txtTitle.PlaceholderText = "Not başlığını giriniz...";
            this.txtTitle.Margin = new System.Windows.Forms.Padding(12, 12, 8, 12);

            this.cmbCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(4, 14, 4, 12);

            this.chkPin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkPin.Text = "★ Sabitlendi";
            this.chkPin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkPin.Margin = new System.Windows.Forms.Padding(6, 16, 4, 12);

            this.btnThemeToggle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnThemeToggle.Text = "Koyu Mod";
            this.btnThemeToggle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnThemeToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemeToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemeToggle.Margin = new System.Windows.Forms.Padding(4, 12, 10, 12);

            // tsFormatting
            this.tsFormatting.Dock = System.Windows.Forms.DockStyle.Top;
            this.tsFormatting.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsFormatting.CanOverflow = false;
            this.tsFormatting.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this.tsFormatting.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsFormatting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tscmbFontFamily,
                this.tscmbFontSize,
                new System.Windows.Forms.ToolStripSeparator(),
                this.tsbtnBold,
                this.tsbtnItalic,
                this.tsbtnUnderline,
                this.tsbtnStrike,
                this.tsSep1,
                this.tsbtnTextColor,
                this.tsbtnHighlightColor,
                this.tsSep2,
                this.tsbtnBullet,
                this.tsbtnAlignLeft,
                this.tsbtnAlignCenter,
                this.tsbtnAlignRight,
                this.tsSep3,
                this.tsbtnInsertImage,
                this.tsbtnFindReplace,
                this.tsbtnTTS,
                this.tsbtnLock,
                this.tsbtnStats,
                this.tsSep4,
                this.tsddbExport,
                this.tsbtnImport
            });

            this.tscmbFontFamily.Width = 85;
            this.tscmbFontFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscmbFontFamily.ToolTipText = "Yazı Tipi";

            this.tscmbFontSize.Width = 35;
            this.tscmbFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscmbFontSize.ToolTipText = "Yazı Boyutu";

            this.tsbtnBold.Text = "B";
            this.tsbtnBold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tsbtnBold.ToolTipText = "Kalın (Ctrl+B)";

            this.tsbtnItalic.Text = "I";
            this.tsbtnItalic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.tsbtnItalic.ToolTipText = "İtalik (Ctrl+I)";

            this.tsbtnUnderline.Text = "U";
            this.tsbtnUnderline.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.tsbtnUnderline.ToolTipText = "Altı Çizili (Ctrl+U)";

            this.tsbtnStrike.Text = "S";
            this.tsbtnStrike.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Strikeout);
            this.tsbtnStrike.ToolTipText = "Üstü Çizili";

            this.tsbtnTextColor.Text = "Renk";
            this.tsbtnTextColor.ToolTipText = "Metin Rengi";

            this.tsbtnHighlightColor.Text = "Vurgu";
            this.tsbtnHighlightColor.ToolTipText = "Vurgu Rengi";

            this.tsbtnBullet.Text = "•";
            this.tsbtnBullet.ToolTipText = "Madde İmleri Aç/Kapat";

            this.tsbtnAlignLeft.Text = "⇤";
            this.tsbtnAlignLeft.ToolTipText = "Sola Hizala";

            this.tsbtnAlignCenter.Text = "↔";
            this.tsbtnAlignCenter.ToolTipText = "Ortala";

            this.tsbtnAlignRight.Text = "⇥";
            this.tsbtnAlignRight.ToolTipText = "Sağa Hizala";

            this.tsbtnInsertImage.Text = "Resim";
            this.tsbtnInsertImage.ToolTipText = "Not içerisine resim ekle";

            this.tsbtnFindReplace.Text = "Bul";
            this.tsbtnFindReplace.ToolTipText = "Metin Ara ve Değiştir (Ctrl+F / Ctrl+H)";

            this.tsbtnTTS.Text = "Oku";
            this.tsbtnTTS.ToolTipText = "Notu Türkçe sesle sesli oku";

            this.tsbtnLock.Text = "Kilit";
            this.tsbtnLock.ToolTipText = "Notu AES-256 parolasıyla şifrele / kilidini aç";

            this.tsbtnStats.Text = "Özet";
            this.tsbtnStats.ToolTipText = "Not defteri analiz ve istatistikleri";

            // Dışa Aktar DropDown
            this.tsddbExport.Text = "Dışa Aktar ▾";
            this.tsddbExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.miExportTxt,
                this.miExportRtf,
                this.miExportMd,
                this.miExportHtml
            });

            this.miExportTxt.Text = "Düz Metin (.txt)";
            this.miExportRtf.Text = "Zengin Metin (.rtf)";
            this.miExportMd.Text = "Markdown (.md)";
            this.miExportHtml.Text = "Web Sayfası (.html)";

            this.tsbtnImport.Text = "İçe Aktar";
            this.tsbtnImport.ToolTipText = "Dosyadan (.txt, .rtf) not yükle";

            // rtbEditor
            this.rtbEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbEditor.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.rtbEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbEditor.AcceptsTab = true;
            this.rtbEditor.ContextMenuStrip = this.cmsEditor;
            this.rtbEditor.HideSelection = false;

            // cmsEditor
            this.miCut.Text = "Kes";
            this.miCut.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X;
            this.miCopy.Text = "Kopyala";
            this.miCopy.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            this.miPaste.Text = "Yapıştır";
            this.miPaste.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            this.miFind.Text = "Bul ve Değiştir...";
            this.miFind.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F;
            this.miSpeak.Text = "🔊 Sesli Oku";
            this.miSelectAll.Text = "Tümünü Seç";
            this.miSelectAll.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A;

            this.cmsEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.miCut,
                this.miCopy,
                this.miPaste,
                this.miSep1,
                this.miFind,
                this.miSpeak,
                this.miSep2,
                this.miSelectAll
            });

            // statusStrip
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblWordCount,
                this.lblCharCount,
                this.lblReadingTime,
                this.lblSaveStatus,
                this.lblTTSStatus,
                this.lblStudentInfo
            });

            this.lblWordCount.Text = "Kelimeler: 0";
            this.lblWordCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblWordCount.Padding = new System.Windows.Forms.Padding(4, 0, 8, 0);

            this.lblCharCount.Text = "Karakterler: 0";
            this.lblCharCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblCharCount.Padding = new System.Windows.Forms.Padding(4, 0, 8, 0);

            this.lblReadingTime.Text = "⏱️ ~1 dk okuma";
            this.lblReadingTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblReadingTime.Padding = new System.Windows.Forms.Padding(4, 0, 8, 0);

            this.lblSaveStatus.Text = "Kaydedildi";
            this.lblSaveStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblSaveStatus.Padding = new System.Windows.Forms.Padding(4, 0, 8, 0);

            this.lblTTSStatus.Text = "🔊 TTS Hazır";
            this.lblTTSStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblTTSStatus.Padding = new System.Windows.Forms.Padding(4, 0, 8, 0);

            this.lblStudentInfo.Spring = true;
            this.lblStudentInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStudentInfo.Text = "İsmail Özdemir - 250541109 | YMH 219 Nesne Tabanlı Programlama";

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 720);
            this.MinimumSize = new System.Drawing.Size(950, 550);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YMH 219 - Gelişmiş Not Defteri | İsmail Özdemir (250541109)";

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.statusStrip);

            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebarHeader.ResumeLayout(false);
            this.pnlSidebarHeader.PerformLayout();
            this.pnlSidebarSearch.ResumeLayout(false);
            this.pnlSidebarSearch.PerformLayout();
            this.pnlSidebarFooter.ResumeLayout(false);
            this.tblActionButtons.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.tlpNoteHeader.ResumeLayout(false);
            this.tlpNoteHeader.PerformLayout();
            this.tsFormatting.ResumeLayout(false);
            this.tsFormatting.PerformLayout();
            this.cmsEditor.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
