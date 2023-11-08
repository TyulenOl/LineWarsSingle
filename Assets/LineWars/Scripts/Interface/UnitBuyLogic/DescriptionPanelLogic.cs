using System.Collections;
using System.Collections.Generic;
using LineWars;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanelLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private TMP_Text unitDescription;

    public void ReDrawDescription(UnitBuyPreset unitBuyPreset)
    {
        if (unitBuyPreset == null)
        {
            unitName.text = "";
            unitDescription.text = "";
            return;
        }

        unitName.text = unitBuyPreset.Name;
        unitDescription.text = unitBuyPreset.Description;
    }
}