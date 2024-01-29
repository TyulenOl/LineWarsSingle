using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars
{
    public class BlessingInfoDrawer : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text amount;
        [SerializeField] private Image background;

        public void Init(BlessingReDrawInfo blessingReDrawInfo)
        {
            image.sprite = blessingReDrawInfo.Sprite;
            amount.text = blessingReDrawInfo.Amont.ToString();
            background.color = blessingReDrawInfo.BgColor;
        }
    }
}
