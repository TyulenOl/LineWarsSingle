using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LineWars;

namespace LineWars.Interface
{
    public class UnitPartDrawer : MonoBehaviour
    {
        [SerializeField] private TMP_Text UnitName;
        [SerializeField] private SpriteRenderer ifInactivePanel;
        [SerializeField] private SpriteRenderer ifAvailablePanel;
        [SerializeField] private SpriteRenderer unitIsExecutorImage;
        [SerializeField] private SpriteRenderer canBlockSprite;
        [field: SerializeField] public SpriteRenderer targetSprite { get; private set; }

        [SerializeField] private ProgressBarV2 healthProgressBar;
        [SerializeField] private ProgressBarV2 armorProgressBar;

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
            UnitName.text = unitToInit.UnitName;
            healthProgressBar.SetMaxValue(unitToInit.MaxHp);
            armorProgressBar.SetMaxValue(unitToInit.MaxArmor);
            healthProgressBar.SetValue(unitToInit.CurrentHp);
            armorProgressBar.SetValue(unitToInit.CurrentArmor);
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
            healthProgressBar.SetValue(currentUnit.CurrentHp);
            armorProgressBar.SetValue(currentUnit.CurrentArmor);
        }
    }
}