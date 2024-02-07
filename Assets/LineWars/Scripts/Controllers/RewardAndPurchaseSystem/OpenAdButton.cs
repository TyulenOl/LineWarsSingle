using System;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(Button))]
    public class OpenAdButton: MonoBehaviour
    {
        [SerializeField] private Prize prizeForAd;
        private static RewardControllerBase RewardController => GameRoot.Instance.RewardController;
        private Button button;

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
            if (RewardController != null)
                RewardController.RewardForAd(prizeForAd);
        }
    }
}