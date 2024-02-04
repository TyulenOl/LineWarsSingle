using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class ShopCardBg: MonoBehaviour
    {
        [SerializeField] private List<Image> rombs;
        [SerializeField] private List<Image> lines;
        [SerializeField] private Image bg;

        public IReadOnlyList<Image> Rombs => rombs;
        public IReadOnlyList<Image> Lines => lines;
        public Image Bg => bg;
    }
}