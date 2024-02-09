using System;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class PurchaseDrawer: MonoBehaviour
    {
        private static SDKAdapterBase SDKAdapter => GameRoot.Instance.SdkAdapter;
        
        [SerializeField] private TextMeshProUGUI title, description, priceValue;
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private bool addCurrency_toPrice = true;
        
        public UnityEvent<UserPurchaseInfo> OnClick;
        public UserPurchaseInfo Data { get; set; }

        private void OnEnable()
        {
            if (button)
                button.onClick.AddListener(OnButtonClick);
        }
        private void OnDisable()
        {
            if (button)
                button.onClick.RemoveListener(OnButtonClick);
        }

        [ContextMenu(nameof(UpdateEntries))]
        public void UpdateEntries()
        {
            if (title) title.text = Data.Title;
            if (description) description.text = Data.Description;
            if (priceValue)
            {
                priceValue.text = Data.PriceValue.ToString();
                if (addCurrency_toPrice) priceValue.text += Data.CurrencyName;
            }

            if (image) image.sprite = Data.Sprite;

            if (button) button.interactable = SDKAdapter.CanBuyPurchase(Data.Id);
        }

        private void OnButtonClick()
        {
            OnClick.Invoke(Data);
        }
    }
}