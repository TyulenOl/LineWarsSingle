using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Interface;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoDrawer : MonoBehaviour
{
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private TMP_Text unitDescription;
    
    [SerializeField] private TMP_Text hpAmount;
    [SerializeField] private TMP_Text actionPointsAmount;
    [SerializeField] private TMP_Text damageAmount;

    [SerializeField] private Image unitImage;

    public void Init(Unit unit)
    {
        if(unitName != null)
            unitName.text = unit.UnitName;
        if(unitDescription != null)
            unitDescription.text = unit.UnitDescription;
        
        hpAmount.text = unit.MaxHp.ToString();
        actionPointsAmount.text = unit.MaxActionPoints.ToString();
        damageAmount.text = unit.InitialPower.ToString();

        unitImage.gameObject.SetActive(true);
        unitImage.sprite = unit.Sprite;
    }

    public void RestoreDefaults()
    {
        unitName.text = "Выберите воина";
        unitDescription.text = "";
        
        hpAmount.text = "?";
        actionPointsAmount.text = "?";
        damageAmount.text = "?";

        if (damageAmount != null)
        {
            damageAmount.text = "?";
        }
        
        unitImage.gameObject.SetActive(false);
        unitImage.sprite = null;
    }
}