using WordGame2;
using WordGame2.Languages;

public class Program
{
    public static void Main()
    {
        Language language = new Language();
        Menu languagesMenu = new Menu(language.GetKeys(), Messages.InputWord);
        int index = languagesMenu.SelectMenuElement();
        string key = language.GetKeys()[index];
        language.SelectLanguage(key);

        Gameplay gameplay = new Gameplay();
        gameplay.Start();
    }
}