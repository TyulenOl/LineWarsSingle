using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class LevelCharacteristics: MonoBehaviour
    {
        [SerializeField] private TMP_Text current, next;
        [SerializeField] private LayoutGroup layoutGroup;

        public TMP_Text Current => current;
        public TMP_Text Next => next;

        public LayoutGroup LayoutGroup => layoutGroup;
    }
}