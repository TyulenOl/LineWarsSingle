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

    private UnitBuyPreset currentPreset;
    public UnitBuyPreset CurrentPreset
    {
        get => currentPreset;
        set
        {
            currentPreset = value;
            CommandsManager.Instance.SetUnitPreset(value);
        }
    }
    
    private UnitBuyPresetDrawer chosenUnitPresetDrawer;
    
    public UnitBuyPresetDrawer ChosenUnitPresetDrawer
    {
        get => chosenUnitPresetDrawer;
        set
        {
            chosenUnitPresetDrawer?.SetChosen(false);
            chosenUnitPresetDrawer = value;
            chosenUnitPresetDrawer?.SetChosen(true);
        }
    }

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
}