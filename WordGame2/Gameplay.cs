using System.Text.RegularExpressions;
using WordGame2.Languages;
using System.IO;

namespace WordGame2
{
    internal class Gameplay
    {
        private string _composedWord;
        private string _primaryWord;
        private Dictionary<char, int> _primaryWordLetters;
        private Stack<string> _usedWords;

        private const int YES = 0;
        private const int CLOSE_APPLICATION_WITHOUT_ERRORS = 0;
        
        public Gameplay()
        {
            _composedWord = string.Empty;
            _primaryWord = string.Empty;
            _usedWords = new Stack<string>();
            _primaryWordLetters = new Dictionary<char, int>();
        }

        public bool CheckPrimaryWord()
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

        public void Start()
        {
            Console.Clear();
            Console.WriteLine(Messages.PrimaryWordInput);
            _primaryWord = (Console.ReadLine() ?? "").ToLower();

            if (!CheckPrimaryWord())
            {
                return;
            }

            Console.Clear();
            Console.WriteLine(Messages.PrimaryWordOutput + _primaryWord);

            _usedWords.Push(_primaryWord);
            _primaryWordLetters = CreateDictionary(_primaryWord);
            int number = 0;

            while (true)
            {
                Console.WriteLine(number % 2 == 0 ? "\n" + Messages.FirstPlayerTurn : "\n" + Messages.SecondPlayerTurn);
                _composedWord = (Console.ReadLine() ?? "").ToLower();

                if (!CheckComposedWord())
                {
                    break;
                }

                _usedWords.Push(_composedWord);
                number++;
            }

            Restart();
        }

        private void Restart()
        {
            Console.ReadKey();
            _usedWords.Clear();
            string[] menuElements = { Messages.Yes, Messages.No };
            Menu confirmMenu = new Menu(menuElements, Messages.Restart);
            int index = confirmMenu.SelectMenuElement();
            
            if (index == YES)
            {
                Start();
            }

            Environment.Exit(CLOSE_APPLICATION_WITHOUT_ERRORS);
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
    }
}
