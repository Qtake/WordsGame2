using System.Text.RegularExpressions;
using WordGame2.Command;
using WordGame2.DataManagers;
using WordGame2.Messages;

namespace WordGame2
{
    internal class Game
    {
        private readonly Dictionary<string, Action> _commands;
        private string _primaryWord;
        private Dictionary<char, int> _primaryWordLetters;
        private readonly Queue<Player> _players;
        private readonly List<string> _usedWords;
        private readonly IDataManager _dataManager;

        private const int MinWordLength = 8;
        private const int MaxWordLength = 30;
        private const int MinPlayerCount = 2;

        public Game()
        {
            _commands = new Dictionary<string, Action>()
            {
                { "/show-words", ShowUsedWords },
                { "/score", ShowScore },
                { "/total-score", ShowTotalScore }
            };

            _primaryWord = string.Empty;
            _primaryWordLetters = new Dictionary<char, int>();
            _players = new Queue<Player>();
            _usedWords = new List<string>();
            _dataManager = new FileManager("Players.json");
        }

        public bool EnterPlayerNames()
        {
            Console.Clear();
            Console.WriteLine(Message.InputPlayerCount);
            bool isConverted = int.TryParse(Console.ReadLine(), out int playerCount);
            string playerName;

            if (!isConverted || playerCount < MinPlayerCount)
            {
                Console.WriteLine(Message.IncorrectPlayerCount);
                return false;
            }

            for (int i = 1; i < playerCount + 1; i++)
            {
                Console.WriteLine("\n" + Message.InputPlayerName + $" {i}:");
                playerName = Console.ReadLine() ?? $"Player {i}";

                if (_players.Select(x => x.Name).Contains(playerName))
                {
                    Console.WriteLine(Message.RepeatedName);
                    return false;
                }

                _players.Enqueue(new Player(playerName));
            }

            return true;
        }

        public void Start()
        {
            if (!InputPrimaryWord())
            {
                WaitAnyKey();
                return;
            }

            _usedWords.Add(_primaryWord);
            _primaryWordLetters = GroupByLetters(_primaryWord);
            ShowPrimaryWord();
            GameCommand gameCommand = new GameCommand();
            
            while (_players.Count > 1)
            {
                ShowCurrentPlayerName(_players.Peek().Name);
                string word = InputWord();

                while (word.StartsWith('/'))
                {
                    if (_commands.ContainsKey(word))
                    {
                        gameCommand.SetCommand(_commands[word]);
                        gameCommand.Execute();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(Message.UnknownCommand);
                    }

                    WaitAnyKey();
                    ShowPrimaryWord();
                    ShowCurrentPlayerName(_players.Peek().Name);
                    word = InputWord();
                }

                Player currentPlayer = _players.Dequeue();

                if (CheckComposedWord(word))
                {
                    _players.Enqueue(currentPlayer);
                    _usedWords.Add(word);
                }
            }

            string winner = _players.Peek().Name;
            Console.WriteLine("\n" + Message.Winner + winner);
            _dataManager.WriteData(CreateGameResult(winner));
        }

        private bool InputPrimaryWord()
        {
            Console.Clear();
            Console.WriteLine(Message.PrimaryWordInput);
            _primaryWord = InputWord();

            if ((_primaryWord?.Length is < MinWordLength or > MaxWordLength) || _primaryWord == string.Empty)
            {
                Console.WriteLine(Message.WordLengthError);
                return false;
            }

            if (!Regex.IsMatch(_primaryWord ?? "", Message.LettersRegex))
            {
                Console.WriteLine(Message.WordCharactersError);
                return false;
            }

            return true;
        }

        private bool CheckComposedWord(string composedWord)
        {
            if (composedWord == string.Empty || !Regex.IsMatch(composedWord, Message.LettersRegex))
            {
                Console.WriteLine(Message.IncorectCompose);
                return false;
            }

            if (!MatchLetters(composedWord))
            {
                Console.WriteLine(Message.IncorectCompose);
                return false;
            }

            if (_usedWords.Contains(composedWord))
            {
                Console.WriteLine(Message.WordIsUsed);
                return false;
            }
            
            return true;
        }

        private static Dictionary<char, int> GroupByLetters(string word)
        {
            Dictionary<char, int> letters = new Dictionary<char, int>();

            foreach (char key in word)
            {
                if (!letters.ContainsKey(key))
                {
                    letters.Add(key, 1);
                }
                else
                {
                    letters[key]++;
                }
            }

            return letters;
        }

        private bool MatchLetters(string composedWord)
        {
            Dictionary<char, int> composedWordLetters = GroupByLetters(composedWord);

            foreach (char key in composedWordLetters.Keys)
            {
                if (!_primaryWordLetters.ContainsKey(key) || _primaryWordLetters[key] < composedWordLetters[key])
                {
                    return false;
                }
            }

            return true;
        }

        private void ShowPrimaryWord()
        {
            Console.Clear();
            Console.WriteLine(Message.PrimaryWordOutput + _primaryWord);
        }

        private static void WaitAnyKey()
        {
            Console.WriteLine("\n" + Message.KeyToContinue);
            Console.ReadKey();
        }

        private static void ShowCurrentPlayerName(string playerName) => Console.WriteLine("\n" + Message.PlayerTurn + playerName);

        private static string InputWord() => (Console.ReadLine() ?? "").ToLower();

        private static void ShowStatistics(List<Player> players, string message)
        {
            Console.Clear();
            Console.WriteLine(message);

            foreach (Player player in players)
            {
                Console.WriteLine(Message.PlayerName + player.Name + Message.WinsNumber + player.WinCount);
            }
        }

        private void ShowUsedWords()
        {
            Console.Clear();
            Console.WriteLine(Message.UsedWords);

            foreach (string word in _usedWords)
            {
                Console.WriteLine(word);
            }
        }

        private void ShowScore()
        {
            List<Player>? previousPlayers = _dataManager.ReadData();

            if (previousPlayers == null)
            {
                Console.WriteLine(Message.NoScore);
                return;
            }

            List<Player> matchedPlayers = previousPlayers
                .Where(x => _players.Select(x => x.Name)
                .Contains(x.Name))
                .ToList();

            if (matchedPlayers.Count == 0)
            {
                Console.WriteLine(Message.NoScore);
                return;
            }

            ShowStatistics(matchedPlayers, Message.Score);
        }

        private void ShowTotalScore()
        {
            List<Player>? previousPlayers = _dataManager.ReadData();

            if (previousPlayers == null)
            {
                Console.WriteLine(Message.FileEmpty);
                return;
            }

            ShowStatistics(previousPlayers, Message.TotalScore);
        }

        private List<Player> CreateGameResult(string winner)
        {
            List<Player>? previousPlayers = _dataManager.ReadData() ?? new List<Player>();

            if (previousPlayers.Select(x => x.Name).Contains(winner))
            {
                previousPlayers.First(x => x.Name == winner).WinCount++;
                return previousPlayers;
            }

            previousPlayers.Add(new Player(winner) { WinCount = 1 });
            return previousPlayers;
        }
    }
}
