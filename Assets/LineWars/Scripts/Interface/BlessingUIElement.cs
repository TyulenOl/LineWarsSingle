using System;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingUIElement: MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image blessingIcon;
        [SerializeField] private TMP_Text blessingName;
        [SerializeField] private TMP_Text blessingCount;

        private BlessingId blessingData;

        public event Action<BlessingId> OnClick;

        private void Start()
        {
            button.onClick.AddListener(ButtonOnClick);
        }

        private void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveListener(ButtonOnClick);
        }

        private void ButtonOnClick()
        {
            OnClick?.Invoke(blessingData);
        }

        public BlessingId BlessingData
        {
            set
            {
                blessingData = value;
                if (value != null)
                    blessingName.text = $"{value.BlessingType} {value.Rarity}";
            }
        }

        public int BlessingCount
        {
            set
            {
                blessingCount.text = $"x{value}";
            }
        }

        public BlessingUIElementState State
        {
            set
            {
                switch (value)
                {
                    case BlessingUIElementState.Active:
                        blessingIcon.color = Color.green;
                        button.interactable = false;
                        break;
                    case BlessingUIElementState.Locked:
                        blessingIcon.color = Color.gray;
                        button.interactable = false;
                        break;
                    case BlessingUIElementState.Unlocked:
                        blessingIcon.color = Color.white;
                        button.interactable = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }
    }

    public enum BlessingUIElementState
    {
        Active,
        Locked,
        Unlocked
    }
}