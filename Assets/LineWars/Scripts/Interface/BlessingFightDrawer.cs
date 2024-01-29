using System;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingFightDrawer: MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image blessingIcon;
        [SerializeField] private TMP_Text blessingName;
        [SerializeField] private TMP_Text blessingCount;

        private BlessingId blessingId;

        public event Action<BlessingId> OnClick;

        public BlessingId BlessingData
        {
            set
            {
                blessingId = value;
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

        public BlessingDrawerState State
        {
            set
            {
                switch (value)
                {
                    case BlessingDrawerState.Active:
                        blessingIcon.color = Color.green;
                        button.interactable = false;
                        break;
                    case BlessingDrawerState.Locked:
                        blessingIcon.color = Color.gray;
                        button.interactable = false;
                        break;
                    case BlessingDrawerState.Unlocked:
                        blessingIcon.color = Color.white;
                        button.interactable = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }
        
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
            OnClick?.Invoke(blessingId);
        }
    }

    public enum BlessingDrawerState
    {
        Active,
        Locked,
        Unlocked
    }
}