using UnityEngine;

namespace LineWars.Model
{
    public class PlayerPrefsSaver<T>: ISaver<T>
    {
        private readonly IConverter<T, string> converter;
        
        public PlayerPrefsSaver(IConverter<T, string> converter)
        {
            this.converter = converter;
        }

        public void Save(T value, int id)
        {
            var str = converter.Convert(value);
            var key = PathsHelper.GenerateKeyForPlayerPrefs<T>(id);
            PlayerPrefs.SetString(key, str);
            PlayerPrefs.Save();
        }
    }
}