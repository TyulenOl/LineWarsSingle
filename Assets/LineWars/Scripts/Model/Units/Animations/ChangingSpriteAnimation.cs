using UnityEngine;

namespace LineWars.Model
{
    public class ChangingSpriteAnimation : UnitStoppingAnimation
    {
        [SerializeField] private Sprite changedSprite;
        [SerializeField] private SpriteRenderer leftSprite;
        [SerializeField] private SpriteRenderer rightSprite;

        private Sprite oldSprite;
        private SpriteRenderer mainSprite;
        public override void Execute(AnimationContext context)
        {
            SetMainSprite();
            StartAnimation();
            oldSprite = mainSprite.sprite;
            mainSprite.sprite = changedSprite;
        }

        private void SetMainSprite()
        {
            if (ownerUnit.UnitDirection == UnitDirection.Left)
                mainSprite = leftSprite;
            else
                mainSprite = rightSprite;
        }

        public override void Stop()
        {
            mainSprite.sprite = oldSprite;
            StartAnimation();
        }
    }
}
