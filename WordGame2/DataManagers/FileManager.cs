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

        public void WriteData(string winner)
        {
            List<Player>? previousPlayers = ReadData();
            string jsonString;

            if (previousPlayers == null)
            {
                Player[] newPlayer = { new Player(winner) { WinCount = 1 } };
                jsonString = JsonSerializer.Serialize(newPlayer);
                CreateFile(jsonString);
                return;
            }

            if (previousPlayers.Select(x => x.Name).Contains(winner))
            {
                previousPlayers.First(x => x.Name == winner).WinCount++;
                jsonString = JsonSerializer.Serialize(previousPlayers);
                CreateFile(jsonString);
                return;
            }

            previousPlayers.Add(new Player(winner) { WinCount = 1 });
            jsonString = JsonSerializer.Serialize(previousPlayers);
            CreateFile(jsonString);
        }

        private void CreateFile(string jsonString)
        {
            File.WriteAllText(_filePath, jsonString);
        }
    }
}
