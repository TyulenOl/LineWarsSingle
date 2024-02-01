using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class DropElement: MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image icon;

        public Image Background => background;
        public Image Icon => icon;
    }
}