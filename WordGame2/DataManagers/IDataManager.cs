namespace WordGame2.DataManagers
{
    internal interface IDataManager
    {
        List<Player>? ReadData();
        void WriteData(List<Player> players);
    }
}
