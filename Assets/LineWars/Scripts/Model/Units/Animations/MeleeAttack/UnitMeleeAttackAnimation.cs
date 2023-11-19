using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    [RequireComponent(typeof(MonoMeleeAttackAction))]
    public partial class UnitMeleeAttackAnimation : UnitAnimation
    {
        private MonoMeleeAttackAction action;
        private Unit currentTarget;

        private StateMachine stateMachine;
        private MeleeMoveToUnitState moveToState;
        private UnitMeleeAttackReturnState returnState;
        private State idleState;

        public UnityEvent<UnitMeleeAttackAnimation> Attacked;
        private Vector2 startPosition;
        private Node targetNode;

        private void Awake()
        {
            InitializeStateMachine();
        }

        protected override void Start()
        {
            base.Start();
            action = GetComponent<MonoMeleeAttackAction>();
        }

        private void Update()
        {
            stateMachine.OnLogic();
        }

        private void FixedUpdate()
        {
            stateMachine.OnPhysics();
        }

        private void InitializeStateMachine()
        {
            stateMachine = new();
            moveToState = new(this);
            returnState = new(this);
            idleState = new();
        }

        public override void Execute(AnimationContext context)
        {
            currentTarget = context.TargetUnit;
            targetNode = context.TargetNode;
            stateMachine.SetState(moveToState);
        }
    }
}
