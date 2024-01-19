using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class ActionInfoDrawer : MonoBehaviour
    {
        [SerializeField] private TMP_Text actionName;
        [SerializeField] private TMP_Text actionFullDescription;
        [SerializeField] private Image actionImage;

        public void ReDraw(ActionReDrawInfo actionReDrawInfo)
        {
            actionName.text = actionReDrawInfo.Name;
            actionFullDescription.text = actionReDrawInfo.Description;
            actionImage.sprite = actionReDrawInfo.Sprite;
        }
    }
}
