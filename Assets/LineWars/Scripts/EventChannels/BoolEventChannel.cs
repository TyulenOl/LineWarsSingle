using System;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "Two String Event", menuName = "Events/Bool")]
    public class BoolEventChannel : ScriptableObject
    {
        public event Action<bool> Raised;

        public void RaiseEvent(bool value)
        {
            Raised?.Invoke(value);
        }
    }

}
