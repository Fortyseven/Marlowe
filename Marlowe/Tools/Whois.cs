using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Marlowe.Tools
{
    class Whois
    {
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
            System.Buffer.BlockCopy( str.ToCharArray(), 0, bytes, 0, bytes.Length );
            return bytes;
        }
    }
}
