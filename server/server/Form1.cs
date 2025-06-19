using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Timers;
using System.Threading.Tasks;

namespace server
{
    public partial class Form1 : Form
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Dictionary<Socket, string> clients = new Dictionary<Socket, string>(); // keep the clients and their names
        private Dictionary<string, int> playerScores = new Dictionary<string, int>(); // keep the scores
        Queue<Socket> waitingClients = new Queue<Socket>(); // for waiting clients
        bool terminating = false;
        bool listening = false;
        Game game;
        private CancellationTokenSource countdownCancellationTokenSource;
        // System.Timers.Timer turnTimer;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
            LoadScores(); // load player scores at startup
            game = new Game(playerScores); 
        }

        private void LoadScores()
        {
            playerScores.Clear();
            if (File.Exists("leaderboard.txt"))
            {
                var lines = File.ReadAllLines("leaderboard.txt");
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        playerScores[parts[0]] = int.Parse(parts[1]);
                    }
                }
            }

            // Update the RichTextBox displaying the leaderboard
            UpdateLeaderboardDisplay();
        }

        private void UpdateLeaderboardDisplay()
        {
            leaderBoard_logs.Clear();
            foreach (var score in playerScores.OrderByDescending(kvp => kvp.Value))
            {
                leaderBoard_logs.AppendText($"{score.Key}: {score.Value}\n");
            }
        }

        private void SaveScores()
        {
            string filePath = Path.Combine(Application.StartupPath, "leaderboard.txt");
            try
            {
                using (var sw = new StreamWriter(filePath))
                {
                    foreach (var player in playerScores)
                    {
                        sw.WriteLine($"{player.Key},{player.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                logs.AppendText("An error occurred while saving the scores: " + ex.Message);
            }
        }

        private void listen_button_Click(object sender, EventArgs e)
        {
            int serverPort;
            if (Int32.TryParse(port_textBox.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(4);
                listening = true;
                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();
                logs.AppendText("Started listening on port: " + serverPort + "\n");
            }
            else
            {
                logs.AppendText("Please check port number.\n");
            }
        }

        private void Accept()
        {
            while (listening)
            {
                try
                {
                    Socket newClient = serverSocket.Accept();
                    if (clients.Count < 4)
                    {
                        Thread receiveThread = new Thread(() => Receive(newClient));
                        receiveThread.Start();
                    }
                    else
                    {
                        SendMessage(newClient, "The room is full, you have been placed in a queue.\n");
                        waitingClients.Enqueue(newClient);
                    }
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The listening socket closed unexpectedly.\n");
                    }
                }

                if (clients.Count == 4 && countdownCancellationTokenSource == null)
                {
                    StartGame();
                }
            }
        }

        private void Receive(Socket thisClient)
        {
            bool connected = true;
            string clientName = null;

            try
            {
                while (connected && !terminating)
                {
                    byte[] buffer = new byte[1024];
                    int received = thisClient.Receive(buffer);
                    if (received <= 0)
                    {
                        throw new SocketException();
                    }

                    string incomingMessage = Encoding.Default.GetString(buffer).Trim('\0');

                    if (incomingMessage == "leave")
                    {
                        HandlePlayerLeave(thisClient);
                        continue;
                    }

                    // Connection of the user
                    if (!clients.ContainsKey(thisClient) && incomingMessage.StartsWith("name:"))
                    {
                        clientName = incomingMessage.Substring(5);
                        if (!clients.Values.Contains(clientName))
                        {
                            if (game.GetLeftPlayers().Contains(clientName))
                            {
                                SendMessage(thisClient, "You cannot join the ongoing game. Please wait for the next game.");
                                continue;
                            }

                            clients[thisClient] = clientName;
                            if (!playerScores.ContainsKey(clientName))
                            {
                                playerScores[clientName] = 0;
                                SaveScores();
                            }
                            UpdateLeaderboardDisplay();
                            logs.AppendText($"{clientName} connected to the game.\n");

                            string playerScoreMessage = $"{clientName} has joined with {playerScores[clientName]} wins.";
                            SendMessageToAllExcept(playerScoreMessage, thisClient);
                            logs.AppendText(playerScoreMessage + "\n");

                            SendMessage(thisClient, "Welcome to Rock-Paper-Scissors Game!");

                            if (clients.Count == 4 && countdownCancellationTokenSource == null)
                            {
                                var playerNames = clients.Values.ToList();
                                game.SetActivePlayers(playerNames);
                                StartGame();
                            }
                        }
                        else
                        {
                            SendMessage(thisClient, "This name is already taken. Please select a different name.");
                        }
                    }

                    // If the user is connected and plays the game 
                    else if (clients.ContainsKey(thisClient))
                    {
                        game.AddPlayerChoice(clients[thisClient], incomingMessage);
                        logs.AppendText($"{clients[thisClient]} selected {incomingMessage}\n");

                        EvaluateGame();
                    }
                    else
                    {
                        logs.AppendText($"{clients[thisClient]}: {incomingMessage}\n");
                    }
                }
            }
            catch
            {
                if (!terminating)
                {
                    string disconnectionMessage = $"{(clientName ?? "An unknown client")} has disconnected.\n";
                    logs.AppendText(disconnectionMessage);
                    HandleClientDisconnect(thisClient);
                }
                clients.Remove(thisClient);
                thisClient.Close();
            }
        }

        private void HandlePlayerLeave(Socket client)
        {
            if (clients.ContainsKey(client))
            {
                string clientName = clients[client];
                clients.Remove(client);
                game.RemovePlayer(clientName);
                logs.AppendText($"{clientName} left the game.\n");
                SendMessageToAll($"{clientName} left the game.");

                if (clients.Count < 4 && countdownCancellationTokenSource != null)
                {
                    CancelCountdown();
                }

                if (game.GetActivePlayers().Count < 2)
                {
                    game.ResetChoices();
                }
                else
                {
                    logs.AppendText($"Remaining players: {string.Join(", ", game.GetActivePlayers())}\n");
                }

                EvaluateGame(); //Oyunu tekrar değerlendir
            }
        }

        private void HandleClientDisconnect(Socket client)
        {
            if (clients.ContainsKey(client))
            {
                string clientName = clients[client];
                clients.Remove(client);
                game.RemovePlayer(clientName);
                logs.AppendText($"{clientName} disconnected from the game.\n");
                SendMessageToAll($"{clientName} disconnected from the game.");

                if (clients.Count < 4 && countdownCancellationTokenSource != null)
                {
                    CancelCountdown();
                }

                if (game.GetActivePlayers().Count < 2)
                {
                    game.ResetChoices();
                }
                else
                {
                    logs.AppendText($"Remaining players: {string.Join(", ", game.GetActivePlayers())}\n");
                }

                EvaluateGame(); //Oyunu tekrar değerlendir
            }
        }

        private void StartGame()
        {
            if (clients.Count == 4)
            {
                var playerNames = clients.Values.ToList();
                game.SetActivePlayers(playerNames);

                logs.AppendText("Game will start in 5 seconds...\n");

                countdownCancellationTokenSource = new CancellationTokenSource();
                var token = countdownCancellationTokenSource.Token;

                Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 5; i > 0; i--)
                        {
                            /**
                            if (clients.Count < 4)
                            {
                                logs.AppendText("A player has disconnected. Filling spot from waiting queue...\n");
                                if (waitingClients.Count > 0)
                                {
                                    Socket nextClient = waitingClients.Dequeue();
                                    Thread receiveThread = new Thread(() => Receive(nextClient));
                                    receiveThread.Start();
                                }
                            }
                            **/
                            SendMessageToAll(i.ToString());
                            logs.AppendText(i.ToString() + "\n");
                            await Task.Delay(1000, token);
                        }

                        SendMessageToAll("Go");
                        logs.AppendText("Go\n");
                        EnableAllButtons();
                    }
                    catch (TaskCanceledException)
                    {
                        logs.AppendText("Countdown was cancelled, waiting for 4 players...\n");
                    }
                }, token);
            }
        }

        private void CancelCountdown()
        {
            if (countdownCancellationTokenSource != null)
            {
                countdownCancellationTokenSource.Cancel();
                countdownCancellationTokenSource = null;
            }
        }

        private void EvaluateGame()
        {
            string gameResult = game.DetermineWinner();
            if (gameResult.Contains("Waiting"))
            {
                //Bekleniyor
            }
            else
            {
                SendMessageToAll(gameResult);
                logs.AppendText(gameResult + "\n");

                if (gameResult.Contains("Winner"))
                {
                    game.ResetChoices();
                    game.ResetLeftPlayers(); //Oyunu terk eden oyuncuların listesini sıfırla
                    SaveScores();
                    UpdateLeaderboardDisplay();

                    //Oyunu bitirip yeni oyuna başlamadan önce sıradaki oyuncuları dahil et
                    IncludeWaitingClients();
                    if (clients.Count < 4)
                    {
                        logs.AppendText("Waiting for more players to start a new game...\n");
                    }
                    else
                    {
                        StartNewGameWithDelay();
                    }
                }
                else if (gameResult.Contains("Continuing"))
                {
                    var activePlayers = game.GetActivePlayers();
                    foreach (var client in clients)
                    {
                        if (!activePlayers.Contains(client.Value))
                        {
                            SendMessage(client.Key, "You have been eliminated.");
                            logs.AppendText($"{client.Value} has been eliminated.\n");
                        }
                        else
                        {
                            SendMessage(client.Key, "EnableButtons");
                        }
                    }
                    game.ResetChoices();
                }
                else if (gameResult.Contains("tie"))
                {
                    game.ResetChoices();
                }
            }
        }


        private void StartNewGameWithDelay()
        {
            SendMessageToAll("New game will start in 15 seconds...");
            logs.AppendText("New game will start in 15 seconds...\n");
            Task.Delay(15000).ContinueWith(t =>
            {
                SendMessageToAll("New game starting now!");
                logs.AppendText("New game starting now!\n");
                game.ResetChoices();

                var playerNames = clients.Values.ToList();
                game.SetActivePlayers(playerNames);

                EnableAllButtons();

                if (clients.Count == 4)
                {
                    StartGame();
                }
            });
        }

        private void IncludeWaitingClients()
        {
            while (clients.Count < 4 && waitingClients.Count > 0)
            {
                Socket nextClient = waitingClients.Dequeue();
                byte[] buffer = new byte[1024];
                int received = nextClient.Receive(buffer);
                string incomingMessage = Encoding.Default.GetString(buffer).Trim('\0');
                string clientName = null;

                if (incomingMessage.StartsWith("name:"))
                {
                    clientName = incomingMessage.Substring(5);
                }

                if (clientName != null)
                {
                    clients[nextClient] = clientName;
                    Thread receiveThread = new Thread(() => Receive(nextClient));
                    receiveThread.Start();
                }
            }
        }

        private void EnableAllButtons()
        {
            foreach (var client in clients)
            {
                SendMessage(client.Key, "EnableButtons");
            }
        }

        private void SendMessageToAll(string message)
        {
            foreach (Socket client in clients.Keys.ToList())
            {
                SendMessage(client, message);
            }
        }

        //Sends the number of games won by a newly joined player to other players
        private void SendMessageToAllExcept(string message, Socket exceptSocket)
        {
            foreach (var client in clients.Keys.ToList())
            {
                if (client != exceptSocket)
                {
                    SendMessage(client, message);
                }
            }
        }

        private void SendMessage(Socket client, string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);
            client.Send(buffer);
        }


        private void send_button_Click(object sender, EventArgs e)
        {
            string message = message_textBox.Text; 
            if (!string.IsNullOrEmpty(message))
            {
                SendMessageToAll("Server:" + message+ "\n");
                logs.AppendText("Server: " + message + "\n"); 
                message_textBox.Clear(); 
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveScores(); 
            terminating = true;
            serverSocket.Close();
            Environment.Exit(0);
        }
    }
}
