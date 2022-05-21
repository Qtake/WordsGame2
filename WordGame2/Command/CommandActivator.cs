namespace WordGame2.Command
{
    internal class CommandActivator
    {
        private ICommand? _command;

        public CommandActivator() => _command = null;

        public void SetCommand(ICommand command) => _command = command;

        public void ActivateCommand()
        {
            if (_command != null)
            {
                _command.Execute();
            }
        }
    }
}
