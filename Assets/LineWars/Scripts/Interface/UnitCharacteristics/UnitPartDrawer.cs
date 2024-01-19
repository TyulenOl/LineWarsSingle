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

        [SerializeField] private ProgressBarV4 healthArmorProgressBar;
        [SerializeField] private ProgressBarV4 apProgressBar;

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
            healthArmorProgressBar.Init(3, currentUnit.MaxHp, 0, 0);
            apProgressBar.Init(2, currentUnit.MaxActionPoints, 0);
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
            healthArmorProgressBar.SetValue((0, currentUnit.CurrentHp), (1, currentUnit.MaxHp - currentUnit.CurrentHp), (2, currentUnit.CurrentArmor));
            apProgressBar.SetValue((0, currentUnit.CurrentActionPoints), (1, currentUnit.MaxActionPoints - currentUnit.CurrentActionPoints));
            Debug.Log(currentUnit.CurrentHp + "    " + (currentUnit.MaxHp - currentUnit.CurrentHp) + "   " + currentUnit.CurrentArmor);
            Debug.Log(currentUnit.CurrentActionPoints + "    " + (currentUnit.MaxActionPoints - currentUnit.CurrentActionPoints));
        }
    }
}