using UnityEngine;

namespace LineWars.Model
{
    public class ToJsonConverter<TValue>: IConverter<TValue, string>
    {
        public string Convert(TValue value)
        {
            return JsonUtility.ToJson(value);
        }
    }
}