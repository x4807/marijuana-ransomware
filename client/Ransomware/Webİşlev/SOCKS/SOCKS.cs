namespace Ransomware.Webİşlev.SOCKS
{
    class SOCKS
    {
        public enum Protokoller
        {
            SOCKS5,
            SOCKS4,
            SOCKS4a
        }

        public ISOCKS SOCKSOlustur(Protokoller protokol, string proxyIp, short proxyPort)
        {
            ISOCKS socks = null;

            switch(protokol)
            {
                case Protokoller.SOCKS4:
                    socks = new SOCKS4(proxyIp, proxyPort);
                    break;
                case Protokoller.SOCKS4a:
                    socks = new SOCKS4a(proxyIp, proxyPort);
                    break;
                case Protokoller.SOCKS5:
                    socks = new SOCKS5(proxyIp, proxyPort);
                    break;
            }

            return socks;
        }

    }
}
