using System.Globalization;

namespace WordGame2.Other
{
    internal class Language
    {
        public Dictionary<string, string> Languages { get; private set; }

        public Language()
        {
            Languages = new Dictionary<string, string>()
            {
                { "English", "en-US" },
                { "Русский", "ru-RU" }
            };
        }

        public void SelectLanguage(string key)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Languages[key]);
        }
    }
}
