using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LineWars;
using UnityEngine.Serialization;

namespace LineWars.Interface
{
    public class UnitPartDrawer : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("UnitName")] private TMP_Text unitName;
        [SerializeField] private SpriteRenderer ifInactivePanel;
        [SerializeField] private SpriteRenderer ifAvailablePanel;
        [SerializeField] private SpriteRenderer unitIsExecutorImage;
        [SerializeField] private SpriteRenderer canBlockSprite;
        [field: SerializeField] public SpriteRenderer targetSprite { get; private set; }

        [SerializeField] private UnitProgressBar unitProgressBar;

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

        public TMP_Text UnitName => unitName;

        private void Init(Unit unitToInit)
        {
            UnitName.text = unitToInit.UnitName;
            unitProgressBar.Init(currentUnit.MaxHp, currentUnit.CurrentArmor, currentUnit.MaxActionPoints);
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
            unitProgressBar.SetValues(currentUnit.CurrentHp, currentUnit.MaxHp,
                currentUnit.CurrentArmor, currentUnit.CurrentActionPoints, currentUnit.MaxActionPoints);
        }
    }
}