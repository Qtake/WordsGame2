using System.Text.Json;
using WordGame2;
using WordGame2.Languages;

public class Program
{
    public static void Main()
    {

        Language applicationLanguage = new Language();
        Menu languagesMenu = new Menu(applicationLanguage.Languages.Keys.ToArray(), Messages.SelectLanguage);
        string key = languagesMenu.SelectMenuElement();
        applicationLanguage.SelectLanguage(key);

        Game game = new Game();
        game.ExecuteCommand("/total-score");
        //game.EnterPlayerNames();
        //game.Start();

        /*
        string a = "test1";
        string b = "test2";

        Stack<string> stack = new Stack<string>();
        stack.Push(a);
        stack.Push(b);

        string fileName = "UsedWords.json";
        string jsonString = JsonSerializer.Serialize(stack);

        File.WriteAllText(fileName, jsonString);
        string s = File.ReadAllText(fileName);
        Console.WriteLine(s);

        string[] des = JsonSerializer.Deserialize<string[]>(s);
        */
    }
}