using System;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(Button))]
    public class OpenAdButton: MonoBehaviour
    {
        [SerializeField] private Prize prizeForAd;
        private static SDKAdapterBase SdkAdapter => GameRoot.Instance.SdkAdapter;
        private Button button;

        public Prize PrizeForAd
        {
            get => prizeForAd;
            set => prizeForAd = value;
        }

        public Button Button => button;


        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            button?.onClick?.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (SdkAdapter != null)
                SdkAdapter.RewardForAd(prizeForAd);
        }
    }
}