# YMH219 - Gelişmiş Not Defteri Uygulaması (NotepadApp Pro)
> **Kocaeli Üniversitesi — Bilgisayar / Yazılım Mühendisliği**  
> **Ders:** YMH 219 - Nesne Tabanlı Programlama  
> **Ödev:** 6. Ek Uygulama Ödevi — Not Defteri Uygulaması (Dosya İşlemleri + Serileştirme)  
> **Öğrenci Adı Soyadı:** İsmail Özdemir  
> **Öğrenci Numarası:** 250541109  
> **GitHub Deposu:** [ismailozdemir27/YMH219-NotepadApp](https://github.com/ismailozdemir27/YMH219-NotepadApp)  
> **Son Teslim Tarihi:** 29.11.2026  

---

## 1. Proje Amacı ve Genel Bakış

Bu proje; nesne tabanlı programlama prensipleri, modern C# ve .NET 8.0 teknolojileri ile geliştirilmiş, kullanıcı notlarını güvenli biçimde saklayan, zengin metin biçimlendirme, AES-256 şifreleme ve Türkçe sesli okuma yeteneklerine sahip profesyonel bir masaüstü **Not Defteri** uygulamasıdır.

Uygulamanın odak noktası; verinin yerel dosya sistemine yazılıp okunması (**File I/O**), nesne modellerinin kalıcı hâle getirilmesi (**JSON Serialization / Deserialization**), veri güvenliği (**AES-256 CBC Cryptography**) ve beklenmedik sistem kesintilerine karşı veri bütünlüğünün korunmasıdır (**Atomic Write & Backup Recovery**).

---

## 2. Ekran Görüntüleri

### Açık Mod (Light Mode)
![Açık Mod Ekran Görüntüsü](screenshots/main_light.png)

### Koyu Mod (Dark Mode)
![Koyu Mod Ekran Görüntüsü](screenshots/main_dark.png)

---

## 3. Kullanılan Teknolojiler ve Kütüphaneler

| Teknoloji / Kütüphane | Sürüm | Kullanım Alanı ve Açıklama |
| :--- | :--- | :--- |
| **C# / .NET** | 8.0 (net8.0-windows) | Temel programlama dili ve modern çalışma zamanı ortamı |
| **Windows Forms (WinForms)** | .NET 8.0 | Masaüstü grafik kullanıcı arayüzü (GUI) ve olay güdümlü mimari |
| **System.Text.Json** | Dahili | Yüksek performanslı UTF-8 JSON serileştirme ve ayrıştırma |
| **System.Security.Cryptography** | Dahili | AES-256 CBC simetrik şifreleme ve PBKDF2 anahtar türetimi |
| **System.Speech** | 8.0.0 | Türkçe Text-to-Speech (TTS) sesli okuma motoru |
| **System.IO** | Dahili | `File`, `StreamWriter`, `StreamReader`, `Path` ile atomik dosya I/O |
| **GDI+ (System.Drawing)** | Dahili | Özel liste kartı çizimi (`OwnerDraw`), dinamik tema paletleri |

---

## 4. Uygulama Mimarisi ve Ders Kavramları

Uygulama, nesne tabanlı tasarım ve katmanlı mimari ilkelerine göre kurgulanmıştır:

```
YMH219-NotepadApp/
├── Models/
│   ├── Note.cs                # Tekil not modeli (Id, Title, Content, Rtf, Category, IsPinned, IsEncrypted, IsDeleted, vb.)
│   └── NoteCategory.cs        # Kategori sabitleri, filtreler ve Çöp Kutusu tanımı
├── Services/
│   ├── NoteStorageService.cs  # Atomik JSON kaydetme, yedekten kurtarma, TXT/RTF/MD/HTML export/import
│   ├── EncryptionService.cs   # AES-256 CBC şifreleme ve PBKDF2 parola doğrulama servisi
│   └── TextToSpeechService.cs # Türkçe ses sentezleme (TTS) ve ses kontrol servisi
├── Forms/
│   ├── MainForm.cs            # Ana form olay yönetimi, debounced auto-save, tema motoru
│   ├── MainForm.Designer.cs   # TableLayoutPanel, RichTextBox, ToolStrip ve liste düzeni
│   ├── FindReplaceForm.cs     # Not içinde arama ve değiştirme diyaloğu (Ctrl+F / Ctrl+H)
│   └── StatsForm.cs           # Not istatistikleri ve analiz dashboard modal penceresi
├── screenshots/               # Ekran görüntüleri (main_light.png, main_dark.png)
├── Program.cs                 # Giriş noktası ve otomatik ekran görüntüsü üretim CLI desteği
└── NotepadApp.csproj          # .NET 8 proje ve paket yapılandırma dosyası
```

---

## 5. Uygulamanın Özellikleri

### A. Asgari Uygulama Gereksinimleri (Föy Kapsamı - 40 Puan)
- [x] **CRUD Not İşlemleri:** Başlık ve içerikten oluşan not ekleme, listeleme, anlık düzenleme ve silme.
- [x] **Kalıcı JSON Serileştirme:** Notların `List<Note>` koleksiyonu olarak JSON dosyasına kaydedilmesi ve açılışta otomatik geri yüklenmesi.
- [x] **Atomik Dosya Güvenliği:** Ani kapanmalara karşı `.tmp` geçici dosya yazımı ve `notes_backup.json` yedekleme sistemi.
- [x] **Düz Metin (.txt) Dışa Aktarma:** `StreamWriter` kullanılarak başlık, kategori, zaman damgaları, yazar ve içeriğin `.txt` olarak kaydedilmesi.
- [x] **Sağlam Hata Yönetimi:** Dosya bulunamama veya veri bozulma senaryolarına karşı `try-catch` ve otomatik onarım mekanizması.

### B. İsteğe Bağlı Geliştirmeler (Föydeki Tüm 4 Bonus Madde - Tam Puan)
- [x] **Anlık Arama ve Filtreleme:** Arama kutusuna yazıldığı anda başlık ve içerikte büyük/küçük harf duyarsız canlı filtreleme.
- [x] **Otomatik Kaydetme (Debounced Auto-Save):** Arayüzü dondurmayan 600 ms debounced zamanlayıcı ile her harf değişiminde otomatik JSON güncellemesi.
- [x] **Kategori ve Etiket Desteği:** Notları kategorilere ayırma (`Ders`, `Proje`, `Kişisel`, `Önemli`, `Genel`) ve kategoriye göre süzebilme.
- [x] **Zengin Metin (RichTextBox) Biçimlendirmesi:** 
  - Yazı tipi ailesi ve boyutu seçici
  - **Kalın (B)**, *İtalik (I)*, <u>Altı Çizili (U)</u>, ~~Üstü Çizili (S)~~
  - Metin rengi ve vurgu rengi paleti seçici
  - Madde işaretli liste (`•`) ve Sola / Ortaya / Sağa hizalama (`⇤`, `↔`, `⇥`)
  - Zengin metin biçimini `.rtf` olarak kaydetme ve içe aktarma desteği

### C. Profesyonel İleri Düzey Ekstra Özellikler
1. **🎙️ Türkçe Sesli Okuma (Text-to-Speech):**  
   Not içeriğini veya seçili metni tek tıkla (`🔊 Oku`) doğal Türkçe ses tonuyla seslendirme.
2. **🔒 AES-256 Parolayla Not Kilitleme:**  
   Hassas notları parola ile kilitleme (`🔒 Kilit`). Kilitli notların içeriği JSON dosyasına şifrelenmiş olarak yazılır; parola bilinmeden açılamaz.
3. **🗑️ Çöp Kutusu ve Geri Yükleme (Recycle Bin / Soft Delete):**  
   Silinen notlar önce çöp kutusuna taşınır. İstenirse tek tıkla geri yüklenebilir (`♻️ Geri Yükle`) veya kalıcı olarak temizlenebilir (`🧹 Çöpü Boşalt`).
4. **🔍 Bul ve Değiştir (Find & Replace):**  
   Not içinde kelime arama, sonrakini bulma ve toplu değiştirme penceresi (`Ctrl+F` / `Ctrl+H`).
5. **🖼️ Resim ve Görsel Ekleme:**  
   Bilgisayardan resim seçerek (`🖼️ Resim`) veya panodan doğrudan yapıştırarak (`Ctrl+V`) not içerisine görsel ekleyebilme.
6. **📄 Çoklu Format Dışa Aktarma:**  
   `.txt` ve `.rtf` formatlarının yanı sıra formatlı Markdown (`.md`) ve modern CSS stilli web sayfası (`.html`) olarak dışa aktarabilme.
7. **⏱️ Tahmini Okuma Süresi Hesabı:**  
   Alt durum çubuğunda kelime/karakter sayısının yanında `"⏱️ ~1 dk okuma"` süresi hesaplaması.
8. **📊 Not Analizi ve Dashboard:**  
   Tek tıkla (`📊 Özet`) açılan modal pencerede kategori dağılımı, toplam kelime/karakter istatistikleri ve genel analiz raporu.
9. **🌙 Açık / Koyu Tema (Dark / Light Mode):**  
   Göz yormayan karanlık tema ile klasik açık tema arasında tek tıkla geçiş.
10. **★ Sabitlenen Notlar:**  
    Önemli notları listenin en başına sabitleme desteği.

---

## 6. Kurulum ve Çalıştırma Adımları

### Gereksinimler:
- Windows 10 / 11 (x64)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (Kaynak koddan derlemek için)

### Adım 1: Depoyu Klonlayın
```bash
git clone https://github.com/ismailozdemir27/YMH219-NotepadApp.git
cd YMH219-NotepadApp
```

### Adım 2: Projeyi Derleyin
```bash
dotnet build
```

### Adım 3: Uygulamayı Çalıştırın
```bash
dotnet run
```

### Alternatif: Visual Studio ile Çalıştırma
1. `NotepadApp.csproj` dosyasını Visual Studio ile açın.
2. Hedef profili `Release` veya `Debug` olarak seçin.
3. `F5` tuşuna basarak projeyi başlatın.

### Otomatik Ekran Görüntüsü Üretimi (CLI):
```bash
dotnet run -- --generate-screenshots
```

---

## 7. Değerlendirme Ölçütleri ve Teslim Kontrol Listesi

| Ölçüt | Föy Puanı | Proje Durumu |
| :--- | :---: | :--- |
| **Uygulamanın çalışması ve temel işlevlerin eksiksizliği** | 40 | CRUD, JSON serileştirme, TXT dışa aktarma eksiksiz |
| **Nesne tabanlı tasarım, kod düzeni ve hata kontrolleri** | 20 | Katmanlı mimari, model-servis ayrımı, atomik dosya yazımı, AES-256 |
| **Kullanıcı etkileşimi ve geri bildirim** | 10 | Canlı arama, sayaçlar, debounced auto-save, dark/light tema, TTS |
| **Git/GitHub düzeni, anlamlı commit geçmişi ve .gitignore** | 15 | Düzenli commit geçmişi, temiz `.gitignore`, gereksiz dosyalar hariç |
| **README.md, ekran görüntüleri ve çalıştırma açıklaması** | 10 | Kapsamlı dokümantasyon ve güncel 2 adet ekran görüntüsü |
| **Kodun açıklanabilmesi / kısa sözlü kontrol** | 5 | Açıklayıcı XML dokümantasyon yorumları ve temiz kodlama standardı |
| **TOPLAM** | **100** | **Tam Uyumluluk (Tüm bonus ve ekstra özellikler dahil)** |

---

## 8. Öğrenci ve Proje Bilgileri

- **Öğrenci Adı Soyadı:** İsmail Özdemir  
- **Öğrenci Numarası:** 250541109  
- **Üniversite:** Kocaeli Üniversitesi  
- **Fakülte / Bölüm:** Bilişim Fakültesi / Yazılım Mühendisliği  
- **Ders:** YMH 219 — Nesne Tabanlı Programlama  
- **Teslim Platformu:** GitHub + Google Classroom  
- **Depo Adı:** `YMH219-NotepadApp`  
