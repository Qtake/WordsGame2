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

        private delegate void Report();
        private readonly Dictionary<string, Report> _reports;

        private const int CLOSE_APPLICATION_WITHOUT_ERRORS = 0;
        private const double ONE_MINUTE = 60000;
        private const string USED_WORDS_FILE = @"C:\Users\grant\source\repos\Qtake\WordsGame2\UsedWords";

        public Game()
        {
            _composedWord = string.Empty;
            _primaryWord = string.Empty;
            _usedWords = new Stack<string>();
            _primaryWordLetters = new Dictionary<char, int>();
            _timer = new GameTimer::Timer();
            _players = new List<Player>();

            _reports = new Dictionary<string, Report>()
            {
                { "/show-words", new Report(ShowWords) },
                { "/score", new Report(Score) },
                { "/total-score", new Report(TotalScore) },
            };
        }

        private void ShowWords()
        {
            Console.WriteLine("ShowWords");
        }

        private void Score()
        {
            Console.WriteLine("Score");
        }

        private void TotalScore()
        {
            Console.WriteLine("TotalScore");
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

        public void EnterPlayerNames()
        {
            Console.Clear();

            Console.WriteLine(Messages.FirstPlayerName);
            string firstPlayerName = Console.ReadLine() ?? "Player1";
            _players.Add(new Player(firstPlayerName));

            Console.WriteLine("\n" + Messages.SecondPlayerName);
            string secondPlayerName = Console.ReadLine() ?? "Player2";
            _players.Add(new Player(secondPlayerName));
        }

        public void Start()
        {
            Console.WriteLine();

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
            int number = 0;

            while (true)
            {
                Console.WriteLine(number % 2 == 0 ? "\n" + Messages.FirstPlayerTurn : "\n" + Messages.SecondPlayerTurn);
                StartTimer();
                _composedWord = (Console.ReadLine() ?? "").ToLower();
                _timer.Stop();

                if (!CheckComposedWord())
                {
                    break;
                }

                _usedWords.Push(_composedWord);
                number++;
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

            if (!Regex.IsMatch(_primaryWord, Messages.LettersRegex))
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
    }
}
