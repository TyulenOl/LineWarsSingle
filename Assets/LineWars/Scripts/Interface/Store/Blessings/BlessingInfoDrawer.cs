using System;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingInfoDrawer : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image ifNoneImage;
        [SerializeField] private TMP_Text amount;
        [SerializeField] private Image background;
        
        public void Redraw(AllBlessingReDrawInfo allBlessingReDrawInfo)
        {
            if (allBlessingReDrawInfo.Sprite != null)
            {
                icon.gameObject.SetActive(true);
                ifNoneImage?.gameObject.SetActive(false);
                amount.gameObject.SetActive(true);
                icon.sprite = allBlessingReDrawInfo.Sprite;
            }
            else
            {
                icon.gameObject.SetActive(false);
                ifNoneImage?.gameObject.SetActive(true);
                amount.gameObject.SetActive(false);
            }
            amount.text = allBlessingReDrawInfo.Amount.ToString();
            background.color = allBlessingReDrawInfo.BgColor;
        }
        
        
    }
}
