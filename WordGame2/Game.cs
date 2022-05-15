using System.Text.Json;
using System.Text.RegularExpressions;
using WordGame2.Languages;
using GameTimer = System.Timers;

namespace WordGame2
{
    internal class Game
    {
        private string _composedWord;
        private string _primaryWord;
        private Dictionary<char, int> _primaryWordLetters;
        private Stack<string> _usedWords;
        private readonly GameTimer::Timer _timer;
        private readonly List<Player> _players;
        private int _playerNumber;
        private readonly Command _commandManager;

        private const int CLOSE_APPLICATION_WITHOUT_ERRORS = 0;
        private const double ONE_MINUTE = 60000;
        private const string FILE_NAME = "Players.json";
        private const int FIRTS_PLAYER_ID = 0;
        private const int SECOND_PLAYER_ID = 1;

        public Game()
        {
            _composedWord = string.Empty;
            _primaryWord = string.Empty;
            _usedWords = new Stack<string>();
            _primaryWordLetters = new Dictionary<char, int>();
            _timer = new GameTimer::Timer();
            _players = new List<Player>();
            _playerNumber = 0;
            _commandManager = new Command(FILE_NAME, _usedWords, _players);
        }

        public void EnterPlayerNames()
        {
            Console.Clear();

            Console.WriteLine(Messages.FirstPlayerName);
            string firstPlayerName = Console.ReadLine() ?? "Player1";
            _players.Add(new Player(FIRTS_PLAYER_ID, firstPlayerName));

            Console.WriteLine("\n" + Messages.SecondPlayerName);
            string secondPlayerName = Console.ReadLine() ?? "Player2";
            _players.Add(new Player(SECOND_PLAYER_ID, secondPlayerName));
        }

        private void SaveGameResult()
        {
            string jsonString;
            int value = _playerNumber % 2 == 0 ? _players[SECOND_PLAYER_ID].WinCount++ : _players[FIRTS_PLAYER_ID].WinCount++;

            if (File.Exists(FILE_NAME))
            {
                string fileText = File.ReadAllText(FILE_NAME);
                Player[] allPlayers = JsonSerializer.Deserialize<Player[]>(fileText);
                string[] currentPlayerNames = _players.Select(x => x.Name).ToArray();
                Player[] matchedPlayers = allPlayers.Where(x => currentPlayerNames.Contains(x.Name)).ToArray();
                Player[] otherPlayers = allPlayers.Where(x => !currentPlayerNames.Contains(x.Name)).ToArray();
                Player[] newPlayers = _players.Where(x => !allPlayers.Select(x => x.Name).Contains(x.Name)).ToArray();

                foreach (Player a in matchedPlayers)
                {
                    foreach (Player b in _players)
                    {
                        if (a.Name == b.Name)
                        {
                            a.WinCount += b.WinCount;
                        }
                    }
                }

                jsonString = JsonSerializer.Serialize(matchedPlayers.Concat(otherPlayers).Concat(newPlayers).ToArray());
            }
            else
            {
                jsonString = JsonSerializer.Serialize(_players);
            }

            File.WriteAllText(FILE_NAME, jsonString);
        }

        public void Start()
        {
            Console.Clear();
            Console.WriteLine(Messages.PrimaryWordInput);
            _primaryWord = (Console.ReadLine() ?? "").ToLower();

            if (!CheckPrimaryWord())
            {
                Restart();
            }

            Console.Clear();
            Console.WriteLine(Messages.PrimaryWordOutput + _primaryWord);

            _usedWords.Push(_primaryWord);
            _primaryWordLetters = CreateDictionary(_primaryWord);

            while (true)
            {
                Console.WriteLine(_playerNumber % 2 == 0 
                    ? "\n" + Messages.PlayerTurn + _players[FIRTS_PLAYER_ID].Name + ":"
                    : "\n" + Messages.PlayerTurn + _players[SECOND_PLAYER_ID].Name + ":");

                AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

                StartTimer();
                _composedWord = (Console.ReadLine() ?? "").ToLower();
                _timer.Stop();

                while(_composedWord.StartsWith('/'))
                {
                    _commandManager.Execute(_composedWord);
                    Console.Clear();
                    Console.WriteLine(Messages.PrimaryWordOutput + _primaryWord);
                    Console.WriteLine(_playerNumber % 2 == 0
                        ? "\n" + Messages.PlayerTurn + _players[FIRTS_PLAYER_ID].Name + ":"
                        : "\n" + Messages.PlayerTurn + _players[SECOND_PLAYER_ID].Name + ":");
                    _composedWord = (Console.ReadLine() ?? "").ToLower();
                }

                if (!CheckComposedWord())
                {
                    SaveGameResult();
                    break;
                }

                _usedWords.Push(_composedWord);
                _playerNumber++;
            }

            _timer.Elapsed -= TimerElapsed;
            Restart();
        }

        private bool CheckPrimaryWord()
        {
            if ((_primaryWord?.Length is < 8 or > 30) || _primaryWord == string.Empty)
            {
                Console.WriteLine(Messages.WordLengthError + "\n" + Messages.KeyToContinue);
                return false;
            }

            if (!Regex.IsMatch(_primaryWord ?? "", Messages.LettersRegex))
            {
                Console.WriteLine(Messages.WordCharactersError + "\n" + Messages.KeyToContinue);
                return false;
            }

            return true;
        }

        private bool CheckComposedWord()
        {
            if (_composedWord == string.Empty || !Regex.IsMatch(_composedWord, Messages.LettersRegex))
            {
                Console.WriteLine(Messages.IncorectCompose + "\n" + Messages.KeyToContinue);
                return false;
            }

            if (!MatchLetters())
            {
                Console.WriteLine(Messages.IncorectCompose + "\n" + Messages.KeyToContinue);
                return false;
            }

            if (_usedWords.Contains(_composedWord))
            {
                Console.WriteLine(Messages.WordIsUsed + "\n" + Messages.KeyToContinue);
                return false;
            }
            
            return true;
        }

        private Dictionary<char, int> CreateDictionary(string word)
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

        private bool MatchLetters()
        {
            Dictionary<char, int> composedWordLetters = CreateDictionary(_composedWord);

            foreach (char key in composedWordLetters.Keys)
            {
                if (!_primaryWordLetters.ContainsKey(key) || _primaryWordLetters[key] < composedWordLetters[key])
                {
                    return false;
                }
            }

            return true;
        }

        private void Restart()
        {
            Console.ReadKey();
            _usedWords.Clear();
            string[] menuElements = { Messages.Yes, Messages.No };
            Menu confirmMenu = new Menu(menuElements, Messages.Restart);
            string element = confirmMenu.SelectMenuElement();

            if (element == Messages.Yes)
            {
                Start();
            }

            Environment.Exit(CLOSE_APPLICATION_WITHOUT_ERRORS);
        }

        private void StartTimer()
        {
            _timer.Interval = ONE_MINUTE;
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, GameTimer::ElapsedEventArgs e)
        {
            Console.WriteLine(Messages.TimerElapsed);
            Environment.Exit(CLOSE_APPLICATION_WITHOUT_ERRORS);
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            SaveGameResult();
        }
    }
}
