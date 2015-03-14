using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Marlowe
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Application.Exit();
        }

        private void btnFoo_Click( object sender, EventArgs e )
        {
            RichTextBox tab = NewTab();

            try {
                Tools.Whois.Get( "foo.com", cb => {
                    tab.SynchronizedInvoke( () => {
                        tab.AppendText("received response");
                        TcpClient remote = (TcpClient)cb.AsyncState;
                        tab.AppendText( remote.GetStream().ToString());
                    } );
                } );
            }
            catch ( Exception ex ) {
                tab.AppendText( "EXCEPTION: " + ex.Message );
            }
        }

        //private void FooCallback( IAsyncResult res )
        //{
        //    Console.WriteLine( res.ToString() );
        //}

        private RichTextBox NewTab()
        {
            // TODO: Assign icon based on task tab has
            TabPage newtabpage = new TabPage( "FooTab" );
            RichTextBox rtb = new RichTextBox();
            rtb.Dock = DockStyle.Fill;
            newtabpage.Controls.Add( rtb );
            tabControl1.Controls.Add( newtabpage );
            tabControl1.SelectedTab = newtabpage;
            return rtb;
        }
    }
}
