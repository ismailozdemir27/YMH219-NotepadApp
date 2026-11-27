using System;
using System.Text.Json.Serialization;

namespace NotepadApp.Models
{
    /// <summary>
    /// Not defterindeki tekil bir notu temsil eden veri modeli.
    /// JSON serileştirme, zengin metin desteği, etiketleme ve zaman damgası içerir.
    /// </summary>
    public class Note
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [JsonPropertyName("title")]
        public string Title { get; set; } = "Başlıksız Not";

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("rtfContent")]
        public string RtfContent { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = "Genel";

        [JsonPropertyName("isPinned")]
        public bool IsPinned { get; set; } = false;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        public string FormattedDate => UpdatedAt.ToString("dd.MM.yyyy HH:mm");

        [JsonIgnore]
        public int WordCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Content)) return 0;
                var parts = Content.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return parts.Length;
            }
        }

        [JsonIgnore]
        public int CharCount => Content?.Length ?? 0;

        [JsonIgnore]
        public string PreviewText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Content)) return "(Boş içerik)";
                var firstLine = Content.Split('\n')[0].Trim();
                return firstLine.Length > 40 ? firstLine.Substring(0, 37) + "..." : firstLine;
            }
        }

        public override string ToString()
        {
            string pin = IsPinned ? "★ " : "";
            return $"{pin}{Title} [{Category}] - {FormattedDate}";
        }
    }
}
