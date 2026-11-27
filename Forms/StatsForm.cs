using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NotepadApp.Models;

namespace NotepadApp.Forms
{
    /// <summary>
    /// Not defteri istatistikleri ve özet paneli (Dashboard Modal).
    /// </summary>
    public class StatsForm : Form
    {
        public StatsForm(IReadOnlyList<Note> allNotes)
        {
            InitializeComponent(allNotes);
        }

        private void InitializeComponent(IReadOnlyList<Note> allNotes)
        {
            this.Text = "📊 Not İstatistikleri ve Dashboard";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(520, 480);
            this.Font = new Font("Segoe UI", 9.5F);
            this.BackColor = Color.FromArgb(248, 250, 252);

            var activeNotes = allNotes.Where(n => !n.IsDeleted).ToList();
            int totalActive = activeNotes.Count;
            int pinnedCount = activeNotes.Count(n => n.IsPinned);
            int encCount = activeNotes.Count(n => n.IsEncrypted);
            int trashCount = allNotes.Count(n => n.IsDeleted);

            int totalWords = activeNotes.Sum(n => n.WordCount);
            int totalChars = activeNotes.Sum(n => n.CharCount);
            int totalReadingMinutes = activeNotes.Sum(n => n.ReadingTimeMinutes);
            double avgWords = totalActive > 0 ? (double)totalWords / totalActive : 0;

            // Başlık Kartı
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(37, 99, 235),
                Padding = new Padding(20, 12, 20, 12)
            };
            var lblTitle = new Label
            {
                Text = "Not Defteri Analiz ve İstatistikleri",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Dock = DockStyle.Top,
                AutoSize = true
            };
            var lblSub = new Label
            {
                Text = "YMH 219 - Nesne Tabanlı Programlama | İsmail Özdemir (250541109)",
                ForeColor = Color.FromArgb(224, 238, 255),
                Font = new Font("Segoe UI", 8.5F),
                Dock = DockStyle.Bottom,
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);

            // İçerik Paneli
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            var grpSummary = new GroupBox
            {
                Text = "Genel Özet",
                Dock = DockStyle.Top,
                Height = 150,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            var tlpSummary = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(10, 10, 10, 10),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            tlpSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            tlpSummary.Controls.Add(new Label { Text = $"📝 Aktif Not Sayısı: {totalActive}", AutoSize = true }, 0, 0);
            tlpSummary.Controls.Add(new Label { Text = $"★ Sabitlenen Notlar: {pinnedCount}", AutoSize = true }, 1, 0);
            tlpSummary.Controls.Add(new Label { Text = $"🔒 Şifreli Notlar: {encCount}", AutoSize = true }, 0, 1);
            tlpSummary.Controls.Add(new Label { Text = $"🗑️ Çöp Kutusundaki Notlar: {trashCount}", AutoSize = true }, 1, 1);
            tlpSummary.Controls.Add(new Label { Text = $"📚 Toplam Kelime: {totalWords:N0}", AutoSize = true }, 0, 2);
            tlpSummary.Controls.Add(new Label { Text = $"✍️ Toplam Karakter: {totalChars:N0}", AutoSize = true }, 1, 2);
            tlpSummary.Controls.Add(new Label { Text = $"⏱️ Toplam Okuma: ~{totalReadingMinutes} dk", AutoSize = true }, 0, 3);
            tlpSummary.Controls.Add(new Label { Text = $"📊 Not Başına Ort. Kelime: {avgWords:F1}", AutoSize = true }, 1, 3);

            grpSummary.Controls.Add(tlpSummary);

            // Kategori Dağılımı
            var grpCategories = new GroupBox
            {
                Text = "Kategori Dağılımı",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Padding = new Padding(10, 20, 10, 10)
            };

            var lstCats = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            lstCats.Columns.Add("Kategori", 180);
            lstCats.Columns.Add("Not Adedi", 120);
            lstCats.Columns.Add("Toplam Kelime", 140);

            var catGroups = activeNotes.GroupBy(n => n.Category);
            foreach (var group in catGroups.OrderByDescending(g => g.Count()))
            {
                var item = new ListViewItem(group.Key);
                item.SubItems.Add(group.Count().ToString());
                item.SubItems.Add(group.Sum(n => n.WordCount).ToString("N0"));
                lstCats.Items.Add(item);
            }
            grpCategories.Controls.Add(lstCats);

            // Alt Buton
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Padding = new Padding(0, 8, 20, 8)
            };
            var btnClose = new Button
            {
                Text = "Tamam",
                Dock = DockStyle.Right,
                Width = 100,
                Height = 34,
                DialogResult = DialogResult.OK,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            pnlBottom.Controls.Add(btnClose);

            pnlBody.Controls.Add(grpCategories);
            pnlBody.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 10 });
            pnlBody.Controls.Add(grpSummary);

            this.Controls.Add(pnlBody);
            this.Controls.Add(pnlBottom);
            this.Controls.Add(pnlHeader);
        }
    }
}
