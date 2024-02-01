using LineWars.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BuyPanel : MonoBehaviour
    {
        [SerializeField] private Button buyAndOpenButton;
        [SerializeField] private CanvasGroup inactiveCanvasGroup;
        
        [SerializeField] private Image lootBoxImage;
        [SerializeField] private TMP_Text boxName;
        [SerializeField] private TMP_Text boxDescription;
        [SerializeField] private CostDrawer costDrawer;
        
        public UnityEvent OnClick => buyAndOpenButton.onClick;
        
        public void OpenWindow(BuyPanelReDrawInfo buyPanelReDrawInfo)
        {
            buyAndOpenButton.interactable = buyPanelReDrawInfo.ButtonInteractable;
            inactiveCanvasGroup.alpha = buyPanelReDrawInfo.ButtonInteractable ? 1f : 0.5f;
            
            costDrawer.DrawCost(buyPanelReDrawInfo.Cost, buyPanelReDrawInfo.CostType);
            boxName.text = buyPanelReDrawInfo.PanelName;
            if (boxDescription != null) 
                boxDescription.text = buyPanelReDrawInfo.PanelDescription;
            lootBoxImage.sprite = buyPanelReDrawInfo.UnitSprite;
            
            gameObject.SetActive(true);
        }
    }
}
