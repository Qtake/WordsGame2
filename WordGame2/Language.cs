using System.Globalization;
using WordsGame.Languages;

namespace WordGame2
{
    internal class Language
    {
        private readonly Dictionary<string, string> _languages = new Dictionary<string, string>()
        {
            { "English", "en-US" },
            { "Русский", "ru-RU" }
        };

        private string _key;
        private int _index;

        public Language()
        {
            _key = string.Empty;
            _index = 0;
        }

        public void CreateMenu()
        {
            string[] menuElements = _languages.Keys.ToArray();
            Console.CursorVisible = false;

            while (true)
            {
                PrintMenuElements(menuElements);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        _index--;
                        break;
                    case ConsoleKey.DownArrow:
                        _index++;
                        break;
                    case ConsoleKey.Enter:
                        Console.CursorVisible = true;
                        _key = menuElements[_index];
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(_languages[_key]);
                        return;
                }
                _index = (_index + menuElements.Length) % menuElements.Length;
            }
        }

        private void PrintMenuElements(string[] menuElements)
        {
            Console.Clear();
            Console.WriteLine(Messages.SelectLanguage);

            for (int i = 0; i < menuElements.Length; i++)
            {
                Console.WriteLine("{0} {1}", menuElements[i], i == _index ? "<<--" : "");
            }
        }
    }
}
