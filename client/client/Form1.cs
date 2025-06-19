using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {
        bool terminating = false;
        bool connected = false;
        Socket clientSocket;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void connect_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name_textBox.Text))
            {
                logs.AppendText("You must enter a name before connecting.\n");
                return;
            }

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = ip_textBox.Text;
            int portNum;

            if (Int32.TryParse(port_textBox.Text, out portNum))
            {
                try
                {
                    clientSocket.Connect(IP, portNum);
                    //connect_button.Enabled = false;
                    disconnect_button.Enabled = true;
                    connected = true;
                    SendMessage("name:" + name_textBox.Text);
                    Thread receiveThread = new Thread(Receive);
                    receiveThread.Start();
                }
                catch
                {
                    logs.AppendText("Could not connect to the server!\n");
                }
            }
            else
            {
                logs.AppendText("Check the port number.\n");
            }
        }

        private void Receive()
        {
            while (connected && !terminating)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int received = clientSocket.Receive(buffer);
                    if (received > 0)
                    {
                        string incomingMessage = Encoding.Default.GetString(buffer).Trim('\0');

                        if (incomingMessage.Contains("EnableButtons"))
                        {
                            EnableButtons();
                        }
                        else if (incomingMessage.Contains("DisableButtons"))
                        {
                            DisableButtons();
                        }
                        else
                        {
                            logs.AppendText(incomingMessage + "\n");

                            if (incomingMessage.Contains("You have been eliminated"))
                            {
                                DisableButtons();
                            }
                        }
                    }
                    else
                    {
                        throw new SocketException();
                    }
                }
                catch (Exception ex)
                {
                    //logs.AppendText($"An error occurred: {ex.Message}\n{ex.StackTrace}\n");
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected.\n");
                        connect_button.Enabled = true;
                        disconnect_button.Enabled = false;
                        clientSocket.Close();
                        connected = false;
                    }
                }
            }
        }

        private void SendMessage(string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);
            clientSocket.Send(buffer);
        }

        private void disconnect_button_Click(object sender, EventArgs e)
        {
            terminating = true;
            connected = false;
            clientSocket.Close();
            connect_button.Enabled = true;
            disconnect_button.Enabled = false;
            logs.AppendText("Disconnected from the server.\n");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connected)
            {
                terminating = true;
                clientSocket.Close();
            }
            Environment.Exit(0);
        }

        private void rock_button_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                SendMessage("rock");
                logs.AppendText("You selected: Rock\n");
                DisableButtons();
            }
            else
            {
                logs.AppendText("You are not connected to the server.\n");
            }
        }

        private void paper_button_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                SendMessage("paper");
                logs.AppendText("You selected: Paper\n");
                DisableButtons();
            }
            else
            {
                logs.AppendText("You are not connected to the server.\n");
            }
        }

        private void scissors_button_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                SendMessage("scissors");
                logs.AppendText("You selected: Scissors\n");
                DisableButtons();
            }
            else
            {
                logs.AppendText("You are not connected to the server.\n");
            }
        }

        private void EnableButtons()
        {
            rock_button.Enabled = true;
            paper_button.Enabled = true;
            scissors_button.Enabled = true;
        }

        private void DisableButtons()
        {
            rock_button.Enabled = false;
            paper_button.Enabled = false;
            scissors_button.Enabled = false;
        }

        private void leave_button_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                SendMessage("leave");
                logs.AppendText("You have left the game.\n");
            }
            else
            {
                logs.AppendText("You are not connected to the server.\n");
            }
        }
    }
}
