using System.Text.RegularExpressions;
using WordGame2.Command;
using WordGame2.DataManagers;
using WordGame2.Messages;
using GameTimer = System.Timers;

namespace WordGame2
{
    internal class Game
    {
        public Dictionary<string, Action> Commands { get; private set; }

        private string _primaryWord;
        private Dictionary<char, int> _primaryWordLetters;
        private readonly Queue<Player> _players;
        private readonly List<string> _usedWords;
        private readonly GameTimer::Timer _timer;
        private readonly IDataManager _dataManager;

        private const int MinWordLength = 8;
        private const int MaxWordLength = 30;
        private const int MinPlayerCount = 2;
        private const int InitialTimerValue = 10000;

        public Game()
        {
            Commands = new Dictionary<string, Action>()
            {
                { "/show-words", ShowUsedWords },
                { "/score", ShowScore },
                { "/total-score", ShowTotalScore }
            };

            _primaryWord = string.Empty;
            _primaryWordLetters = new Dictionary<char, int>();
            _players = new Queue<Player>();
            _usedWords = new List<string>();
            _timer = new GameTimer::Timer { Interval = InitialTimerValue };
            _timer.Elapsed += TimerElapsed;
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
            _primaryWordLetters = GroupingByLetters(_primaryWord);
            ShowPrimaryWord();
            

            while (_players.Count > 1)
            {
                ShowCurrentPlayerName(_players.Peek().Name);
                _timer.Start();
                string word = InputComposedWord();
                _timer.Stop();

                while (word.StartsWith('/'))
                {
                    CommandActivator activator = new CommandActivator();
                    activator.SetCommand(new GameCommand(this, word));
                    activator.ActivateCommand();
                    WaitAnyKey();
                    ShowPrimaryWord();
                    ShowCurrentPlayerName(_players.Peek().Name);
                    word = InputComposedWord();
                }

                Player currentPlayer = _players.Dequeue();

                if (CheckComposedWord(word))
                {
                    _players.Enqueue(currentPlayer);
                    _usedWords.Add(word);
                }
            }

            _dataManager.WriteData(_players.Peek().Name);
            _timer.Elapsed -= TimerElapsed;
        }

        private bool InputPrimaryWord()
        {
            Console.Clear();
            Console.WriteLine(Message.PrimaryWordInput);
            _primaryWord = (Console.ReadLine() ?? "").ToLower();

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

        private static Dictionary<char, int> GroupingByLetters(string word)
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
            Dictionary<char, int> composedWordLetters = GroupingByLetters(composedWord);

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

        private static void ShowCurrentPlayerName(string playerName)
        {
            Console.WriteLine("\n" + Message.PlayerTurn + $"{playerName}:");
        }

        private static string InputComposedWord()
        {
            return (Console.ReadLine() ?? "").ToLower();
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

        private static void ShowStatistics(List<Player> players)
        {
            foreach (Player player in players)
            {
                Console.WriteLine(Message.PlayerName + player.Name + Message.WinsNumber + player.WinCount);
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

            Console.WriteLine(Message.Score);
            ShowStatistics(matchedPlayers);
        }

        private void ShowTotalScore()
        {
            List<Player>? previousPlayers = _dataManager.ReadData();

            if (previousPlayers == null)
            {
                Console.WriteLine(Message.FileEmpty);
                return;
            }

            Console.WriteLine(Message.TotalScore);
            ShowStatistics(previousPlayers);
        }

        private void TimerElapsed(object sender, GameTimer::ElapsedEventArgs e)
        {
            Console.WriteLine(Message.TimerElapsed);
            _timer.Stop();
        }
    }
}
