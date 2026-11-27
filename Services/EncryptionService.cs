using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NotepadApp.Services
{
    /// <summary>
    /// Hassas notların AES-256 CBC şifreleme algoritması ve PBKDF2 anahtar türetimi
    /// ile güvenli biçimde şifrelenmesini ve çözülmesini sağlayan kripto servisi.
    /// </summary>
    public static class EncryptionService
    {
        private const int SaltBytesLength = 16;
        private const int KeyBytesLength = 32; // 256-bit
        private const int IvBytesLength = 16;  // 128-bit AES block size
        private const int Iterations = 50000;

        /// <summary>
        /// Parolanın SHA-256 özetini (hash) üretir.
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Girilen parolanın saklanan hash ile eşleşip eşleşmediğini doğrular.
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash)) return true;
            string inputHash = HashPassword(password);
            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(inputHash),
                Encoding.UTF8.GetBytes(storedHash)
            );
        }

        /// <summary>
        /// Düz metni verilen parola ile AES-256 kullanarak şifreler.
        /// Çıktı: Base64 kodlanmış [Salt (16B) + IV (16B) + CipherText]
        /// </summary>
        public static string Encrypt(string plainText, string password)
        {
            if (string.IsNullOrEmpty(plainText)) return string.Empty;
            if (string.IsNullOrEmpty(password)) return plainText;

            byte[] salt = RandomNumberGenerator.GetBytes(SaltBytesLength);
            using var kdf = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] key = kdf.GetBytes(KeyBytesLength);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();
            byte[] iv = aes.IV;

            using var ms = new MemoryStream();
            ms.Write(salt, 0, salt.Length);
            ms.Write(iv, 0, iv.Length);

            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cs, Encoding.UTF8))
            {
                writer.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Şifrelenmiş metni verilen parola ile çözer. Hatalı parola durumunda CryptographicException fırlatır.
        /// </summary>
        public static string Decrypt(string cipherTextBase64, string password)
        {
            if (string.IsNullOrEmpty(cipherTextBase64)) return string.Empty;
            if (string.IsNullOrEmpty(password)) return cipherTextBase64;

            byte[] fullBytes = Convert.FromBase64String(cipherTextBase64);
            if (fullBytes.Length < SaltBytesLength + IvBytesLength)
                throw new CryptographicException("Geçersiz şifrelenmiş veri formatı.");

            byte[] salt = new byte[SaltBytesLength];
            Array.Copy(fullBytes, 0, salt, 0, SaltBytesLength);

            byte[] iv = new byte[IvBytesLength];
            Array.Copy(fullBytes, SaltBytesLength, iv, 0, IvBytesLength);

            using var kdf = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] key = kdf.GetBytes(KeyBytesLength);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            int cipherOffset = SaltBytesLength + IvBytesLength;
            int cipherLength = fullBytes.Length - cipherOffset;

            using var ms = new MemoryStream(fullBytes, cipherOffset, cipherLength);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var reader = new StreamReader(cs, Encoding.UTF8);

            return reader.ReadToEnd();
        }
    }
}
