using System.Text.Json;

namespace WordGame2.DataManagers
{
    internal class FileManager : IDataManager
    {
        private readonly string _filePath;

        public FileManager(string filePath)
        {
            _filePath = filePath;
        }

        public List<Player>? ReadData()
        {
            List<Player>? previousPlayers = null;

            if (File.Exists(_filePath) && new FileInfo(_filePath).Length > 0)
            {
                string fileText = File.ReadAllText(_filePath);
                previousPlayers = JsonSerializer.Deserialize<List<Player>>(fileText);
            }

            return previousPlayers;
        }

        public void WriteData(List<Player> players)
        {
            string jsonString = JsonSerializer.Serialize(players);
            File.WriteAllText(_filePath, jsonString);
        }
    }
}
