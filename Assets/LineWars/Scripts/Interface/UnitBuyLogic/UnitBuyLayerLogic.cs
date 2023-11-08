using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuyLayerLogic : MonoBehaviour
{
    [SerializeField] private RectTransform buyUnitsLayer;
    [SerializeField] private Button buyButton;


    private UnitBuyPresetDrawer chosenUnitPresetDrawer;

    public UnitBuyPresetDrawer ChosenUnitPresetDrawer
    {
        get => chosenUnitPresetDrawer;
        set
        {
            chosenUnitPresetDrawer?.SetChosen(false);
            chosenUnitPresetDrawer = value;
            chosenUnitPresetDrawer?.SetChosen(true);
            buyButton.interactable = value != null && value.IsAvailable;
        }
    }

    public UnitBuyPreset CurrentPreset { get; set; }


    private void Start()
    {
        PhaseManager.Instance.PhaseChanged.AddListener(OnPhaseChanged);
    }

    private void OnPhaseChanged(PhaseType phaseTypeOld, PhaseType phaseTypeNew)
    {
        if (phaseTypeNew != PhaseType.Buy) return;
        CurrentPreset = null;
        ChosenUnitPresetDrawer = null;
        buyUnitsLayer.gameObject.SetActive(true);
    }

    public void SpawnCurrentPreset()
    {
        if (CurrentPreset == null) return;
        var player = Player.LocalPlayer;
        if (PhaseManager.Instance.CurrentPhase != PhaseType.Buy) return;
        UnitsController.ExecuteCommand(
            new SpawnPresetCommand<Node, Edge, Unit, BasePlayer>(
                player,
                CurrentPreset
            ), false);
    }
}