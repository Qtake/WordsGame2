using WordGame2;
using WordGame2.Languages;

public class Program
{
    public static void Main()
    {
        Language language = new Language();
        Menu languagesMenu = new Menu(language.GetKeys(), Messages.InputWord);
        int index = languagesMenu.SelectMenuElement();
        string key = language.GetKeys().ToArray()[index];
        language.SelectLanguage(key);

        Console.Clear();
        Console.WriteLine(Messages.InputWord);
        string primaryWord = (Console.ReadLine() ?? "").ToLower();
        

        Gameplay gameplay = new Gameplay(primaryWord);
        gameplay.Start();
        gameplay.Restart();

    }
}