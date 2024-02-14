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
            openAdButton.PrizeForAd = new Prize(PrizeType.Gold, WinOrLoseScene.GoldAmount);
            openAdButton.Button.onClick.AddListener(OpenAdButtonOnClick);
        }
        
        private void OnDisable()
        {
            if (openAdButton != null && openAdButton.Button != null)
                openAdButton.Button.onClick.RemoveListener(OpenAdButtonOnClick);
        }

        private void OpenAdButtonOnClick()
        {
            openAdButton.Button.interactable = false;
        }
    }
}