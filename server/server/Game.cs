using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class Game
    {
        private Dictionary<string, string> playerChoices;
        private Dictionary<string, int> playerScores;
        private List<string> activePlayers;
        private List<string> leftPlayers; // Oyunu terk eden oyuncuları takip eden list

        public Game(Dictionary<string, int> scores)
        {
            playerChoices = new Dictionary<string, string>();
            playerScores = scores;
            activePlayers = new List<string>();
            leftPlayers = new List<string>();
        }

        public void AddPlayerChoice(string playerName, string choice)
        {
            if (leftPlayers.Contains(playerName)) // Oyuncu ayrıldıysa seçim yapamaz
            {
                return;
            }

            if (playerChoices.ContainsKey(playerName))
            {
                playerChoices[playerName] = choice;
            }
            else
            {
                playerChoices.Add(playerName, choice);
            }
        }

        public void SetActivePlayers(List<string> players)
        {
            activePlayers = new List<string>(players);
        }

        public string DetermineWinner()
        {
            if (activePlayers.Count == 1)
            {
                string winner = activePlayers[0];
                playerScores[winner]++;
                return $"Game over! Winner: {winner}.\nPlayer choices:\n{string.Join("\n", playerChoices.Select(pc => $"{pc.Key}: {pc.Value}"))}\n";
            }

            if (playerChoices.Count < activePlayers.Count)
            {
                return "Waiting for all active players to make their choice.";
            }

            var choices = new List<string>();
            foreach (var player in activePlayers)
            {
                if (playerChoices.ContainsKey(player))
                {
                    choices.Add(playerChoices[player]);
                }
                else
                {
                    choices.Add("none");
                }
            }

            var distinctChoices = new HashSet<string>(choices);

            var playerChoicesMessage = new StringBuilder("Player choices:\n");
            foreach (var choice in playerChoices)
            {
                playerChoicesMessage.AppendLine($"{choice.Key}: {choice.Value}");
            }

            // All the Same Choice
            if (distinctChoices.Count == 1)
            {
                return $"It's a tie! All active players chose the same.\n{playerChoicesMessage}\nContinuing the game with remaining players: {string.Join(", ", activePlayers)}";
            }

            // Three Choices Alike
            if (choices.Count(c => c == "rock") == 3 && choices.Count(c => c == "paper") == 1)
            {
                var winner = activePlayers.First(p => playerChoices[p] == "paper");
                playerScores[winner]++;
                return $"Game over! Winner: {winner}.\n{playerChoicesMessage}";
            }
            if (choices.Count(c => c == "paper") == 3 && choices.Count(c => c == "scissors") == 1)
            {
                var winner = activePlayers.First(p => playerChoices[p] == "scissors");
                playerScores[winner]++;
                return $"Game over! Winner: {winner}.\n{playerChoicesMessage}";
            }
            if (choices.Count(c => c == "scissors") == 3 && choices.Count(c => c == "rock") == 1)
            {
                var winner = activePlayers.First(p => playerChoices[p] == "rock");
                playerScores[winner]++;
                return $"Game over! Winner: {winner}.\n{playerChoicesMessage}";
            }

            // Two and Two
            if (choices.Count(c => c == "rock") == 2 && choices.Count(c => c == "paper") == 2)
            {
                activePlayers = activePlayers.Where(p => playerChoices[p] == "paper").ToList();
                ResetChoices();
                return $"{playerChoicesMessage}\nContinuing the game with remaining players: {string.Join(", ", activePlayers)}";
            }
            if (choices.Count(c => c == "rock") == 2 && choices.Count(c => c == "scissors") == 2)
            {
                activePlayers = activePlayers.Where(p => playerChoices[p] == "rock").ToList();
                ResetChoices();
                return $"{playerChoicesMessage}\nContinuing the game with remaining players: {string.Join(", ", activePlayers)}";
            }
            if (choices.Count(c => c == "paper") == 2 && choices.Count(c => c == "scissors") == 2)
            {
                activePlayers = activePlayers.Where(p => playerChoices[p] == "scissors").ToList();
                ResetChoices();
                return $"{playerChoicesMessage}\nContinuing the game with remaining players: {string.Join(", ", activePlayers)}";
            }

            // Two of One Choice, One of Each of the Others
            if (choices.Count(c => c == "rock") == 2 && distinctChoices.Contains("paper") && distinctChoices.Contains("scissors"))
            {
                activePlayers = activePlayers.Where(p => playerChoices[p] == "rock").ToList();
                ResetChoices();
                return $"{playerChoicesMessage}\nContinuing the game with remaining players: {string.Join(", ", activePlayers)}";
            }
            if (choices.Count(c => c == "paper") == 2 && distinctChoices.Contains("rock") && distinctChoices.Contains("scissors"))
            {
                activePlayers = activePlayers.Where(p => playerChoices[p] == "paper").ToList();
                ResetChoices();
                return $"{playerChoicesMessage}\nContinuing the game with remaining players: {string.Join(", ", activePlayers)}";
            }
            if (choices.Count(c => c == "scissors") == 2 && distinctChoices.Contains("rock") && distinctChoices.Contains("paper"))
            {
                activePlayers = activePlayers.Where(p => playerChoices[p] == "scissors").ToList();
                ResetChoices();
                return $"{playerChoicesMessage}\nContinuing the game with remaining players: {string.Join(", ", activePlayers)}";
            }

            // İki oyuncu kaldığında durumunu işle
            if (activePlayers.Count == 2)
            {
                string player1 = activePlayers[0];
                string player2 = activePlayers[1];
                string choice1 = playerChoices[player1];
                string choice2 = playerChoices[player2];

                if (choice1 == choice2)
                {
                    return $"It's a tie! Both players chose {choice1}.\n{playerChoicesMessage}\nContinuing the game with remaining players: {string.Join(", ", activePlayers)}";
                }
                else
                {
                    string winningChoice = GetWinningChoice(new HashSet<string> { choice1, choice2 });
                    string winner = playerChoices[player1] == winningChoice ? player1 : player2;
                    playerScores[winner]++;
                    return $"Game over! Winner: {winner}.\n{playerChoicesMessage}";
                }
            }

            return $"Unexpected state.\n{playerChoicesMessage}";
        }

        public Dictionary<string, string> GetPlayerChoices()
        {
            return new Dictionary<string, string>(playerChoices);
        }

        private string GetWinningChoice(HashSet<string> choices)
        {
            if (choices.Contains("rock") && choices.Contains("scissors"))
            {
                return "rock";
            }
            else if (choices.Contains("scissors") && choices.Contains("paper"))
            {
                return "scissors";
            }
            else // rock ve paper
            {
                return "paper";
            }
        }

        private List<string> GetWinners(string winningChoice)
        {
            var winners = new List<string>();
            foreach (var player in playerChoices)
            {
                if (player.Value == winningChoice && activePlayers.Contains(player.Key))
                {
                    winners.Add(player.Key);
                }
            }
            return winners;
        }

        public void ResetChoices()
        {
            playerChoices.Clear();
        }

        public List<string> GetActivePlayers()
        {
            return activePlayers;
        }

        public void RemovePlayer(string playerName)
        {
            if (activePlayers.Contains(playerName))
            {
                activePlayers.Remove(playerName);
            }

            if (playerChoices.ContainsKey(playerName))
            {
                playerChoices.Remove(playerName);
            }

            if (!leftPlayers.Contains(playerName))
            {
                leftPlayers.Add(playerName);
            }
        }

        public void ResetLeftPlayers()
        {
            leftPlayers.Clear();
        }

        public List<string> GetLeftPlayers()
        {
            return leftPlayers;
        }
    }
}
