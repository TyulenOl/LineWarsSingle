using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuyPanelLogic : MonoBehaviour
{
    [SerializeField] private LayoutGroup presetsLayoutGroup;
    [SerializeField] private UnitBuyPresetDrawer presetDrawerPrefab;
    
    private List<UnitBuyPresetDrawer> unitBuyPresetDrawers;
    private Dictionary<UnitBuyPreset, UnitBuyPresetDrawer> unitBuyPresetDrawersDictionary;

    private Nation nation;
    private Node baseToSpawnUnits;

    public void Awake()
    {
        nation = NationHelper.GetNationByType(Player.LocalPlayer.NationType);
        GeneratePresets();
    }

    private void GeneratePresets()
    {
        var presets = nation.NationEconomicLogic.UnitBuyPresets;
        foreach (var preset in presets)
        {
            var presetDrawer = Instantiate(presetDrawerPrefab.gameObject, presetsLayoutGroup.transform).GetComponent<UnitBuyPresetDrawer>();
            presetDrawer.UnitBuyPreset = preset;
            presetDrawer.Button.onClick.AddListener( () =>
            {
                var player = Player.LocalPlayer;
                if(!player.CanExecuteBuy()) return; //TODO Покупка только в фазу покупки
                UnitsController.ExecuteCommand(
                    new SpawnPresetCommand(
                        player,
                        player.Base,
                        preset
                    ),false);
            });
        }
    }
}
