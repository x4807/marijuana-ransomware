using System.Net.Sockets;

namespace Ransomware.Webİşlev.SOCKS
{
    interface ISOCKS
    {
        Socket SOCKSBaglan(string ip, short port);
    }
}
