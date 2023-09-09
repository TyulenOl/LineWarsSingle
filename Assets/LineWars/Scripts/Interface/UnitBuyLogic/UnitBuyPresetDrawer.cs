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
        [SerializeField] private Button button;
        [SerializeField] private Image ifChosenPanel;
        [SerializeField] private Image ifUnAvailablePanel;
        [SerializeField] private Image ifAvailablePanel;

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
            if(phaseTypeNew != PhaseType.Buy) return;
            SetAvailable(Player.LocalPlayer.CanSpawnPreset(unitBuyPreset));
        }

        private void Init()
        {
            image.sprite = unitBuyPreset.Image;
            cost.text = unitBuyPreset.Cost.ToString();
            SetAvailable(Player.LocalPlayer.CanSpawnPreset(unitBuyPreset));
        }

        
        public void SetChosen(bool isChosen)
        {
            if(isAvailable)
                ifChosenPanel.gameObject.SetActive(isChosen);
            else
                ifUnAvailablePanel.gameObject.SetActive(isChosen);
        }
        
        private void SetAvailable(bool isAvailable)
        {
            ifAvailablePanel.gameObject.SetActive(!isAvailable);
            this.isAvailable = isAvailable;
        }
    }
}