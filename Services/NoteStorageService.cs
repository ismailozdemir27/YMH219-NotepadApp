using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using NotepadApp.Models;

namespace NotepadApp.Services
{
    /// <summary>
    /// Notların JSON serileştirme/ayrıştırma (System.Text.Json) ile kalıcı dosyaya kaydedilmesi,
    /// güvenli yedekleme, kurtarma ve .txt/.rtf dışa aktarma işlemlerini yöneten servis.
    /// </summary>
    public class NoteStorageService
    {
        private readonly string _storageFilePath;
        private readonly string _backupFilePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public event Action<string>? OnLog;
        public event Action<string>? OnError;

        public NoteStorageService(string? customDirectory = null)
        {
            string baseDir = customDirectory ?? AppDomain.CurrentDomain.BaseDirectory;
            _storageFilePath = Path.Combine(baseDir, "notes.json");
            _backupFilePath = Path.Combine(baseDir, "notes_backup.json");

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Tüm not listesini JSON dosyasına güvenli ve atomik biçimde kaydeder.
        /// </summary>
        public bool SaveNotes(IReadOnlyList<Note> notes)
        {
            if (notes == null) return false;

            try
            {
                string json = JsonSerializer.Serialize(notes, _jsonOptions);

                // 1. Önce geçici dosyaya yaz (veri kaybını önlemek için)
                string tempFile = _storageFilePath + ".tmp";
                File.WriteAllText(tempFile, json, Encoding.UTF8);

                // 2. Mevcut dosyayı yedeğe al
                if (File.Exists(_storageFilePath))
                {
                    File.Copy(_storageFilePath, _backupFilePath, true);
                }

                // 3. Geçici dosyayı asıl dosya ile değiştir
                File.Move(tempFile, _storageFilePath, true);

                OnLog?.Invoke($"[{DateTime.Now:HH:mm:ss}] {notes.Count} adet not başarıyla JSON olarak kaydedildi.");
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Notlar kaydedilirken hata oluştu: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// JSON dosyasından notları ayrıştırarak yükler. Dosya yoksa veya bozuksa yedekten kurtarmayı dener.
        /// </summary>
        public List<Note> LoadNotes()
        {
            // 1. Asıl dosyadan yüklemeyi dene
            if (File.Exists(_storageFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_storageFilePath, Encoding.UTF8);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var notes = JsonSerializer.Deserialize<List<Note>>(json, _jsonOptions);
                        if (notes != null)
                        {
                            OnLog?.Invoke($"[{DateTime.Now:HH:mm:ss}] {notes.Count} adet not başarıyla yüklendi.");
                            return notes;
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"Asıl JSON dosyası ayrıştırılamadı ({ex.Message}), yedek dosyaya başvuruluyor...");
                }
            }

            // 2. Yedek dosyayı dene
            if (File.Exists(_backupFilePath))
            {
                try
                {
                    string backupJson = File.ReadAllText(_backupFilePath, Encoding.UTF8);
                    var backupNotes = JsonSerializer.Deserialize<List<Note>>(backupJson, _jsonOptions);
                    if (backupNotes != null && backupNotes.Count > 0)
                    {
                        OnLog?.Invoke($"Yedek dosyadan {backupNotes.Count} adet not kurtarıldı.");
                        SaveNotes(backupNotes); // Asıl dosyayı onar
                        return backupNotes;
                    }
                }
                catch { }
            }

            // 3. Hiçbir dosya yoksa örnek başlangıç notları üret
            var starters = SeedStarterNotes();
            SaveNotes(starters);
            return starters;
        }

        /// <summary>
        /// İlk çalıştırmada kullanıcının hazır şablonlarla başlaması için örnek notlar üretir.
        /// </summary>
        public List<Note> SeedStarterNotes()
        {
            return new List<Note>
            {
                new Note
                {
                    Title = "YMH 219 Laboratuvar Ödevi Kuralları",
                    Category = NoteCategories.Course,
                    IsPinned = true,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    UpdatedAt = DateTime.Now.AddDays(-2),
                    Content = "Nesne Tabanlı Programlama Laboratuvarı 6. Ek Ödevi:\n" +
                              "1. Not defteri uygulaması geliştirilecek.\n" +
                              "2. Veriler JSON biçiminde serileştirilerek kalıcı saklanacak.\n" +
                              "3. Nesne yönelimli tasarım ve dosya hata yönetimi uygulanacak.\n" +
                              "4. Son teslim tarihi: 29.11.2026."
                },
                new Note
                {
                    Title = "Proje Fikirleri ve İyileştirmeler",
                    Category = NoteCategories.Project,
                    IsPinned = true,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    UpdatedAt = DateTime.Now.AddDays(-1),
                    Content = "Not Defteri Ek Özellikleri:\n" +
                              "- Zengin metin (RichTextBox) biçimlendirmesi (Kalın, İtalik, Renk)\n" +
                              "- Anlık arama ve kategori filtreleme\n" +
                              "- Otomatik kaydetme (Auto-Save)\n" +
                              "- Düz metin (.txt) ve zengin metin (.rtf) dışa aktarma."
                },
                new Note
                {
                    Title = "Haftalık Görevler ve Alışveriş Listesi",
                    Category = NoteCategories.Personal,
                    IsPinned = false,
                    CreatedAt = DateTime.Now.AddHours(-5),
                    UpdatedAt = DateTime.Now.AddHours(-1),
                    Content = "Haftalık Alışveriş ve Notlar:\n- Defter ve tükenmez kalem\n- USB Flash Bellek\n- Laboratuvar raporu çıktısı\n- Kahve ve atıştırmalık"
                }
            };
        }

        /// <summary>
        /// Bir notu düz metin (.txt) olarak StreamWriter kullanarak dışa aktarır.
        /// </summary>
        public bool ExportToTxt(Note note, string destinationPath, string studentInfo = "İsmail Özdemir - 250541109")
        {
            if (note == null || string.IsNullOrWhiteSpace(destinationPath)) return false;

            try
            {
                using var writer = new StreamWriter(destinationPath, false, Encoding.UTF8);
                writer.WriteLine("================================================================================");
                writer.WriteLine($"BAŞLIK     : {note.Title}");
                writer.WriteLine($"KATEGORİ   : {note.Category}");
                writer.WriteLine($"OLUŞTURMA  : {note.CreatedAt:dd.MM.yyyy HH:mm:ss}");
                writer.WriteLine($"GÜNCELLEME : {note.UpdatedAt:dd.MM.yyyy HH:mm:ss}");
                writer.WriteLine($"ÖĞRENCİ    : {studentInfo} (YMH 219)");
                writer.WriteLine("================================================================================");
                writer.WriteLine();
                writer.WriteLine(note.Content);
                writer.WriteLine();
                writer.WriteLine("================================================================================");
                writer.WriteLine("YMH219-NotepadApp ile oluşturuldu.");
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Düz metin aktarılırken hata: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Bir notu zengin metin (.rtf) olarak dışa aktarır.
        /// </summary>
        public bool ExportToRtf(Note note, string destinationPath)
        {
            if (note == null || string.IsNullOrWhiteSpace(destinationPath)) return false;

            try
            {
                string rtf = !string.IsNullOrEmpty(note.RtfContent) ? note.RtfContent : note.Content;
                File.WriteAllText(destinationPath, rtf, Encoding.Default);
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"RTF aktarılırken hata: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Dosyadan (.txt veya .rtf) içerik okuyarak yeni Not nesnesi oluşturur.
        /// </summary>
        public Note? ImportFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            try
            {
                string ext = Path.GetExtension(filePath).ToLowerInvariant();
                string title = Path.GetFileNameWithoutExtension(filePath);
                string text = File.ReadAllText(filePath, Encoding.UTF8);

                var note = new Note
                {
                    Title = title,
                    Category = NoteCategories.General,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                if (ext == ".rtf")
                {
                    note.RtfContent = text;
                    note.Content = text;
                }
                else
                {
                    note.Content = text;
                }

                return note;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Dosya içe aktarılırken hata: {ex.Message}");
                return null;
            }
        }
    }
}
