using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public class MonoSacrificeForPerunAction :
        MonoUnitAction<SacrificeForPerunAction<Node, Edge, Unit>>,
        ISacrificeForPerunAction<Node, Edge, Unit>
    {
        [SerializeField] private PerunAnimation perunAnimation;
        
        [SerializeField] protected SFXData sacrSfx;

        [SerializeField] protected SFXList sfxList;

        private IDJ DJ;

        private void Awake()
        {
            DJ = new RandomDJ(1);
        }

        public bool CanSacrifice(Node node) => Action.CanSacrifice(node);

        public void Sacrifice(Node node)
        {
            Executor.PlaySfx(DJ.GetSound(sfxList));
            
            if(perunAnimation == null)
            {
                SacrificeInstant(node);
                return;
            }

            var animContext = new AnimationContext()
            {
                TargetNode = node
            };

            perunAnimation.Ended.AddListener(SacrificeAfterAnim);
            perunAnimation.Execute(animContext);

            void SacrificeAfterAnim(UnitAnimation _)
            {
                perunAnimation.Ended.RemoveListener(SacrificeAfterAnim);
                SacrificeInstant(node);
                
            }
        }

        private void SacrificeInstant(Node node)
        {
            Action.Sacrifice(node);
            Player.LocalPlayer.AddAdditionalVisibleNode(node);
            Player.LocalPlayer.RecalculateVisibility();
            Executor.PlaySfx(sacrSfx);
        }

        protected override SacrificeForPerunAction<Node, Edge, Unit> GetAction()
        {
            return new SacrificeForPerunAction<Node, Edge, Unit>(Executor);
        }
        
        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}