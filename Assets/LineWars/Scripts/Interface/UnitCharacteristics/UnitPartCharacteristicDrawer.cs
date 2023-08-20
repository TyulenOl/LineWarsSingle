using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitPartCharacteristicDrawer : MonoBehaviour
{
    [SerializeField] private CharacteristicDrawer CharacteristicDrawerPrefab;
    [SerializeField] private LayoutGroup CharacteristicsLayoutGroup;
    [SerializeField] private TMP_Text UnitName;
    [SerializeField] private SpriteRenderer ifInactivePanel;

    private CharacteristicDrawer MeleeAttackDrawer;
    private CharacteristicDrawer FarAttackDrawer;
    private CharacteristicDrawer ArmorDrawer;
    private CharacteristicDrawer HPDrawer;
    
    private Unit currentUnit;

    public Unit CurrentUnit
    {
        get => currentUnit;
        set
        {
            currentUnit = value;
            Init(currentUnit);
        }
    }

    private void Init(Unit unitToInit)
    {
        var hpSprite = DrawHelper.GetSpriteByCharacteristicType(CharacteristicType.Hp);
        var armorSprite = DrawHelper.GetSpriteByCharacteristicType(CharacteristicType.Armor);
        var attackSprite = DrawHelper.GetSpriteByCharacteristicType(CharacteristicType.MeleeAttack);
        MeleeAttackDrawer = Instantiate(CharacteristicDrawerPrefab.gameObject, CharacteristicsLayoutGroup.transform)
            .GetComponent<CharacteristicDrawer>();
        MeleeAttackDrawer.Init(attackSprite, unitToInit.MeleeDamage.ToString());
        ArmorDrawer = Instantiate(CharacteristicDrawerPrefab.gameObject, CharacteristicsLayoutGroup.transform)
            .GetComponent<CharacteristicDrawer>();
        ArmorDrawer.Init(armorSprite, unitToInit.ArmorChanged.ToString());
        HPDrawer = Instantiate(CharacteristicDrawerPrefab.gameObject, CharacteristicsLayoutGroup.transform)
            .GetComponent<CharacteristicDrawer>();
        HPDrawer.Init(hpSprite, unitToInit.CurrentHp.ToString());
        UnitName.text = DrawHelper.GetNameByUnitType(unitToInit.Type);
    }


    public void ReDrawActivity(bool isActive)
    {
        ifInactivePanel.gameObject.SetActive(!isActive);
    }
    
    public void ReDrawCharacteristics()
    {
        if( MeleeAttackDrawer != null)
        {
            MeleeAttackDrawer.ReDraw(currentUnit.MeleeDamage.ToString());
        }
        //TODO Добавить проверку на то, что юнит стреляющий. Если он стреляющий - выводить дальнюю атаку
        // if( FarAttackDrawer != null)
        // {
        //     FarAttackDrawer.ReDraw(currentUnit.MeleeDamage.ToString());
        // }
        if( ArmorDrawer != null)
        {
            ArmorDrawer.ReDraw(currentUnit.CurrentArmor.ToString());
        }
        if( HPDrawer != null)
        {
            HPDrawer.ReDraw(currentUnit.CurrentHp.ToString());
        }
    }
}
