using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NotepadApp.Forms;

namespace NotepadApp
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana giriş noktası.
        /// --generate-screenshots parametresi verilirse Light ve Dark mod ekran görüntülerini otomatik üretir.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            if (args != null && args.Any(a => a.Equals("--generate-screenshots", StringComparison.OrdinalIgnoreCase) || a.Equals("-s", StringComparison.OrdinalIgnoreCase)))
            {
                try
                {
                    Console.WriteLine("[INFO] Ekran görüntüleri oluşturuluyor...");
                    using var form = new MainForm();
                    form.Size = new Size(1160, 700);
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(50, 50);
                    form.Show();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(300);
                    Application.DoEvents();

                    string screenshotDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screenshots");
                    Directory.CreateDirectory(screenshotDir);

                    // 1. Açık Mod (Light Mode)
                    form.ApplyTheme(false);
                    form.Refresh();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(200);

                    using (var bmpLight = new Bitmap(form.Width, form.Height))
                    {
                        form.DrawToBitmap(bmpLight, new Rectangle(0, 0, form.Width, form.Height));
                        string lightPath = Path.Combine(screenshotDir, "main_light.png");
                        bmpLight.Save(lightPath, ImageFormat.Png);
                        Console.WriteLine($"[OK] Açık mod ekran görüntüsü kaydedildi: {lightPath}");
                    }

                    // 2. Koyu Mod (Dark Mode)
                    form.ApplyTheme(true);
                    form.Refresh();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(200);

                    using (var bmpDark = new Bitmap(form.Width, form.Height))
                    {
                        form.DrawToBitmap(bmpDark, new Rectangle(0, 0, form.Width, form.Height));
                        string darkPath = Path.Combine(screenshotDir, "main_dark.png");
                        bmpDark.Save(darkPath, ImageFormat.Png);
                        Console.WriteLine($"[OK] Koyu mod ekran görüntüsü kaydedildi: {darkPath}");
                    }

                    form.Close();
                    Console.WriteLine("[SUCCESS] Tüm ekran görüntüleri başarıyla üretildi.");
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[ERROR] Ekran görüntüsü alınamadı: {ex.Message}\n{ex.StackTrace}");
                    return 1;
                }
            }

            Application.Run(new MainForm());
            return 0;
        }
    }
}
