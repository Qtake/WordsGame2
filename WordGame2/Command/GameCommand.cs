namespace WordGame2.Command
{
    internal class GameCommand : ICommand
    {
        private readonly Game _game;
        private readonly string _command;

        public GameCommand(Game game, string command)
        {
            _game = game;
            _command = command;
        }

        public void Execute()
        {
            if (_game.Commands.ContainsKey(_command))
            {
                _game.Commands[_command].Invoke();
            }
            else
            {
                Console.WriteLine(Messages.Message.UnknownCommand);
            }
        }
    }
}
