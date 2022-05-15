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
        game.EnterPlayerNames();
        game.Start();
    }
}