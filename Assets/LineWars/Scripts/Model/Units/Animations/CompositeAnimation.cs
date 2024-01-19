using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class CompositeAnimation : UnitAnimation
    {
        [SerializeField] private UnitAnimation mainAnimation;
        [SerializeField] private List<UnitStoppingAnimation> additionalsAnimations;

        public override void Execute(AnimationContext context)
        {
            StartAnimation();
            mainAnimation.Ended.AddListener(OnAnimationEnd);
            mainAnimation.Execute(context);
            foreach(var animation in additionalsAnimations)
            {
                animation.Execute(context);
            }
        }

        private void OnAnimationEnd(UnitAnimation animation)
        {
            mainAnimation.Ended.RemoveListener(OnAnimationEnd);
            foreach (var anim in additionalsAnimations)
                anim.Stop();
            EndAnimation();
        }
    }
}
