using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public sealed class Unit : Owned,
        IUnit<Node, Edge, Unit>, 
        IMonoExecutor,
        IMonoTarget
    {
        [Header("Units Settings")] 
        [SerializeField, ReadOnlyInspector] private int index;

        [SerializeField] private string unitName;
        [SerializeField][TextArea] private string unitDescription;
        
        [SerializeField, Min(0)] private int initialPower;
        [SerializeField, Min(0)] private int maxHp;
        private int maxArmor = 100;
        [SerializeField, Min(0)] private int visibility;
        [field: SerializeField] public Sprite Sprite { get; private set; }
        
        [SerializeField] private UnitType unitType;
        [SerializeField] private UnitSize unitSize;
        [SerializeField] private LineType movementLineType;
        [SerializeField] private CommandPriorityData priorityData;

        [Header("Sounds")] 
        [SerializeField] [Min(0)] private SFXList HpHealedSounds;
        [SerializeField] [Min(0)] private SFXList HpDamagedSounds;
        private IDJ dj;
        
        [Header("Actions Settings")] 
        [SerializeField] [Min(0)] private int maxActionPoints;
        [SerializeField] private List<EffectInitializer> effectInitializers;

        [Header("DEBUG")] 
        [SerializeField, ReadOnlyInspector] private Node myNode;

        [SerializeField, ReadOnlyInspector] private UnitDirection unitDirection;
        [SerializeField, ReadOnlyInspector] private int currentHp;
        [SerializeField, ReadOnlyInspector] private int currentArmor;
        [SerializeField, ReadOnlyInspector] private int currentActionPoints;
        [SerializeField, ReadOnlyInspector] private int currentPower;


        [field: Header("Events")]
        [field: SerializeField] public UnityEvent<UnitSize, UnitDirection> UnitDirectionChange { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> HpChanged { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ArmorChanged { get; private set; }
        [field: SerializeField] public UnityEvent<Unit> Died { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ActionPointsChanged { get; private set; }

        public event Action AnyActionCompleted;
        public event Action ExecutorDestroyed;
        public event Action<Unit, Node, Node> UnitNodeChanged; //++
        public event Action<Unit, int, int> UnitHPChanged; //++
        public event Action<Unit, int, int> UnitActionPointsChanged; //++
        public event Action<Unit, int, int> UnitPowerChanged; //++
        public event Action<Unit, int, int> UnitArmorChanged; //++
        public event Action<Unit> UnitReplenished; //++

        private List<Effect<Node, Edge, Unit>> effects = new(); 

        private Dictionary<CommandType, IMonoUnitAction<UnitAction<Node, Edge, Unit>>> monoActionsDictionary;
        public IEnumerable<IMonoUnitAction<UnitAction<Node, Edge, Unit>>> MonoActions => monoActionsDictionary.Values;
        public IReadOnlyList<Effect<Node, Edge, Unit>> Effects => effects;
        public uint MaxPossibleActionRadius { get; private set; }

        #region Properties
        public int Id => index;
        public bool IsVisible => Node.IsVisible;
        public string UnitName => unitName;

        public int InitialPower
        {
            get => initialPower;
            set => initialPower = Mathf.Max(value, 0);
        }

        public int CurrentPower
        {
            get => currentPower;
            set
            {
                var prevPower = currentPower;
                currentPower = Mathf.Max(value, 0);
                UnitPowerChanged?.Invoke(this, prevPower, currentPower);
            }
        }
        
        public int MaxActionPoints
        {
            get => maxActionPoints;
            set
            {
                maxActionPoints = Mathf.Max(value, 0);
                CurrentActionPoints = Mathf.Min(currentActionPoints, maxActionPoints);
            }
        }

        public int CurrentActionPoints
        {
            get => currentActionPoints;
            set
            {
                var previousValue = currentActionPoints;
                currentActionPoints = Mathf.Clamp(value, 0, MaxActionPoints);
                UnitActionPointsChanged?.Invoke(this, previousValue, currentActionPoints);
                ActionPointsChanged.Invoke(previousValue, currentActionPoints);
            }
        }

        public int MaxHp
        {
            get => maxHp;
            set
            {
                maxHp = Mathf.Max(value, 0);
                CurrentHp = Mathf.Min(currentHp, maxHp);
            }
        }

        public int CurrentHp
        {
            get => currentHp;
            set
            {
                var before = currentHp;
                currentHp = Mathf.Clamp(value, 0, maxHp);
                if (before == currentHp) return;
                HpChanged.Invoke(before, currentHp);
                UnitHPChanged?.Invoke(this, before, currentHp);
                SfxManager.Instance.Play(before < currentHp ? dj.GetSound(HpHealedSounds) : dj.GetSound(HpDamagedSounds));

                if (currentHp == 0)
                {
                    OnDied();
                    Died.Invoke(this);
                    ExecutorDestroyed?.Invoke();
                }
            }
        }

        public int MaxArmor
        {
            get => maxArmor;
            set => maxArmor = Mathf.Max(value, 0);
        }

        public int CurrentArmor
        {
            get => currentArmor;
            set
            {
                var before = currentArmor;
                currentArmor = Mathf.Clamp(value, 0, maxArmor);
                if(before == currentArmor) return;
                ArmorChanged.Invoke(before, currentArmor);
                UnitArmorChanged?.Invoke(this, before, currentArmor);
            }
        }

        public string UnitDescription => unitDescription;
        public UnitType Type => unitType;

        public UnitDirection UnitDirection
        {
            get => unitDirection;
            set
            {
                unitDirection = value;
                UnitDirectionChange?.Invoke(Size, unitDirection);
            }
        }

        public int Visibility
        {
            get => visibility;
            set => visibility = value;
        }

        public UnitSize Size => unitSize;
        public LineType MovementLineType => movementLineType;

        public Node Node
        {
            get => myNode;
            set
            {
                if (value == null)
                    throw new ArgumentException();
                var prevNode = myNode;
                myNode = value;
                UnitNodeChanged?.Invoke(this, prevNode, myNode);
            }
        }

        public CommandPriorityData CommandPriorityData => priorityData;
        public bool CanDoAnyAction => currentActionPoints > 0;

        public bool IsDied => CurrentHp <= 0;

        #endregion
        
        private void Awake()
        {
            dj = new RandomDJ(0.5f);
            
            currentHp = maxHp;
            currentArmor = 0;
            currentActionPoints = maxActionPoints;
            currentPower = initialPower;

            InitialiseAllActions();
            index = SingleGameRoot.Instance.AllUnits.Add(this);
            void InitialiseAllActions()
            {
                var serializeActions = gameObject.GetComponents<Component>()
                    .OfType<IMonoUnitAction<UnitAction<Node, Edge, Unit>>>()
                    .OrderByDescending(x => x.InitializePriority)
                    .ToArray();

                monoActionsDictionary = new Dictionary<CommandType, IMonoUnitAction<UnitAction<Node, Edge, Unit>>>(serializeActions.Length);
                foreach (var serializeAction in serializeActions)
                {
                    serializeAction.Initialize();
                    monoActionsDictionary.Add(serializeAction.CommandType, serializeAction);
                    serializeAction.ActionCompleted += () =>
                    {
                        AnyActionCompleted?.Invoke();
                    };
                }

                MaxPossibleActionRadius = MonoActions.Max(x => x.GetPossibleMaxRadius());
            }
        }

        public void Initialize(Node node, UnitDirection direction)
        {
            Node = node;
            UnitDirection = direction;
            InitializeAllEffects();
        }

        private void InitializeAllEffects()
        {
            foreach (var effectInit in effectInitializers)
            {
                var newEffect = effectInit.GetEffect(this);
                AddEffect(newEffect);
            }
        }

        public IEnumerable<IUnitAction<Node, Edge, Unit>> Actions => MonoActions;
        IEnumerable<IExecutorAction> IExecutor.Actions => Actions;

        public T GetAction<T>() where T : IUnitAction<Node, Edge, Unit>
            => MonoActions.OfType<T>().FirstOrDefault();

        public bool TryGetAction<T>(out T action) where T : IUnitAction<Node, Edge, Unit>
        {
            action = GetAction<T>();
            return action != null;
        }

        private void OnDied()
        {
            if (unitSize == UnitSize.Large)
            {
                myNode.LeftUnit = null;
                myNode.RightUnit = null;
            }
            else if (UnitDirection == UnitDirection.Left)
            {
                myNode.LeftUnit = null;
            }
            else
            {
                myNode.RightUnit = null;
            }

            Owner.RemoveOwned(this);
            SingleGameRoot.Instance.AllUnits.Remove(this);
            if(!TryGetComponent(out AnimationResponses responses) || responses.CurrentDeathAnimation == null) 
                Destroy(gameObject);
            else
            {
                responses.CurrentDeathAnimation.Ended.AddListener(DestroyOnAnimationEnd);
                var animContext = new AnimationContext()
                {
                    TargetNode = myNode,
                    TargetUnit = this
                };
                responses.PlayDeathAnimation();
            }
        }

        private void DestroyOnAnimationEnd(UnitAnimation animation)
        {
            animation.Ended.RemoveListener(DestroyOnAnimationEnd);
            Destroy(gameObject);
        }

        protected override void OnReplenish()
        {
            CurrentActionPoints = maxActionPoints;
            CurrentArmor = 0;
            
            foreach (var unitAction in MonoActions)
                unitAction.OnReplenish();

            UnitReplenished?.Invoke(this);
        }
        public void AddEffect(Effect<Node, Edge, Unit> effect)
        {
            if(effect.TargetUnit != this)
            {
                Debug.LogError("Adding effect with other owner!");
                return;
            }
            effect.ExecuteOnEnter();
            effects.Add(effect);
        }

        public void DeleteEffect(Effect<Node, Edge, Unit> effect)
        {
            effect.ExecuteOnExit();
            effects.Remove(effect);
        }

        public T Accept<T>(IMonoExecutorVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

    }
}