using System;
using System.Drawing;
using System.Windows.Forms;

namespace NotepadApp.Forms
{
    /// <summary>
    /// Not içerisinde kelime arama ve toplu değiştirme (Ctrl+F / Ctrl+H) penceresi.
    /// </summary>
    public class FindReplaceForm : Form
    {
        private readonly RichTextBox _targetEditor;

        private Label lblFind = null!;
        private TextBox txtFind = null!;
        private Label lblReplace = null!;
        private TextBox txtReplace = null!;
        private CheckBox chkMatchCase = null!;
        private CheckBox chkWholeWord = null!;
        private Button btnFindNext = null!;
        private Button btnReplace = null!;
        private Button btnReplaceAll = null!;
        private Button btnClose = null!;

        public FindReplaceForm(RichTextBox targetEditor, bool isReplaceMode = false)
        {
            _targetEditor = targetEditor ?? throw new ArgumentNullException(nameof(targetEditor));

            InitializeControls(isReplaceMode);
        }

        private void InitializeControls(bool isReplaceMode)
        {
            this.Text = isReplaceMode ? "Bul ve Değiştir (Ctrl+H)" : "Metin Ara (Ctrl+F)";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(420, isReplaceMode ? 210 : 170);
            this.Font = new Font("Segoe UI", 9F);

            lblFind = new Label { Text = "Aranan:", Location = new Point(15, 18), AutoSize = true };
            txtFind = new TextBox { Location = new Point(80, 15), Width = 210 };
            if (!string.IsNullOrEmpty(_targetEditor.SelectedText))
            {
                txtFind.Text = _targetEditor.SelectedText;
            }

            lblReplace = new Label { Text = "Yeni Metin:", Location = new Point(15, 52), AutoSize = true, Visible = isReplaceMode };
            txtReplace = new TextBox { Location = new Point(80, 49), Width = 210, Visible = isReplaceMode };

            chkMatchCase = new CheckBox { Text = "Büyük/Küçük harf eşleştir", Location = new Point(80, isReplaceMode ? 82 : 52), AutoSize = true };
            chkWholeWord = new CheckBox { Text = "Yalnızca tam sözcükleri bul", Location = new Point(80, isReplaceMode ? 108 : 78), AutoSize = true };

            btnFindNext = new Button { Text = "Sonrakini Bul", Location = new Point(305, 13), Width = 100, Height = 28 };
            btnFindNext.Click += (s, e) => FindNext();

            btnReplace = new Button { Text = "Değiştir", Location = new Point(305, 47), Width = 100, Height = 28, Visible = isReplaceMode };
            btnReplace.Click += (s, e) => ReplaceCurrent();

            btnReplaceAll = new Button { Text = "Tümünü Değiştir", Location = new Point(305, 81), Width = 100, Height = 28, Visible = isReplaceMode };
            btnReplaceAll.Click += (s, e) => ReplaceAll();

            btnClose = new Button { Text = "Kapat", Location = new Point(305, isReplaceMode ? 115 : 47), Width = 100, Height = 28 };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblFind, txtFind,
                lblReplace, txtReplace,
                chkMatchCase, chkWholeWord,
                btnFindNext, btnReplace, btnReplaceAll, btnClose
            });

            this.AcceptButton = btnFindNext;
            this.CancelButton = btnClose;
        }

        private RichTextBoxFinds GetFindOptions()
        {
            var options = RichTextBoxFinds.None;
            if (chkMatchCase.Checked) options |= RichTextBoxFinds.MatchCase;
            if (chkWholeWord.Checked) options |= RichTextBoxFinds.WholeWord;
            return options;
        }

        private bool FindNext()
        {
            string query = txtFind.Text;
            if (string.IsNullOrEmpty(query)) return false;

            int startIndex = _targetEditor.SelectionStart + _targetEditor.SelectionLength;
            if (startIndex >= _targetEditor.TextLength) startIndex = 0;

            int index = _targetEditor.Find(query, startIndex, GetFindOptions());
            if (index == -1 && startIndex > 0)
            {
                // Baştan tekrar dene (wrap around)
                index = _targetEditor.Find(query, 0, GetFindOptions());
            }

            if (index != -1)
            {
                _targetEditor.Select(index, query.Length);
                _targetEditor.ScrollToCaret();
                _targetEditor.Focus();
                return true;
            }

            MessageBox.Show($"\"{query}\" ifadesi bulunamadı.", "Arama Sonucu", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        private void ReplaceCurrent()
        {
            if (_targetEditor.SelectionLength > 0 &&
                _targetEditor.SelectedText.Equals(txtFind.Text, chkMatchCase.Checked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
            {
                _targetEditor.SelectedText = txtReplace.Text;
            }
            FindNext();
        }

        private void ReplaceAll()
        {
            string query = txtFind.Text;
            if (string.IsNullOrEmpty(query)) return;

            int count = 0;
            int startIndex = 0;
            var options = GetFindOptions();

            _targetEditor.SuspendLayout();
            while (startIndex < _targetEditor.TextLength)
            {
                int index = _targetEditor.Find(query, startIndex, options);
                if (index == -1) break;

                _targetEditor.Select(index, query.Length);
                _targetEditor.SelectedText = txtReplace.Text;
                count++;
                startIndex = index + txtReplace.Text.Length;
            }
            _targetEditor.ResumeLayout();

            MessageBox.Show($"{count} adet eşleşme değiştirildi.", "Değiştirme Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
