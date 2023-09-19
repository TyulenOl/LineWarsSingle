using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitPartDrawer : MonoBehaviour
{
    [SerializeField] private CharacteristicDrawer CharacteristicDrawerPrefab;
    [SerializeField] private LayoutGroup CharacteristicsLayoutGroup;
    [SerializeField] private TMP_Text UnitName;
    [SerializeField] private SpriteRenderer ifInactivePanel;    
    [SerializeField] private SpriteRenderer ifAvailablePanel;
    [SerializeField] private SpriteRenderer unitIsExecutorImage;
    [SerializeField] private SpriteRenderer canBlockSprite;
    [field: SerializeField] public SpriteRenderer targetSprite { get; private set; }
    
    private CharacteristicDrawer MeleeAttackDrawer;
    private CharacteristicDrawer FarAttackDrawer;
    private CharacteristicDrawer ArmorDrawer;
    private CharacteristicDrawer HPDrawer;
    private CharacteristicDrawer ActionPointsDrawer;
    
    private ComponentUnit currentUnit;

    public ComponentUnit CurrentUnit
    {
        get => currentUnit;
        set
        {
            currentUnit = value;
            Init(currentUnit);
        }
    }

    private void Init(ComponentUnit unitToInit)
    {
        var hpSprite = DrawHelper.GetSpriteByCharacteristicType(CharacteristicType.Hp);
        var armorSprite = DrawHelper.GetSpriteByCharacteristicType(CharacteristicType.Armor);
        var attackSprite = DrawHelper.GetSpriteByCharacteristicType(CharacteristicType.MeleeAttack);
        var actionPointsSprite = DrawHelper.GetSpriteByCharacteristicType(CharacteristicType.ActionPoints);
        
        MeleeAttackDrawer = Instantiate(CharacteristicDrawerPrefab.gameObject, CharacteristicsLayoutGroup.transform)
            .GetComponent<CharacteristicDrawer>();
        MeleeAttackDrawer.Init(attackSprite, unitToInit.GetDamage().ToString());
        
        ArmorDrawer = Instantiate(CharacteristicDrawerPrefab.gameObject, CharacteristicsLayoutGroup.transform)
            .GetComponent<CharacteristicDrawer>();
        ArmorDrawer.Init(armorSprite, unitToInit.ArmorChanged.ToString());
        
        HPDrawer = Instantiate(CharacteristicDrawerPrefab.gameObject, CharacteristicsLayoutGroup.transform)
            .GetComponent<CharacteristicDrawer>();
        HPDrawer.Init(hpSprite, unitToInit.CurrentHp.ToString());
        
        ActionPointsDrawer = Instantiate(CharacteristicDrawerPrefab.gameObject, CharacteristicsLayoutGroup.transform)
            .GetComponent<CharacteristicDrawer>();
        ActionPointsDrawer.Init(actionPointsSprite, unitToInit.CurrentActionPoints.ToString());
        
        UnitName.text = unitToInit.UnitName;
    }

    public void SetUnitAsExecutor(bool isExecutor)
    {
        unitIsExecutorImage.gameObject.SetActive(isExecutor);
    }

    public void ReDrawAvailability(bool available)
    {
        ifAvailablePanel.gameObject.SetActive(available);
    }
    
    public void ReDrawActivity(bool isActive)
    {
        ifInactivePanel.gameObject.SetActive(!isActive);
        if (!isActive)
        {
            SetUnitAsExecutor(false);
        }
    }

    public void ReDrawCanBlock(bool canBlock)
    {
        canBlockSprite.gameObject.SetActive(canBlock);
    }
    
    public void ReDrawCharacteristics()
    {
        if( MeleeAttackDrawer != null)
        {
            MeleeAttackDrawer.ReDraw(currentUnit.GetDamage().ToString());
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
        if (ActionPointsDrawer != null)
        {
            ActionPointsDrawer.ReDraw(currentUnit.CurrentActionPoints.ToString());
        }
    }
}
