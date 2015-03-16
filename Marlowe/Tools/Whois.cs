using System;
using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;

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
                    "whois.ripe.net",      
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

        static public string Get( string query, string whois_server, AsyncCallback ar = null )
        {
            try {
                IPAddress ip = Dns.GetHostEntry( whois_server ).AddressList[ 0 ];
                IPEndPoint ip_end_point = new IPEndPoint( ip, WHOIS_PORT );

                Socket sock = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

                sock.ReceiveBufferSize = 8192;
                sock.SendTimeout = 1000;
                sock.ReceiveTimeout = 1000;
                sock.NoDelay = true;

                sock.Connect( ip_end_point );

                sock.Send( System.Text.Encoding.ASCII.GetBytes( query + "\n" ) );

                StringBuilder results = new StringBuilder();
                byte[] buffer = new byte[ 8192 ];

                int len = 0;
                while ( ( len = sock.Receive( buffer ) ) > 0 ) {
                    results.Append( System.Text.Encoding.ASCII.GetString( buffer, 0, len ) );
                }
                sock.Close();
                return results.ToString();
            }
            catch ( Exception e ) {
                MessageBox.Show( e.Message, "SOCKET ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return "SOCKET ERROR: " + e.Message + "\n";
            }
        }
    }
}
