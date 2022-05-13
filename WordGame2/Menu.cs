namespace WordGame2
{
    internal class Menu
    {
        private readonly string[] _menuElements;
        private readonly string _message;
        private int _index;

        public Menu(string[] menuElements, string message)
        {
            _menuElements = menuElements;
            _message = message;
            _index = 0;
        }

        public int SelectMenuElement()
        {
            Console.CursorVisible = false;

            while (true)
            {
                CreateMenu();
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        _index--;
                        break;
                    case ConsoleKey.DownArrow:
                        _index++;
                        break;
                    case ConsoleKey.Enter:
                        return _index;
                }
                _index = (_index + _menuElements.Length) % _menuElements.Length;
            }
        }

        private void CreateMenu()
        {
            Console.Clear();
            Console.WriteLine(_message);

            for (int i = 0; i < _menuElements.Length; i++)
            {
                Console.WriteLine("{0} {1}", _menuElements[i], i == _index ? "<<--" : "");
            }
        }
    }
}
