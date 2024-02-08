using System;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class GoldBuyPresetLogic : MonoBehaviour
    {
        [SerializeField] private BuyPanel buyPanel;
        [SerializeField] private int goldAmount;
        [SerializeField] private int costInDiamonds;

        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text amountText;

        private static UserInfoController UserController => GameRoot.Instance.UserController;

        private void Start()
        {
            ReDraw();
            buyButton.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            buyButton.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            buyPanel.OpenWindow(GetBuyPanelReDrawInfo());
            buyPanel.OnClick.AddListener(BuyGold);
        }

        private BuyPanelReDrawInfo GetBuyPanelReDrawInfo()
        {
            return new BuyPanelReDrawInfo
            (
                DrawHelper.GoldSprite,
                ParseAmount(goldAmount),
                GetDescription(),
                costInDiamonds,
                CostType.Diamond,
                UserController.UserDiamond >= costInDiamonds
            );
        }

        private void BuyGold()
        {
            if (UserController.UserDiamond < costInDiamonds)
                throw new InvalidOperationException();
            
            UserController.UserDiamond -= costInDiamonds;
            UserController.UserGold += goldAmount;
            
            buyPanel.OnClick.RemoveListener(BuyGold);
        }
        
        private void ReDraw()
        {
            costText.text = costInDiamonds.ToString();
            amountText.text = ParseAmount(goldAmount);
        }

        private string GetDescription()
        {
            return $"Вы приобретаете {ParseAmount(goldAmount)} за {costInDiamonds} кристаллов";
        }
        
        private string ParseAmount(int amount)
        {
            return $"{amount} монет";
        }
    }
}   