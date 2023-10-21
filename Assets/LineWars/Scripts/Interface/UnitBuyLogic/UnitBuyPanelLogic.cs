using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class UnitBuyPanelLogic : MonoBehaviour
    {
        [SerializeField] private LayoutGroup presetsLayoutGroup;
        [SerializeField] private UnitBuyPresetDrawer presetDrawerPrefab;
        [SerializeField] private UnitBuyLayerLogic unitBuyLayerLogic;

        private List<UnitBuyPresetDrawer> unitBuyPresetDrawers;
        private Dictionary<UnitBuyPreset, UnitBuyPresetDrawer> unitBuyPresetDrawersDictionary;

        private Nation nation;
        private Node baseToSpawnUnits;

        public void Awake()
        {
            nation = Player.LocalPlayer.Nation;
            GeneratePresets();
        }

        private void GeneratePresets()
        {
            var presets = nation.NationEconomicLogic;
            foreach (var preset in presets)
            {
                var presetDrawer = Instantiate(presetDrawerPrefab.gameObject, presetsLayoutGroup.transform)
                    .GetComponent<UnitBuyPresetDrawer>();
                presetDrawer.UnitBuyPreset = preset;
                presetDrawer.Button.onClick.AddListener(() =>
                {
                    unitBuyLayerLogic.CurrentPreset = presetDrawer.UnitBuyPreset;
                    unitBuyLayerLogic.ChosenUnitPresetDrawer = presetDrawer;
                });
            }
        }
    }
}