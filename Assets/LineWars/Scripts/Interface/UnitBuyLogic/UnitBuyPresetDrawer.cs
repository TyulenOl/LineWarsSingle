using System;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class UnitBuyPresetDrawer : MonoBehaviour
    {
        [SerializeField] private TMP_Text cost;
        [SerializeField] private Image image;
        [SerializeField] private Image moneyImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Button button;
        [SerializeField] private Image ifChosenPanel;

        private bool isAvailable;

        public Button Button => button;

        private UnitBuyPreset unitBuyPreset;

        public bool IsAvailable => isAvailable;

        public UnitBuyPreset UnitBuyPreset
        {
            get => unitBuyPreset;
            set
            {
                unitBuyPreset = value;
                Init();
            }
        }

        private void Awake()
        {
            PhaseManager.Instance.PhaseChanged.AddListener(OnPhaseChanged);
        }

        private void OnPhaseChanged(PhaseType phaseTypeOld, PhaseType phaseTypeNew)
        {
            if (phaseTypeNew != PhaseType.Buy) return;
            SetAvailable(Player.LocalPlayer.CanBuyPreset(unitBuyPreset));
        }

        private void Init()
        {
            image.sprite = unitBuyPreset.Image;
            cost.text = unitBuyPreset.Cost.ToString();
            SetAvailable(Player.LocalPlayer.CanBuyPreset(unitBuyPreset));
        }


        public void SetChosen(bool isChosen)
        {
            if (isAvailable)
            {
                ifChosenPanel.gameObject.SetActive(isChosen);
            }
        }

        private void SetAvailable(bool isAvailable)
        {
            button.interactable = isAvailable;
            var color = isAvailable ? Color.white : new Color(226 / 255f, 43 / 255f, 18 / 255f, 255 / 255f);
            cost.color = color;
            moneyImage.color = color;
            backgroundImage.color = !isAvailable ? Color.gray : new Color(226 / 255f, 43 / 255f, 18 / 255f, 255 / 255f);
            image.color = isAvailable ? Color.white : Color.gray;
            this.isAvailable = isAvailable;
        }
    }
}