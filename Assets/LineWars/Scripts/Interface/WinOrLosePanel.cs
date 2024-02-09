using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    public class WinOrLosePanel: MonoBehaviour
    {
        [SerializeField] private OpenAdButton openAdButton;
        
        private SDKAdapterBase SDKAdapter => GameRoot.Instance.SdkAdapter;

        private void OnEnable()
        {
            openAdButton.PrizeForAd = new Prize(PrizeType.Gold, WinOrLoseScene.MoneyAmount);
            
            SDKAdapter.RewardVideoEvent += SDKAdapterOnRewardVideoEvent;
            SDKAdapter.ErrorVideoEvent += SDKAdapterOnErrorVideoEvent;
        }


        private void OnDisable()
        {
            SDKAdapter.RewardVideoEvent -= SDKAdapterOnRewardVideoEvent;
            SDKAdapter.ErrorVideoEvent -= SDKAdapterOnErrorVideoEvent;
        }

        private void SDKAdapterOnRewardVideoEvent(PrizeType prize, int amount)
        {
            if (prize == PrizeType.Gold && amount == WinOrLoseScene.MoneyAmount)
            {
                openAdButton.Button.interactable = false;
                FullscreenPanel.OpenPanel(new TextRedrawInfo("Успешно", Color.red), new Money(CostType.Gold, amount));
            }
        }
        
        private void SDKAdapterOnErrorVideoEvent()
        {
            FullscreenPanel.OpenPanel(
                new TextRedrawInfo("Ой-ой", Color.white),
                new TextRedrawInfo("произошла ошибка", Color.white)
            );
        }
    }
}