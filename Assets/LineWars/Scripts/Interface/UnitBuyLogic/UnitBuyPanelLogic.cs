using System;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class UnitBuyPanelLogic : MonoBehaviour
    {
        [SerializeField] private LayoutGroup presetsLayoutGroup;
        [SerializeField] private CardBuyPresetDrawer presetDrawerPrefab;
        [SerializeField] private UnitBuyLayerLogic unitBuyLayerLogic;
        [FormerlySerializedAs("chosenPresetInfoDrawer")] [SerializeField] private UnitInfoDrawer chosenDeckCardInfoDrawer;
        
        private List<CardBuyPresetDrawer> unitBuyPresetDrawers;
        private Node baseToSpawnUnits;

        public void Awake()
        {
            GeneratePresets();
        }

        private void GeneratePresets()  
        {
            foreach (var card in SingleGameRoot.Instance.CurrentDeck.Cards)
            {
                var presetDrawer = Instantiate(presetDrawerPrefab.gameObject, presetsLayoutGroup.transform)
                    .GetComponent<CardBuyPresetDrawer>();
                presetDrawer.DeckCard = card;
                presetDrawer.Button.onClick.AddListener(() =>
                {
                    unitBuyLayerLogic.CurrentDeckCard = card;
                    unitBuyLayerLogic.ChosenCardPresetDrawer = presetDrawer;
                    chosenDeckCardInfoDrawer.Init(card.Unit);
                });
            }
        }

        private void OnEnable()
        {
            chosenDeckCardInfoDrawer.RestoreDefaults();
        }
    }
}