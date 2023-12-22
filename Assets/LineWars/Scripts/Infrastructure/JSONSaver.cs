using System;
using System.IO;
using UnityEngine;

namespace LineWars.Model
{
    public class JSONSaver<TValue, TOut> : ISaver<TValue>
    {
        private readonly IConverter<TValue, TOut> converter;
        private readonly Func<string, int, string> pathGenerator;
        public JSONSaver(IConverter<TValue, TOut> converter, Func<string, int, string> pathGenerator) 
        { 
            this.converter = converter;
            this.pathGenerator = pathGenerator;
        }

        public void Save(TValue value, int id)
        {
            var newDeckInfo = converter.Convert(value);
            var json = JsonUtility.ToJson(newDeckInfo);
            var objectName = nameof(TValue);
            var path = pathGenerator(objectName, id);
            File.WriteAllText(path, json);
        }
    }
}
