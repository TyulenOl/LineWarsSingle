using UnityEngine;
using UnityEngine.UI;

namespace LineWars.LineWars.Scripts.Interface.ProgressBar
{
    public class UnitCharacteristicProgressBar : ProgressBarV2
    {
        [SerializeField] private Image borderImage;

        private Sprite GetBorderImageByMaxValue(int value)
        {
            var borderSprite = Resources.Load($"UI/Sorokin/Panels/ProgressBars/Bgs/Border{value}");
            if (borderSprite == null)
            {
                borderSprite = Resources.Load("Assets/LineWars/Resources/UI/Sorokin/Panels/ProgressBars/Bgs/BorderDefault");
                //Debug.LogWarning($"There is no border of progress bar of amount {value}");
            }

            return borderSprite as Sprite;
        }
        
        public override void SetMaxValue(float value)
        {
            base.SetMaxValue(value);
            var newBorderSprite = GetBorderImageByMaxValue((int)value);
            if (newBorderSprite != null)
                borderImage.sprite = newBorderSprite;
        }


    }
}