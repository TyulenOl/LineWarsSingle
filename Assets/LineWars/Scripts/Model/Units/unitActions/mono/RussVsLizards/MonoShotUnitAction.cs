﻿using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public class MonoShotUnitAction :
        MonoUnitAction<ShotUnitAction<Node, Edge, Unit>>,
        IShotUnitAction<Node, Edge, Unit>
    {

        [SerializeField] private SFXList ThrowRusSounds;
        [SerializeField] private SFXList ThrowLizardSounds;
        [SerializeField] private SFXList FallSoundsList;

        private IDJ dj;
        
        protected override bool NeedAutoComplete => false;
        public bool IsAvailable(Unit target1) => Action.IsAvailable(target1);

        public bool IsAvailable(Unit target1, Node target2) => Action.IsAvailable(target1, target2);

        private void Awake()
        {
            dj = new RandomDJ(1);
        }

        public void Execute(Unit unitTarget, Node nodeTarget)
        {
            if(unitTarget.TryGetComponent(out AnimationResponses responses))
                TryExecuteWithAnimations(unitTarget, nodeTarget, responses);
            else
                ExecuteInstant(unitTarget, nodeTarget);
        }

        private void TryExecuteWithAnimations(Unit unitTarget, 
                Node nodeTarget,
                AnimationResponses responses)
        {
            if(!responses.CanRespond(AnimationResponseType.ComeTo)
                || !responses.CanRespond(AnimationResponseType.Throw))
            {
                ExecuteInstant(unitTarget, nodeTarget);
                return;
            }
            var animContext = new AnimationContext()
            {
                TargetNode = Executor.Node,
                TargetUnit = Executor
            };

            if (unitTarget.Owner == Executor.Owner)
            {
                Executor.PlaySfx(dj.GetSound(ThrowRusSounds));
            }
            else
            {
                Executor.PlaySfx(dj.GetSound(ThrowLizardSounds));
            }
            
            var walkAnimation = responses.Respond(AnimationResponseType.ComeTo, animContext);
            walkAnimation.Ended.AddListener(OnWalkEnded);
            
            void OnWalkEnded(UnitAnimation animation)
            {
                animation.Ended.RemoveListener(OnWalkEnded);
                var throwContext = new AnimationContext()
                {
                    TargetNode = nodeTarget,
                    TargetUnit = null

                };
                var throwAnimation = responses.Respond(AnimationResponseType.Throw, throwContext);
                throwAnimation.Ended.AddListener(OnThrowEnded);
            }
            
            void OnThrowEnded(UnitAnimation animation)
            {
                animation.Ended.RemoveListener(OnThrowEnded);
                Action.Execute(unitTarget, nodeTarget);
                Complete();
                Player.LocalPlayer.RecalculateVisibility();
                Executor.PlaySfx(dj.GetSound(FallSoundsList));
            }
        }

        private void ExecuteInstant(Unit unitTarget, Node nodeTarget)
        {
            unitTarget.MovementLogic.MoveTo(nodeTarget.transform.position);
            Action.Execute(unitTarget, nodeTarget);
            Player.LocalPlayer.RecalculateVisibility();
            Complete();
        }

        protected override ShotUnitAction<Node, Edge, Unit> GetAction()
        {
            return new ShotUnitAction<Node, Edge, Unit>(Executor);
        }

        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) =>
            visitor.Visit(this);
    }
}