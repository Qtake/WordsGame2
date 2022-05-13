using System.Text.RegularExpressions;
using WordGame2.Languages;

namespace WordGame2
{
    internal class Gameplay
    {
        public string ComposedWord { get; set; }
        private readonly string _primaryWord;
        private Dictionary<char, int> _primaryWordLetters;
        private Stack<string> _usedWords;

        private const int YES = 0;
        private const int CLOSE_APPLICATION_WITHOUT_ERRORS = 0;
        
        public Gameplay(string primaryWord)
        {
            ComposedWord = string.Empty;
            _primaryWord = primaryWord;
            _usedWords = new Stack<string>();
            _usedWords.Push(primaryWord);
            _primaryWordLetters = new Dictionary<char, int>();
        }

        public void Start()
        {
            if ((_primaryWord?.Length is < 8 or > 30) || string.IsNullOrEmpty(_primaryWord))
            {
                Console.WriteLine(Messages.WordLengthError + "\n" + "Нажмите любую клавишу для продолжения");
                Console.Read();
                return;
            }

            if (!Regex.IsMatch(_primaryWord, Messages.LettersRegex))
            {
                Console.WriteLine(Messages.WordCharactersError + "\n" + "Нажмите любую клавишу для продолжения");
                Console.Read();
                return;
            }

            _primaryWordLetters = CreateDictionary(_primaryWord);
            int number = 0;

            while (true)
            {
                Console.WriteLine(number % 2 == 0 ? "\n" + Messages.FirstPlayerTurn : "\n" + Messages.SecondPlayerTurn);

                ComposedWord = (Console.ReadLine() ?? "").ToLower();

                if (ComposedWord == string.Empty || !Regex.IsMatch(ComposedWord, Messages.LettersRegex))
                {
                    Console.WriteLine(Messages.IncorectCompose + ' ' + Messages.Lose + "\n" + "Нажмите любую клавишу для продолжения");
                    Console.Read();
                    return;
                }

                if (!MatchLetters())
                {
                    Console.WriteLine(Messages.IncorectCompose + ' ' + Messages.Lose + "\n" + "Нажмите любую клавишу для продолжения");
                    Console.Read();
                    return;
                }

                if (_usedWords.Contains(ComposedWord))
                {
                    Console.WriteLine(Messages.WordIsUsed);
                    Console.Read();
                    return;
                }

                _usedWords.Push(ComposedWord);
                number++;
            }
        }

        public void Restart()
        {
            string[] menuElements = { "Yes", "No" };
            Menu confirmMenu = new Menu(menuElements, "Restart?");
            int index = confirmMenu.SelectMenuElement();
            
            if (index == YES)
            {
                Start();
            }
            
            Environment.Exit(CLOSE_APPLICATION_WITHOUT_ERRORS);
        }

        /*


        public static bool CheckComposedWord(string composedWord)
        {
            if (composedWord == string.Empty)
            {
                Console.WriteLine(Messages.Lose);
                return false;
            }

            return CheckCharacters(composedWord);
        }
        */

        private static IEnumerable<(char Letter, int Count)> CreateLettersGroup(string word)
        {
            return word
                .GroupBy(c => c)
                .Select(g => (g.Key, g.Count()));
        }

        private Dictionary<char, int> CreateDictionary(string word)
        {
            Dictionary<char, int> letters = new Dictionary<char, int>();

            foreach (char key in word)
            {
                if (letters.ContainsKey(key))
                {
                    letters[key]++;
                }
                else
                {
                    letters.Add(key, 1);
                }
            }

            return letters;
        }

        private bool MatchLetters()
        {
            //var primaryWordLetters = CreateLettersGroup(_primaryWord);
            //var composedWordLetters = CreateLettersGroup(ComposedWord);

            Dictionary<char, int> composedWordLetters = CreateDictionary(ComposedWord);

            foreach (char key in _primaryWordLetters.Keys)
            {
                if (composedWordLetters.ContainsKey(key))
                {
                    if (_primaryWordLetters[key] != composedWordLetters[key])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
