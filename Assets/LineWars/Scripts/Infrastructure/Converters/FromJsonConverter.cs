using UnityEngine;

namespace LineWars.Model
{
    public class FromJsonConverter<TValue>: IConverter<string, TValue>
    {
        public TValue Convert(string value)
        {
            return JsonUtility.FromJson<TValue>(value);
        }
    }
}