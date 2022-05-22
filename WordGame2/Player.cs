namespace WordGame2
{
    internal class Player
    { 
        public string Name { get; private set; }
        public int WinCount { get; set; }

        public Player(string name)
        {
            Name = name;
            WinCount = 0;
        }
    }
}
