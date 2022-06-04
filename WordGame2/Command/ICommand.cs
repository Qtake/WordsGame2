namespace WordGame2.Command
{
    internal interface ICommand
    {
        bool CanExecute();
        void Execute();
    }
}
