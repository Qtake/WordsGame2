namespace WordGame2
{
    internal class Player
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int WinCount { get; set; }

        public Player(int id, string name)
        {
            ID = id;
            Name = name;
            WinCount = 0;
        }
    }
}
