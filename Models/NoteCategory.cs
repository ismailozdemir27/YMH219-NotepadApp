using System;
using System.Collections.Generic;

namespace NotepadApp.Models
{
    /// <summary>
    /// Not defterinde desteklenen kategoriler ve filtreleme seçenekleri.
    /// </summary>
    public static class NoteCategories
    {
        public const string All = "Tümü";
        public const string General = "Genel";
        public const string Course = "Ders";
        public const string Personal = "Kişisel";
        public const string Project = "Proje";
        public const string Important = "Önemli";
        public const string Pinned = "★ Sabitlenenler";
        public const string Trash = "🗑️ Çöp Kutusu";

        public static readonly string[] AvailableCategories = new[]
        {
            General,
            Course,
            Personal,
            Project,
            Important
        };

        public static readonly string[] FilterCategories = new[]
        {
            All,
            Course,
            Personal,
            Project,
            Important,
            Pinned,
            Trash
        };
    }
}
