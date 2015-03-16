using Marlowe.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Marlowe
{
    public partial class MainForm : Form
    {
        public enum ToolType
        {
            PING,
            DNS,
            WHOIS,
            IPBLOCK,
            DIG,
            TRACE,
            FINGER,
            SMTP,
            TIME,
            WEB,
            AWAKE,
            RBL,
            ABUSE
        }

        private Font _default_tab_font;

        public MainForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Application.Exit();
        }

        private RichTextBox NewTab( ToolType tool_type, string value )
        {
            int imagelist_index = 0;
            string tool_name = "";

            switch ( tool_type ) {
                case ToolType.WHOIS:
                    imagelist_index = 0;
                    tool_name = "Whois";
                    break;
                case ToolType.PING:
                    imagelist_index = 1;
                    tool_name = "Ping";
                    break;
                case ToolType.DNS:
                    imagelist_index = 2;
                    tool_name = "DNS";
                    break;
            }

            RichTextBox rtb = new RichTextBox();

            rtb.Dock = DockStyle.Fill;
            rtb.Font = _default_tab_font;

            TabPage newtabpage = new TabPage();

            newtabpage.ImageIndex = imagelist_index;
            newtabpage.Text = value + " [" + tool_name + "]";
            newtabpage.Controls.Add( rtb );

            tabControl1.Controls.Add( newtabpage );
            tabControl1.SelectedTab = newtabpage;

            newtabpage.Focus();
            return rtb;
        }

        private void btnWhois_Click( object sender, EventArgs e )
        {
            LookAtWhois( cbAddress.Text.Trim() );
        }

        private void cbAddress_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.Enter )
                DoDefaultOperation();
        }

        /// <summary>
        /// This will take the address in the cbAddress combo box and determine a default operation
        /// for the value given (ip block for ip's, whois for domains, etc.)
        /// </summary>
        private void DoDefaultOperation()
        {
            string value = cbAddress.Text.Trim();
            if ( value.Length == 0 ) {
                // nothing to do
                return;
            }
            try {
                IPAddress.Parse( value );
                LookAtIPBlock( value );
                return;
            }
            catch ( Exception e ) {
                // Eat it, we're just checking if this is valid
            }
            try {
                if ( Dns.GetHostByName( value ) != null ) {
                    LookAtWhois( value );
                    return;
                }
            }
            catch ( Exception e ) {
                // Eat it, we're just checking if this is valid
            }

            MessageBox.Show( "Sorry, I don't know what to do with \"" + value + "\"" );
        }

        /// <summary>
        /// Performs a whois lookup on a given value
        /// </summary>
        /// <param name="value"></param>
        private void LookAtWhois( string value )
        {
            if ( value.Length == 0 )
                return;

            RichTextBox tab = NewTab( ToolType.WHOIS, value );

            tab.AppendText( DateTime.Now.ToLocalTime() + " whois " + value + "@" + cbWhoisServer.Text.Trim() + "\n" );
            tab.AppendText( "---------------------------------------------------------------\n" );

            try {
                tab.AppendText( Tools.Whois.Get( "foo.com", cbWhoisServer.Text.Trim() ) );
                tab.AppendText( "---------------------------------------------------------------\n" );
            }
            catch ( Exception ex ) {
                tab.AppendText( "EXCEPTION: " + ex.Message );
            }
        }

        /// <summary>
        /// Performs an IPBlock lookup on a given value
        /// </summary>
        /// <param name="value"></param>
        private void LookAtIPBlock( string value )
        {
            MessageBox.Show( "Would have looked at: " + value );
            //throw new NotImplementedException();
        }

        private void MainForm_Shown( object sender, EventArgs e )
        {
            _default_tab_font = new Font( "Consolas", 12.0f, FontStyle.Regular );
            cbWhoisServer.SelectedIndex = 0;
            cbAddress.Focus();
        }
    }
}
