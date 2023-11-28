using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
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

        protected void Start()
        {
            action = ownerUnit.GetComponent<MonoMeleeAttackAction>();
            if (action == null)
                Debug.LogWarning("Melee Attack Animation without Melee Attack Action!");
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
