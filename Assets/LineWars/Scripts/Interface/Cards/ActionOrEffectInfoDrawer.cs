using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class ActionOrEffectInfoDrawer : MonoBehaviour
    {
        [SerializeField] private TMP_Text actionName;
        [SerializeField] private TMP_Text actionFullDescription;
        [SerializeField] private Image actionImage;

        public void ReDraw(ActionOrEffectReDrawInfo actionReDrawInfo)
        {
            actionName.text = actionReDrawInfo.Name;
            actionFullDescription.text = actionReDrawInfo.Description;
            actionImage.sprite = actionReDrawInfo.Sprite;
        }
    }
}
