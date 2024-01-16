using System;
using LineWars.Controllers;
using LineWars.LootBoxes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class GoldBuyPresetLogic : MonoBehaviour
    {
        private static Sprite goldSprite => Resources.Load<Sprite>("UI/Sorokin/Icons/ColoredMoney");
        
        [SerializeField] private BuyPanel buyPanel;
        [SerializeField] private int goldAmount;
        [SerializeField] private int costInDiamonds;

        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text amountText;

        private void Awake()
        {
            ReDraw();
            buyButton.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            buyPanel.OpenWindow(GetBuyPanelReDrawInfo());
        }

        private BuyPanelReDrawInfo GetBuyPanelReDrawInfo()
        {
            return new BuyPanelReDrawInfo
            (() =>
                {
                    GameRoot.Instance.UserController.UserDiamond -= costInDiamonds;
                    GameRoot.Instance.UserController.UserGold += goldAmount;
                },
                () => GameRoot.Instance.UserController.UserDiamond >= costInDiamonds,
                goldSprite,
                ParseAmount(goldAmount),
                GetDescription(),
                costInDiamonds,
                CostType.Diamond
            );
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