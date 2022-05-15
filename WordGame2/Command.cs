using System.Text.Json;
using WordGame2.Languages;

namespace WordGame2
{
    internal class Command
    {
        private delegate void Report();
        private readonly Dictionary<string, Report> _reports;
        private readonly string _fileName;
        private readonly Stack<string> _usedWords;
        private readonly List<Player> _players;

        public Command(string fileName, Stack<string> usedWords, List<Player> players)
        {
            _reports = new Dictionary<string, Report>()
            {
                { "/show-words", new Report(ShowWords) },
                { "/score", new Report(Score) },
                { "/total-score", new Report(TotalScore) },
            };
            _fileName = fileName;
            _usedWords = usedWords;
            _players = players;
        }

        private void ShowWords()
        {
            Console.WriteLine(Messages.UsedWords);

            foreach (var word in _usedWords)
            {
                Console.WriteLine(word);
            }
        }

        private void Score()
        {
            string fileText = File.ReadAllText(_fileName);
            Player[] allPlayers = JsonSerializer.Deserialize<Player[]>(fileText);
            string[] currentPlayerNames = _players.Select(x => x.Name).ToArray();
            Player[] matchedPlayers = allPlayers.Where(x => currentPlayerNames.Contains(x.Name)).ToArray();

            if (matchedPlayers.Length != 0)
            {
                Console.WriteLine(Messages.Score);

                foreach (Player player in matchedPlayers)
                {
                    Console.WriteLine(Messages.PlayerName + player.Name + Messages.WinsNumber + player.WinCount);
                }
            }
            else
            {
                Console.WriteLine(Messages.NoScore);
            }
        }

        private void TotalScore()
        {
            if (File.Exists(_fileName) && new FileInfo(_fileName).Length != 0)
            {
                string fileText = File.ReadAllText(_fileName);
                Player[] allPlayers = JsonSerializer.Deserialize<Player[]>(fileText);

                Console.WriteLine(Messages.TotalScore);

                foreach (Player player in allPlayers)
                {
                    Console.WriteLine(Messages.PlayerName + player.Name + Messages.WinsNumber + player.WinCount);
                }
            }
            else
            {
                Console.WriteLine(Messages.FileEmpty);
            }
        }

        public void ExecuteCommand(string command)
        {
            if (_reports.ContainsKey(command))
            {
                _reports[command].Invoke();
            }
            else
            {
                Console.WriteLine("LOX");
            }

        }
    }
}
