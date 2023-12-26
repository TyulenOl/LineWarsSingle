using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using LineWars.Controllers;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuyLayerLogic : MonoBehaviour
{
    [SerializeField] private RectTransform buyUnitsLayer;

    private DeckCard deckCard;
    public DeckCard CurrentDeckCard
    {
        get => deckCard;
        set
        {
            deckCard = value;
            CommandsManager.Instance.SetDeckCard(value);
        }
    }
    
    private CardBuyPresetDrawer chosenCardPresetDrawer;
    
    public CardBuyPresetDrawer ChosenCardPresetDrawer
    {
        get => chosenCardPresetDrawer;
        set
        {
            chosenCardPresetDrawer?.SetChosen(false);
            chosenCardPresetDrawer = value;
            chosenCardPresetDrawer?.SetChosen(true);
        }
    }

    private void Start()
    {
        Player.LocalPlayer.TurnStarted += OnPhaseStarted;
    }

    private void OnPhaseStarted(IActor _,PhaseType phaseTypeNew)
    {
        if (phaseTypeNew != PhaseType.Buy) return;
        CurrentDeckCard = null;
        ChosenCardPresetDrawer = null;
        buyUnitsLayer.gameObject.SetActive(true);
    }
}