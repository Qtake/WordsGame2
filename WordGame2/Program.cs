using WordGame2;
using WordGame2.Messages;
using WordGame2.Other;

public class Program
{
    public static void Main()
    {
        Language applicationLanguage = new Language();
        Menu languagesMenu = new Menu(applicationLanguage.Languages.Keys.ToArray(), Message.SelectLanguage);
        string key = languagesMenu.SelectMenuElement();
        applicationLanguage.SelectLanguage(key);

        Game game = new Game();

        if (game.EnterPlayerNames())
        {
            game.Start();
        }
    }
}