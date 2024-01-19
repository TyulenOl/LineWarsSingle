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

        [SerializeField] private ProgressBarV3 healthArmorProgressBar;
        [SerializeField] private ProgressBarV3 apProgressBar;

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
            healthArmorProgressBar.SetBar(currentUnit.CurrentHp, currentUnit.CurrentArmor);
            apProgressBar.SetHp(currentUnit.CurrentActionPoints);
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
            healthArmorProgressBar.SetBar(currentUnit.CurrentHp, currentUnit.CurrentArmor);
            apProgressBar.SetHp(currentUnit.CurrentActionPoints);
        }
    }
}