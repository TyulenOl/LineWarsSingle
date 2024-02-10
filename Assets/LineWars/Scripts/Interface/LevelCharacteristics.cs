using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class LevelCharacteristics: MonoBehaviour
    {
        [SerializeField] private TMP_Text current, next;

        public TMP_Text Current => current;
        public TMP_Text Next => next;
    }
}