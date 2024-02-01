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
        
        public void Redraw(FullBlessingReDrawInfo fullBlessingReDrawInfo)
        {
            if (fullBlessingReDrawInfo.Sprite != null)
            {
                icon.gameObject.SetActive(true);
                ifNoneImage?.gameObject.SetActive(false);
                amount?.gameObject.SetActive(true);
                icon.sprite = fullBlessingReDrawInfo.Sprite;
            }
            else
            {
                icon.gameObject.SetActive(false);
                ifNoneImage?.gameObject.SetActive(true);
                amount?.gameObject.SetActive(false);
            }
            if (amount != null)
                amount.text = fullBlessingReDrawInfo.Amount.ToString();
            background.color = fullBlessingReDrawInfo.BgColor;

            if (fullBlessingReDrawInfo.Amount == 0)
            {
                icon.color = icon.color.WithAlpha(0.2f);
                if (ifNoneImage != null)
                    ifNoneImage.color = ifNoneImage.color.WithAlpha(0.2f);
                background.color = background.color.WithAlpha(0.2f);
            }
            else
            {
                icon.color = icon.color.WithAlpha(1);
                if (ifNoneImage != null)
                    ifNoneImage.color = ifNoneImage.color.WithAlpha(1);
                background.color = background.color.WithAlpha(1);
            }
        }
    }
}
