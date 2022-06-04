namespace WordGame2.Command
{
    internal class GameCommand : ICommand
    {
        private Action? _command;

        public GameCommand()
        {
            _command = null;
        }

        public void SetCommand(Action command) => _command = command;

        public bool CanExecute() => _command != null;

        public void Execute()
        {
            _command?.Invoke();
        }
    }
}
