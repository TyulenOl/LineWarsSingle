using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using UnityEngine;
using UnityEngine.UI;

public class UnitsInfoPanel : MonoBehaviour
{
    [SerializeField] private UnitBuyPresetInfoDrawer PresetInfoDrawerPrefab;
    [SerializeField] private LayoutGroup presetsDrawingLayoutGroup;

    private void Awake()
    {
        foreach (var unit in Player.LocalPlayer.Nation.UnitTypeUnitPairs.Values)
        {
            var instance = Instantiate(PresetInfoDrawerPrefab, presetsDrawingLayoutGroup.transform).gameObject;
            instance.GetComponent<UnitBuyPresetInfoDrawer>().Init(unit);
        }
    }
}