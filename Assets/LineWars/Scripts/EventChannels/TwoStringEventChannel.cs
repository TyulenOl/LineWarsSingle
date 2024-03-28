using System;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "Two String Event", menuName = "Events/Two String")]
    public class TwoStringEventChannel : ScriptableObject
    {
        public event Action<string, string> Raised;

        public void RaiseEvent(string value, string value2)
        {
            Raised?.Invoke(value, value2);
        }
    }
}
