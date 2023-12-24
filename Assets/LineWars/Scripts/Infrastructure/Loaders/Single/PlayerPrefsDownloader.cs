using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class PlayerPrefsDownloader<T>: IDownloader<T>
    {
        private readonly IConverter<string, T> converter;

        public PlayerPrefsDownloader(IConverter<string, T> converter)
        {
            this.converter = converter;
        }

        public T Load(int id)
        {
            var key = PathsHelper.GenerateKeyForPlayerPrefs<T>(id);
            var str = PlayerPrefs.GetString(key);
            var value = converter.Convert(str);
            return value;
        }
    }
}