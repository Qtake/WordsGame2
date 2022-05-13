using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGame2
{
    internal class Game
    {
        public string ComposedWord { get; set; }
        private readonly string _primaryWord;
        private Stack<string> _usedWords;
        
        public Game(string primaryWord)
        {
            _primaryWord = primaryWord;
            ComposedWord = string.Empty;
            _usedWords = new Stack<string>();
            _usedWords.Push(primaryWord);
        }

        public static bool CheckPrimaryWord(string primaryWord)
        {
            if ((primaryWord?.Length is < 8 or > 30) || primaryWord == string.Empty)
            {
                Console.WriteLine(Messages.WordLengthError);
                return false;
            }

            return CheckCharacters(primaryWord);
        }

        public static bool CheckComposedWord(string composedWord)
        {
            if (composedWord == string.Empty)
            {
                Console.WriteLine(Messages.Lose);
                return false;
            }

            return CheckCharacters(composedWord);
        }

        public static bool CheckCharacters(string word)
        {
            if (!Regex.IsMatch(word, Messages.LettersRegex))
            {
                Console.WriteLine(Messages.WordCharactersError);
                return false;
            }

            return true;
        }

        private static IEnumerable<(char Letter, int Count)> CreateLettersGroup(string word)
        {
            return word
                .GroupBy(c => c)
                .Select(g => (g.Key, g.Count()));
        }

        public bool MatchLetters()
        {
            var primaryWordLetters = CreateLettersGroup(_primaryWord);
            var composedWordLetters = CreateLettersGroup(ComposedWord);

            foreach (var c1 in primaryWordLetters)
            {
                foreach (var c2 in composedWordLetters)
                {
                    if ((c1.Letter == c2.Letter && c1.Count != c2.Count))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
