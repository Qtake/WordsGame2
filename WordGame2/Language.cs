using System.Globalization;

namespace WordGame2
{
    internal class Language
    {
        private readonly Dictionary<string, string> _languages;
  
        public Language()
        {
            _languages = new Dictionary<string, string>()
            {
                { "English", "en-US" },
                { "Русский", "ru-RU" }
            };
        }

        public string[] GetKeys()
        {
            return _languages.Keys.ToArray();
        }

        public void SelectLanguage(string key)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(_languages[key]);
        }

    }
}
