using System;
using System.Net;
using System.Net.Sockets;

namespace Marlowe.Tools
{
    class Whois
    {
        // TO - timed out
        // DNR - did not resolve
        public string[] WhoisServers =
        {
            "whois.internic.net",
            "whois.arin.net",
            //"whois.ripe.net",       // TO
            "whois.nic.am",
            "whois.aunic.net",
            //"whois.cdnnet.ca",      // DNR
            "whois.nic.ch",
            //"crsnic",               // DNR
            //"whois.eunet.es",       // TO
            "whois.nic.fr",
            "whois.nic.gov",
            "whois.apnic.net",
            //"whois.nis.garr.it",    // DNR
            //"whois.nic.ad.jp",      // TO
            //"whois.nic.nm.kr",      // DNR
            "whois.nic.li",
            //"nic.ddn.mil",          // DNR
            "whois.nic.mx",
            "domain-registry.nl",
            "whois.ripn.net",
            //"whois.internic.se",    // DNR
            "nanos.arnes.si",
            "whois.thnic.net",
            //"whois.nic.tj",         // DNR
            //"whois.nic.uk",         // TO
            "whois.ja.net",
            "nii-server.isi.edu"
            //"rwhois.exodus.net",    // TO
            //"rwhois.digex.net",     // DNR
            //"whois.geektools.com"   // TO
        };

        public const string DEFAULT_SERVER = "whois.iana.org";
        public const int WHOIS_PORT = 43;

        static public void Get( string query, AsyncCallback ar )
        {
            string server_host = DEFAULT_SERVER;

            //TcpClient tcp = new TcpClient();
            //tcp.BeginConnect(server_host, WHOIS_PORT, new AsyncCallback(), )

            TcpClient tcp = new TcpClient();
            IPAddress ip = Dns.GetHostEntry( server_host ).AddressList[ 0 ];
            IPEndPoint ip_end_point = new IPEndPoint( ip, WHOIS_PORT );

            tcp.Connect( ip_end_point );
            var stream = tcp.GetStream();

            stream.BeginWrite( GetBytes( "foo.com" ), 0, 7, ar, null );
        }

        private static void WhoisResponseCallback( IAsyncResult ar )
        {
            Console.WriteLine( ar.ToString() );
            throw new NotImplementedException();
        }

        static byte[] GetBytes( string str )
        {
            byte[] bytes = new byte[ str.Length * sizeof( char ) ];
            Buffer.BlockCopy( str.ToCharArray(), 0, bytes, 0, bytes.Length );
            return bytes;
        }
    }
}
