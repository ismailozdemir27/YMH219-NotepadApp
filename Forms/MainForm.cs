using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NotepadApp.Models;
using NotepadApp.Services;

namespace NotepadApp.Forms
{
    public partial class MainForm : Form
    {
        private readonly NoteStorageService _storageService;
        private readonly TextToSpeechService _ttsService;
        private readonly List<Note> _allNotes = new();
        private readonly List<Note> _filteredNotes = new();

        private Note? _currentNote;
        private bool _isUpdatingUi = false;
        private bool _isDarkMode = false;
        private readonly System.Windows.Forms.Timer _autoSaveTimer;

        // Tema Renk Paletleri
        private static readonly Color LightBg = Color.FromArgb(248, 250, 252);
        private static readonly Color LightSidebarBg = Color.FromArgb(241, 245, 249);
        private static readonly Color LightCardBg = Color.White;
        private static readonly Color LightBorder = Color.FromArgb(226, 232, 240);
        private static readonly Color LightText = Color.FromArgb(15, 23, 42);
        private static readonly Color LightSubText = Color.FromArgb(100, 116, 139);
        private static readonly Color LightAccent = Color.FromArgb(37, 99, 235);
        private static readonly Color LightSelected = Color.FromArgb(224, 238, 255);

        private static readonly Color DarkBg = Color.FromArgb(15, 23, 42);
        private static readonly Color DarkSidebarBg = Color.FromArgb(30, 41, 59);
        private static readonly Color DarkCardBg = Color.FromArgb(15, 23, 42);
        private static readonly Color DarkBorder = Color.FromArgb(51, 65, 85);
        private static readonly Color DarkText = Color.FromArgb(241, 245, 249);
        private static readonly Color DarkSubText = Color.FromArgb(148, 163, 184);
        private static readonly Color DarkAccent = Color.FromArgb(59, 130, 246);
        private static readonly Color DarkSelected = Color.FromArgb(30, 58, 110);

        public MainForm()
        {
            InitializeComponent();

            _storageService = new NoteStorageService();
            _storageService.OnLog += msg => System.Diagnostics.Debug.WriteLine(msg);
            _storageService.OnError += msg => MessageBox.Show(msg, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

            _ttsService = new TextToSpeechService();
            _ttsService.OnSpeakingStateChanged += isSpeaking =>
            {
                if (this.IsDisposed) return;
                this.BeginInvoke((Action)(() =>
                {
                    tsbtnTTS.Text = isSpeaking ? "⏹️ Durdur" : "🔊 Oku";
                    lblTTSStatus.Text = isSpeaking ? "🔊 Okunuyor..." : "🔊 TTS Hazır";
                }));
            };
            _ttsService.OnStatusMessage += msg =>
            {
                if (this.IsDisposed) return;
                this.BeginInvoke((Action)(() => lblTTSStatus.Text = msg));
            };

            _autoSaveTimer = new System.Windows.Forms.Timer { Interval = 600 };
            _autoSaveTimer.Tick += (s, e) =>
            {
                _autoSaveTimer.Stop();
                FlushCurrentNoteChanges();
                SaveAllNotes();
            };

            InitializeControls();
            ApplyTheme(false);
            LoadNotesData();
        }

        private void InitializeControls()
        {
            // Kategoriler
            cmbCategoryFilter.Items.AddRange(NoteCategories.FilterCategories);
            cmbCategoryFilter.SelectedIndex = 0;
            cmbCategoryFilter.SelectedIndexChanged += (s, e) => ApplyFilter();

            cmbCategory.Items.AddRange(NoteCategories.AvailableCategories);
            cmbCategory.SelectedIndexChanged += (s, e) =>
            {
                if (_isUpdatingUi || _currentNote == null) return;
                _currentNote.Category = cmbCategory.SelectedItem?.ToString() ?? NoteCategories.General;
                _currentNote.UpdatedAt = DateTime.Now;
                lstNotes.Invalidate();
                ScheduleAutoSave();
            };

            // Font Ailesi
            string[] preferredFonts = { "Segoe UI", "Arial", "Consolas", "Courier New", "Georgia", "Tahoma", "Times New Roman", "Verdana" };
            foreach (var f in preferredFonts) tscmbFontFamily.Items.Add(f);
            tscmbFontFamily.SelectedItem = "Segoe UI";
            tscmbFontFamily.SelectedIndexChanged += (s, e) => ChangeFontFamily();

            // Font Boyutu
            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 24, 28, 36 };
            foreach (var sz in sizes) tscmbFontSize.Items.Add(sz.ToString());
            tscmbFontSize.SelectedItem = "11";
            tscmbFontSize.SelectedIndexChanged += (s, e) => ChangeFontSize();

            // Format Butonları
            tsbtnBold.Click += (s, e) => ToggleFontStyle(FontStyle.Bold);
            tsbtnItalic.Click += (s, e) => ToggleFontStyle(FontStyle.Italic);
            tsbtnUnderline.Click += (s, e) => ToggleFontStyle(FontStyle.Underline);
            tsbtnStrike.Click += (s, e) => ToggleFontStyle(FontStyle.Strikeout);
            tsbtnTextColor.Click += (s, e) => ChangeTextColor();
            tsbtnHighlightColor.Click += (s, e) => ChangeHighlightColor();
            tsbtnBullet.Click += (s, e) => ToggleBullet();
            tsbtnAlignLeft.Click += (s, e) => rtbEditor.SelectionAlignment = HorizontalAlignment.Left;
            tsbtnAlignCenter.Click += (s, e) => rtbEditor.SelectionAlignment = HorizontalAlignment.Center;
            tsbtnAlignRight.Click += (s, e) => rtbEditor.SelectionAlignment = HorizontalAlignment.Right;

            // Gelişmiş Özellik Butonları
            tsbtnInsertImage.Click += (s, e) => InsertImageDialog();
            tsbtnFindReplace.Click += (s, e) => OpenFindReplaceDialog(false);
            tsbtnTTS.Click += (s, e) => ToggleTTS();
            tsbtnLock.Click += (s, e) => ToggleEncryption();
            tsbtnStats.Click += (s, e) => OpenStatsDashboard();

            // Dışa / İçe Aktar
            miExportTxt.Click += (s, e) => ExportCurrentNoteTxt();
            miExportRtf.Click += (s, e) => ExportCurrentNoteRtf();
            miExportMd.Click += (s, e) => ExportCurrentNoteMd();
            miExportHtml.Click += (s, e) => ExportCurrentNoteHtml();
            tsbtnImport.Click += (s, e) => ImportNoteDialog();

            // Arama ve Başlık
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            txtTitle.TextChanged += (s, e) =>
            {
                if (_isUpdatingUi || _currentNote == null) return;
                _currentNote.Title = string.IsNullOrWhiteSpace(txtTitle.Text) ? "Başlıksız Not" : txtTitle.Text;
                _currentNote.UpdatedAt = DateTime.Now;
                lstNotes.Invalidate();
                ScheduleAutoSave();
            };

            chkPin.CheckedChanged += (s, e) =>
            {
                if (_isUpdatingUi || _currentNote == null) return;
                _currentNote.IsPinned = chkPin.Checked;
                _currentNote.UpdatedAt = DateTime.Now;
                SortAndRefreshList();
                ScheduleAutoSave();
            };

            // Editör Değişimleri
            rtbEditor.TextChanged += (s, e) =>
            {
                if (_isUpdatingUi || _currentNote == null) return;
                _currentNote.Content = rtbEditor.Text ?? string.Empty;
                _currentNote.RtfContent = rtbEditor.Rtf ?? string.Empty;
                _currentNote.UpdatedAt = DateTime.Now;
                UpdateCounters();
                lstNotes.Invalidate();
                ScheduleAutoSave();
            };

            // Liste Olayları
            lstNotes.DrawItem += LstNotes_DrawItem;
            lstNotes.SelectedIndexChanged += LstNotes_SelectedIndexChanged;

            // Buton Olayları
            btnNewNote.Click += (s, e) => OnSidebarPrimaryAction();
            btnDelete.Click += (s, e) => OnSidebarDeleteAction();
            btnPin.Click += (s, e) => OnSidebarPinOrRestoreAction();
            btnDuplicate.Click += (s, e) => DuplicateCurrentNote();
            btnThemeToggle.Click += (s, e) => ToggleTheme();

            // Sağ Tık Menüsü
            miCut.Click += (s, e) => rtbEditor.Cut();
            miCopy.Click += (s, e) => rtbEditor.Copy();
            miPaste.Click += (s, e) => rtbEditor.Paste();
            miFind.Click += (s, e) => OpenFindReplaceDialog(false);
            miSpeak.Click += (s, e) => ToggleTTS();
            miSelectAll.Click += (s, e) => rtbEditor.SelectAll();

            this.FormClosing += (s, e) =>
            {
                _ttsService.Dispose();
                FlushCurrentNoteChanges();
                SaveAllNotes();
            };
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                OpenFindReplaceDialog(false);
                return true;
            }
            if (keyData == (Keys.Control | Keys.H))
            {
                OpenFindReplaceDialog(true);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Veri Yükleme ve Saklama
        private void LoadNotesData()
        {
            _allNotes.Clear();
            var notes = _storageService.LoadNotes();
            _allNotes.AddRange(notes);

            ApplyFilter();

            if (_filteredNotes.Count > 0)
            {
                lstNotes.SelectedIndex = 0;
            }
            else
            {
                CreateNewNote();
            }
        }

        private void SaveAllNotes()
        {
            bool success = _storageService.SaveNotes(_allNotes);
            if (success)
            {
                lblSaveStatus.Text = $"Kaydedildi ({DateTime.Now:HH:mm:ss})";
            }
        }

        private void ScheduleAutoSave()
        {
            lblSaveStatus.Text = "Kaydediliyor...";
            _autoSaveTimer.Stop();
            _autoSaveTimer.Start();
        }

        private void FlushCurrentNoteChanges()
        {
            if (_currentNote == null) return;
            _currentNote.Title = string.IsNullOrWhiteSpace(txtTitle.Text) ? "Başlıksız Not" : txtTitle.Text;
            _currentNote.Category = cmbCategory.SelectedItem?.ToString() ?? NoteCategories.General;
            _currentNote.IsPinned = chkPin.Checked;
            _currentNote.Content = rtbEditor.Text ?? string.Empty;
            _currentNote.RtfContent = rtbEditor.Rtf ?? string.Empty;
            _currentNote.UpdatedAt = DateTime.Now;
        }
        #endregion

        #region Not Yönetimi (CRUD & Çöp Kutusu)
        private bool IsViewingTrash => cmbCategoryFilter.SelectedItem?.ToString() == NoteCategories.Trash;

        private void OnSidebarPrimaryAction()
        {
            if (IsViewingTrash)
            {
                EmptyTrash();
            }
            else
            {
                CreateNewNote();
            }
        }

        private void OnSidebarDeleteAction()
        {
            if (IsViewingTrash)
            {
                DeleteCurrentNotePermanently();
            }
            else
            {
                MoveCurrentNoteToTrash();
            }
        }

        private void OnSidebarPinOrRestoreAction()
        {
            if (IsViewingTrash)
            {
                RestoreCurrentNoteFromTrash();
            }
            else
            {
                ToggleCurrentNotePin();
            }
        }

        private void CreateNewNote()
        {
            FlushCurrentNoteChanges();

            var newNote = new Note
            {
                Title = "Yeni Not",
                Category = NoteCategories.General,
                Content = "",
                RtfContent = "",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsPinned = false,
                IsDeleted = false
            };

            _allNotes.Insert(0, newNote);
            ApplyFilter();

            int index = _filteredNotes.IndexOf(newNote);
            if (index >= 0)
            {
                lstNotes.SelectedIndex = index;
            }

            txtTitle.Focus();
            txtTitle.SelectAll();
            ScheduleAutoSave();
        }

        private void MoveCurrentNoteToTrash()
        {
            if (_currentNote == null) return;

            var result = MessageBox.Show(
                $"\"{_currentNote.Title}\" başlıklı notu Çöp Kutusu'na taşımak istediğinize emin misiniz?",
                "Çöpe At",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _currentNote.IsDeleted = true;
                _currentNote.DeletedAt = DateTime.Now;
                _currentNote = null;
                ApplyFilter();

                if (_filteredNotes.Count > 0)
                {
                    lstNotes.SelectedIndex = 0;
                }
                else
                {
                    CreateNewNote();
                }

                SaveAllNotes();
            }
        }

        private void RestoreCurrentNoteFromTrash()
        {
            if (_currentNote == null) return;

            _currentNote.IsDeleted = false;
            _currentNote.DeletedAt = null;
            _currentNote.UpdatedAt = DateTime.Now;

            MessageBox.Show($"\"{_currentNote.Title}\" başarıyla geri yüklendi!", "Geri Yüklendi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ApplyFilter();
            if (_filteredNotes.Count > 0)
            {
                lstNotes.SelectedIndex = 0;
            }
            SaveAllNotes();
        }

        private void DeleteCurrentNotePermanently()
        {
            if (_currentNote == null) return;

            var result = MessageBox.Show(
                $"\"{_currentNote.Title}\" başlıklı notu KALICI olarak silmek istediğinize emin misiniz? Bu işlem geri alınamaz!",
                "Kalıcı Olarak Sil",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _allNotes.Remove(_currentNote);
                _currentNote = null;
                ApplyFilter();

                if (_filteredNotes.Count > 0)
                {
                    lstNotes.SelectedIndex = 0;
                }
                SaveAllNotes();
            }
        }

        private void EmptyTrash()
        {
            int count = _allNotes.Count(n => n.IsDeleted);
            if (count == 0)
            {
                MessageBox.Show("Çöp kutusu zaten boş.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Çöp kutusundaki {count} adet not kalıcı olarak silinecek. Emin misiniz?",
                "Çöp Kutusunu Boşalt",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _allNotes.RemoveAll(n => n.IsDeleted);
                _currentNote = null;
                ApplyFilter();
                SaveAllNotes();
                MessageBox.Show("Çöp kutusu tamamen boşaltıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ToggleCurrentNotePin()
        {
            if (_currentNote == null) return;
            _currentNote.IsPinned = !_currentNote.IsPinned;
            _currentNote.UpdatedAt = DateTime.Now;
            chkPin.Checked = _currentNote.IsPinned;
            btnPin.Text = _currentNote.IsPinned ? "Sabiti Kaldır" : "★ Sabitle";
            SortAndRefreshList();
            ScheduleAutoSave();
        }

        private void DuplicateCurrentNote()
        {
            if (_currentNote == null) return;
            FlushCurrentNoteChanges();

            var clone = new Note
            {
                Title = $"[Kopya] {_currentNote.Title}",
                Category = _currentNote.Category,
                Content = _currentNote.Content,
                RtfContent = _currentNote.RtfContent,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsPinned = false,
                IsDeleted = false
            };

            _allNotes.Insert(0, clone);
            ApplyFilter();

            int index = _filteredNotes.IndexOf(clone);
            if (index >= 0)
            {
                lstNotes.SelectedIndex = index;
            }
            ScheduleAutoSave();
        }
        #endregion

        #region Şifreleme ve Kilit (AES-256)
        private void ToggleEncryption()
        {
            if (_currentNote == null) return;

            if (_currentNote.IsEncrypted)
            {
                // Kilidi tamamen kaldır
                string? pass = PromptPassword("Notun kilidini kaldırmak için mevcut parolayı giriniz:");
                if (pass == null) return;

                if (!EncryptionService.VerifyPassword(pass, _currentNote.PasswordHash ?? ""))
                {
                    MessageBox.Show("Hatalı parola! Kilit kaldırılamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _currentNote.IsEncrypted = false;
                _currentNote.PasswordHash = null;
                UpdateLockStatusButton();
                lstNotes.Invalidate();
                SaveAllNotes();
                MessageBox.Show("Notun parola kilidi başarıyla kaldırıldı.", "Kilit Kaldırıldı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Parolayla kilitle
                string? pass1 = PromptPassword("Notu kilitlemek için bir parola belirleyiniz:");
                if (string.IsNullOrWhiteSpace(pass1)) return;

                string? pass2 = PromptPassword("Parolayı tekrar giriniz:");
                if (pass1 != pass2)
                {
                    MessageBox.Show("Girdiğiniz parolalar eşleşmiyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _currentNote.IsEncrypted = true;
                _currentNote.PasswordHash = EncryptionService.HashPassword(pass1);
                UpdateLockStatusButton();
                lstNotes.Invalidate();
                SaveAllNotes();
                MessageBox.Show("Not başarıyla AES-256 ile kilitlendi! Artık içeriği görmek için parola gerekecektir.", "Kilitlendi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateLockStatusButton()
        {
            if (_currentNote != null && _currentNote.IsEncrypted)
            {
                tsbtnLock.Text = "🔓 Kilidi Aç";
                tsbtnLock.ToolTipText = "Notun parola kilidini kaldır";
            }
            else
            {
                tsbtnLock.Text = "🔒 Kilitle";
                tsbtnLock.ToolTipText = "Notu AES-256 parolasıyla kilitle";
            }
        }

        private static string? PromptPassword(string promptTitle)
        {
            using var prompt = new Form
            {
                Width = 360,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "🔒 Parola Girişi",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lbl = new Label { Left = 20, Top = 15, Text = promptTitle, Width = 310, AutoSize = false };
            var txt = new TextBox { Left = 20, Top = 45, Width = 300, UseSystemPasswordChar = true };
            var btnOk = new Button { Text = "Tamam", Left = 140, Width = 85, Top = 80, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "İptal", Left = 235, Width = 85, Top = 80, DialogResult = DialogResult.Cancel };

            prompt.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
            prompt.AcceptButton = btnOk;
            prompt.CancelButton = btnCancel;

            return prompt.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }
        #endregion

        #region Sesli Okuma (TTS)
        private void ToggleTTS()
        {
            if (_ttsService.IsSpeaking)
            {
                _ttsService.Stop();
            }
            else
            {
                string textToRead = !string.IsNullOrWhiteSpace(rtbEditor.SelectedText) ? rtbEditor.SelectedText : rtbEditor.Text;
                if (!string.IsNullOrWhiteSpace(textToRead))
                {
                    _ttsService.Speak(textToRead);
                }
                else
                {
                    MessageBox.Show("Sesli okunacak bir metin bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion

        #region Liste ve Filtreleme
        private void ApplyFilter()
        {
            string keyword = txtSearch.Text.Trim().ToLowerInvariant();
            string selectedCat = cmbCategoryFilter.SelectedItem?.ToString() ?? NoteCategories.All;

            _filteredNotes.Clear();

            // Çöp kutusu ve normal not ayrımı
            IEnumerable<Note> query;
            if (selectedCat == NoteCategories.Trash)
            {
                query = _allNotes.Where(n => n.IsDeleted);
                btnNewNote.Text = "🧹 Çöpü Boşalt";
                btnPin.Text = "♻️ Geri Yükle";
                btnDelete.Text = "Kalıcı Sil";
                btnDuplicate.Enabled = false;
            }
            else
            {
                query = _allNotes.Where(n => !n.IsDeleted);
                btnNewNote.Text = "+ Yeni Not";
                btnDuplicate.Enabled = true;

                if (selectedCat == NoteCategories.Pinned)
                {
                    query = query.Where(n => n.IsPinned);
                }
                else if (selectedCat != NoteCategories.All)
                {
                    query = query.Where(n => n.Category == selectedCat);
                }
            }

            // Arama filtresi
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(n =>
                    (n.Title != null && n.Title.ToLowerInvariant().Contains(keyword)) ||
                    (n.Content != null && n.Content.ToLowerInvariant().Contains(keyword)));
            }

            // Sabitlenenler en başta, sonra güncellenme tarihine göre sırala
            var sorted = query.OrderByDescending(n => n.IsPinned)
                              .ThenByDescending(n => n.UpdatedAt)
                              .ToList();

            _filteredNotes.AddRange(sorted);

            lblNoteCountBadge.Text = $"{_filteredNotes.Count} / {_allNotes.Count(n => !n.IsDeleted)} Not";

            lstNotes.BeginUpdate();
            lstNotes.Items.Clear();
            foreach (var note in _filteredNotes)
            {
                lstNotes.Items.Add(note);
            }
            lstNotes.EndUpdate();

            // Seçimi ayarla
            if (_currentNote != null && _filteredNotes.Contains(_currentNote))
            {
                lstNotes.SelectedItem = _currentNote;
            }
            else if (_filteredNotes.Count > 0)
            {
                lstNotes.SelectedIndex = 0;
            }
        }

        private void SortAndRefreshList()
        {
            var noteToKeep = _currentNote;
            ApplyFilter();
            if (noteToKeep != null && _filteredNotes.Contains(noteToKeep))
            {
                lstNotes.SelectedItem = noteToKeep;
            }
        }

        private void LstNotes_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstNotes.SelectedItem is Note selectedNote)
            {
                if (_currentNote != null && _currentNote != selectedNote)
                {
                    FlushCurrentNoteChanges();
                }

                _currentNote = selectedNote;
                DisplayCurrentNote();
            }
        }

        private void DisplayCurrentNote()
        {
            if (_currentNote == null) return;

            _isUpdatingUi = true;
            try
            {
                txtTitle.Text = _currentNote.Title;
                chkPin.Checked = _currentNote.IsPinned;

                if (!IsViewingTrash)
                {
                    btnPin.Text = _currentNote.IsPinned ? "Sabiti Kaldır" : "★ Sabitle";
                    btnDelete.Text = "Çöpe At";
                }

                UpdateLockStatusButton();

                if (cmbCategory.Items.Contains(_currentNote.Category))
                {
                    cmbCategory.SelectedItem = _currentNote.Category;
                }
                else
                {
                    cmbCategory.SelectedItem = NoteCategories.General;
                }

                // İçeriği doldur
                if (!string.IsNullOrEmpty(_currentNote.RtfContent) && _currentNote.RtfContent.TrimStart().StartsWith("{\\rtf"))
                {
                    try
                    {
                        rtbEditor.Rtf = _currentNote.RtfContent;
                    }
                    catch
                    {
                        rtbEditor.Text = _currentNote.Content;
                    }
                }
                else
                {
                    rtbEditor.Text = _currentNote.Content;
                }

                UpdateCounters();
            }
            finally
            {
                _isUpdatingUi = false;
            }
        }

        private void UpdateCounters()
        {
            int words = _currentNote?.WordCount ?? 0;
            int chars = _currentNote?.CharCount ?? 0;
            int minutes = _currentNote?.ReadingTimeMinutes ?? 1;

            lblWordCount.Text = $"Kelimeler: {words:N0}";
            lblCharCount.Text = $"Karakterler: {chars:N0}";
            lblReadingTime.Text = $"⏱️ ~{minutes} dk okuma";
        }
        #endregion

        #region Özel Liste Çizimi (OwnerDraw)
        private void LstNotes_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= _filteredNotes.Count) return;

            var note = _filteredNotes[e.Index];
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color bgColor = isSelected ? (_isDarkMode ? DarkSelected : LightSelected) : (_isDarkMode ? DarkSidebarBg : LightSidebarBg);
            Color titleColor = isSelected ? (_isDarkMode ? Color.White : LightAccent) : (_isDarkMode ? DarkText : LightText);
            Color metaColor = _isDarkMode ? DarkSubText : LightSubText;
            Color snippetColor = _isDarkMode ? Color.FromArgb(180, 190, 205) : Color.FromArgb(120, 130, 145);

            using var brushBg = new SolidBrush(bgColor);
            e.Graphics.FillRectangle(brushBg, e.Bounds);

            // Sol tarafa seçili göstergesi mavi şerit çiz
            if (isSelected)
            {
                using var accentBrush = new SolidBrush(_isDarkMode ? DarkAccent : LightAccent);
                e.Graphics.FillRectangle(accentBrush, e.Bounds.X, e.Bounds.Y, 4, e.Bounds.Height);
            }

            int x = e.Bounds.X + 12;
            int y = e.Bounds.Y + 6;

            // 1. Satır: Pin + Lock + Başlık
            int currentX = x;
            if (note.IsPinned)
            {
                using var fontStar = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                using var brushStar = new SolidBrush(Color.FromArgb(245, 158, 11)); // Amber star
                e.Graphics.DrawString("★", fontStar, brushStar, currentX, y);
                currentX += 18;
            }

            if (note.IsEncrypted)
            {
                using var fontLock = new Font("Segoe UI", 9F, FontStyle.Regular);
                using var brushLock = new SolidBrush(Color.FromArgb(239, 68, 68)); // Kırmızı kilit
                e.Graphics.DrawString("🔒", fontLock, brushLock, currentX, y);
                currentX += 18;
            }

            using (var fontTitle = new Font("Segoe UI", 9.5F, FontStyle.Bold))
            using (var brushTitle = new SolidBrush(titleColor))
            {
                var titleRect = new Rectangle(currentX, y, e.Bounds.Right - currentX - 10, 20);
                e.Graphics.DrawString(note.Title, fontTitle, brushTitle, titleRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap });
            }

            // 2. Satır: Kategori Rozeti + Tarih
            y += 20;
            string catText = $"[{note.Category}] • {note.FormattedDate}";
            using (var fontMeta = new Font("Segoe UI", 8F, FontStyle.Regular))
            using (var brushMeta = new SolidBrush(metaColor))
            {
                e.Graphics.DrawString(catText, fontMeta, brushMeta, x, y);
            }

            // 3. Satır: İçerik Önizleme
            y += 16;
            string preview = note.PreviewText;
            using (var fontPreview = new Font("Segoe UI", 8F, FontStyle.Italic))
            using (var brushPreview = new SolidBrush(snippetColor))
            {
                var previewRect = new Rectangle(x, y, e.Bounds.Width - 24, 16);
                e.Graphics.DrawString(preview, fontPreview, brushPreview, previewRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap });
            }

            // Alt ayırıcı çizgi
            using var penBorder = new Pen(_isDarkMode ? DarkBorder : LightBorder);
            e.Graphics.DrawLine(penBorder, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
        }
        #endregion

        #region Zengin Metin Biçimlendirme & Medya
        private void ToggleFontStyle(FontStyle style)
        {
            if (rtbEditor.SelectionFont == null) return;
            FontStyle currentStyle = rtbEditor.SelectionFont.Style;
            FontStyle newStyle = currentStyle ^ style;
            rtbEditor.SelectionFont = new Font(rtbEditor.SelectionFont.FontFamily, rtbEditor.SelectionFont.Size, newStyle);
        }

        private void ChangeFontFamily()
        {
            if (tscmbFontFamily.SelectedItem == null || rtbEditor.SelectionFont == null) return;
            string family = tscmbFontFamily.SelectedItem.ToString()!;
            FontStyle style = rtbEditor.SelectionFont.Style;
            float size = rtbEditor.SelectionFont.Size;
            rtbEditor.SelectionFont = new Font(family, size, style);
        }

        private void ChangeFontSize()
        {
            if (tscmbFontSize.SelectedItem == null || rtbEditor.SelectionFont == null) return;
            if (float.TryParse(tscmbFontSize.SelectedItem.ToString(), out float newSize))
            {
                string family = rtbEditor.SelectionFont.FontFamily.Name;
                FontStyle style = rtbEditor.SelectionFont.Style;
                rtbEditor.SelectionFont = new Font(family, newSize, style);
            }
        }

        private void ChangeTextColor()
        {
            using var cd = new ColorDialog { Color = rtbEditor.SelectionColor };
            if (cd.ShowDialog() == DialogResult.OK)
            {
                rtbEditor.SelectionColor = cd.Color;
            }
        }

        private void ChangeHighlightColor()
        {
            using var cd = new ColorDialog { Color = rtbEditor.SelectionBackColor };
            if (cd.ShowDialog() == DialogResult.OK)
            {
                rtbEditor.SelectionBackColor = cd.Color;
            }
        }

        private void ToggleBullet()
        {
            rtbEditor.SelectionBullet = !rtbEditor.SelectionBullet;
        }

        private void InsertImageDialog()
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Resim Dosyaları (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif",
                Title = "Nota Resim Ekle"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var img = Image.FromFile(ofd.FileName);
                    Clipboard.SetImage(img);
                    rtbEditor.Paste();
                    MessageBox.Show("Resim başarıyla nota eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Resim eklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenFindReplaceDialog(bool isReplaceMode)
        {
            var dlg = new FindReplaceForm(rtbEditor, isReplaceMode);
            dlg.Show(this);
        }

        private void OpenStatsDashboard()
        {
            FlushCurrentNoteChanges();
            using var stats = new StatsForm(_allNotes);
            stats.ShowDialog(this);
        }
        #endregion

        #region İçe / Dışa Aktarma
        private void ExportCurrentNoteTxt()
        {
            if (_currentNote == null) return;
            FlushCurrentNoteChanges();

            using var sfd = new SaveFileDialog
            {
                Filter = "Düz Metin Dosyası (*.txt)|*.txt",
                FileName = $"{SanitizeFileName(_currentNote.Title)}.txt",
                Title = "Notu Düz Metin Olarak Kaydet"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bool ok = _storageService.ExportToTxt(_currentNote, sfd.FileName);
                if (ok) MessageBox.Show("Not başarıyla düz metin (.txt) olarak dışa aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportCurrentNoteRtf()
        {
            if (_currentNote == null) return;
            FlushCurrentNoteChanges();

            using var sfd = new SaveFileDialog
            {
                Filter = "Zengin Metin Dosyası (*.rtf)|*.rtf",
                FileName = $"{SanitizeFileName(_currentNote.Title)}.rtf",
                Title = "Notu Zengin Metin Olarak Kaydet"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bool ok = _storageService.ExportToRtf(_currentNote, sfd.FileName);
                if (ok) MessageBox.Show("Not başarıyla zengin metin (.rtf) olarak dışa aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportCurrentNoteMd()
        {
            if (_currentNote == null) return;
            FlushCurrentNoteChanges();

            using var sfd = new SaveFileDialog
            {
                Filter = "Markdown Dosyası (*.md)|*.md",
                FileName = $"{SanitizeFileName(_currentNote.Title)}.md",
                Title = "Notu Markdown Olarak Kaydet"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bool ok = _storageService.ExportToMarkdown(_currentNote, sfd.FileName);
                if (ok) MessageBox.Show("Not başarıyla Markdown (.md) olarak dışa aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportCurrentNoteHtml()
        {
            if (_currentNote == null) return;
            FlushCurrentNoteChanges();

            using var sfd = new SaveFileDialog
            {
                Filter = "Web Sayfası (*.html)|*.html",
                FileName = $"{SanitizeFileName(_currentNote.Title)}.html",
                Title = "Notu HTML Olarak Kaydet"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bool ok = _storageService.ExportToHtml(_currentNote, sfd.FileName);
                if (ok) MessageBox.Show("Not başarıyla HTML (.html) sayfası olarak dışa aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ImportNoteDialog()
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Desteklenen Metin Dosyaları (*.txt;*.rtf)|*.txt;*.rtf|Düz Metin (*.txt)|*.txt|Zengin Metin (*.rtf)|*.rtf",
                Title = "Not Dosyası Seç"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var importedNote = _storageService.ImportFromFile(ofd.FileName);
                if (importedNote != null)
                {
                    _allNotes.Insert(0, importedNote);
                    ApplyFilter();
                    lstNotes.SelectedItem = importedNote;
                    SaveAllNotes();
                    MessageBox.Show($"\"{importedNote.Title}\" başarıyla içe aktarıldı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private static string SanitizeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return string.IsNullOrWhiteSpace(name) ? "Not" : name;
        }
        #endregion

        #region Tema Desteği (Dark / Light)
        public void ToggleTheme()
        {
            _isDarkMode = !_isDarkMode;
            ApplyTheme(_isDarkMode);
        }

        public void ApplyTheme(bool isDark)
        {
            _isDarkMode = isDark;

            Color bg = isDark ? DarkBg : LightBg;
            Color sideBg = isDark ? DarkSidebarBg : LightSidebarBg;
            Color cardBg = isDark ? DarkCardBg : LightCardBg;
            Color text = isDark ? DarkText : LightText;
            Color border = isDark ? DarkBorder : LightBorder;
            Color accent = isDark ? DarkAccent : LightAccent;

            this.BackColor = bg;
            this.ForeColor = text;

            // Kenar Çubuğu
            pnlSidebar.BackColor = sideBg;
            pnlSidebarHeader.BackColor = sideBg;
            pnlSidebarSearch.BackColor = sideBg;
            pnlSidebarFooter.BackColor = sideBg;
            lblAppTitle.ForeColor = text;
            lblNoteCountBadge.ForeColor = isDark ? DarkSubText : LightSubText;

            txtSearch.BackColor = isDark ? Color.FromArgb(45, 55, 72) : Color.White;
            txtSearch.ForeColor = text;

            cmbCategoryFilter.BackColor = isDark ? Color.FromArgb(45, 55, 72) : Color.White;
            cmbCategoryFilter.ForeColor = text;

            lstNotes.BackColor = sideBg;
            lstNotes.ForeColor = text;

            btnNewNote.BackColor = accent;
            btnNewNote.ForeColor = Color.White;
            btnNewNote.FlatAppearance.BorderColor = accent;

            btnPin.BackColor = isDark ? Color.FromArgb(51, 65, 85) : Color.FromArgb(241, 245, 249);
            btnPin.ForeColor = text;
            btnPin.FlatAppearance.BorderColor = border;

            btnDuplicate.BackColor = isDark ? Color.FromArgb(51, 65, 85) : Color.FromArgb(241, 245, 249);
            btnDuplicate.ForeColor = text;
            btnDuplicate.FlatAppearance.BorderColor = border;

            btnDelete.BackColor = isDark ? Color.FromArgb(127, 29, 29) : Color.FromArgb(254, 226, 226);
            btnDelete.ForeColor = isDark ? Color.FromArgb(254, 202, 202) : Color.FromArgb(185, 28, 28);
            btnDelete.FlatAppearance.BorderColor = isDark ? Color.FromArgb(185, 28, 28) : Color.FromArgb(252, 165, 165);

            // Ana İçerik
            pnlContent.BackColor = cardBg;
            tlpNoteHeader.BackColor = cardBg;

            txtTitle.BackColor = cardBg;
            txtTitle.ForeColor = text;

            cmbCategory.BackColor = isDark ? Color.FromArgb(45, 55, 72) : Color.White;
            cmbCategory.ForeColor = text;

            chkPin.ForeColor = text;

            btnThemeToggle.Text = isDark ? "Açık Mod" : "Koyu Mod";
            btnThemeToggle.BackColor = isDark ? Color.FromArgb(51, 65, 85) : Color.FromArgb(241, 245, 249);
            btnThemeToggle.ForeColor = text;
            btnThemeToggle.FlatAppearance.BorderColor = border;

            // Araç Çubuğu
            tsFormatting.BackColor = isDark ? Color.FromArgb(30, 41, 59) : Color.FromArgb(241, 245, 249);
            tsFormatting.ForeColor = text;

            // Editör
            rtbEditor.BackColor = cardBg;
            rtbEditor.ForeColor = text;

            // Durum Çubuğu
            statusStrip.BackColor = isDark ? Color.FromArgb(15, 23, 42) : Color.FromArgb(241, 245, 249);
            statusStrip.ForeColor = isDark ? DarkSubText : LightSubText;

            lstNotes.Invalidate();
        }
        #endregion
    }
}
