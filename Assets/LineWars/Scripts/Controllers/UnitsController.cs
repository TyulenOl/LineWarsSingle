using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class UnitsController : MonoBehaviour
    {
        public static UnitsController Instance { get; private set; }

        private Invoker invoker;

        private void Awake()
        {
            invoker = new Invoker();
            Instance = this;
        }

        // public void SpawnUnit([NotNull] Point point, [NotNull] Player owner, UnitType unitType)
        // {
        //     if (point == null) throw new ArgumentNullException(nameof(point));
        //     if (owner == null) throw new ArgumentNullException(nameof(owner));
        //     
        //     var prefab = owner.GetUnitPrefab(unitType);
        //     var instance = Instantiate(prefab);
        //     
        //
        //     
        //     var unit = instance.GetComponent<Unit>();
        //     point.AddUnitToVacantPosition(unit);
        // }
        //

        public void Action([NotNull] Player owner, IExecutor executor, ITarget target)
        {
            Debug.Log(executor);
            Debug.Log(target);
            /*
            //максимально абстрактно
            if (!owner.IsMyOwn(executor))
            {
                return;
            }

            switch (executor)
            {
                
                case IAttackerVisitor attacker
                    when target is IAlive alive && attacker.IsCanAttack(alive):
                    invoker.Execute(new AttackCommand(attacker, alive));
                    break;
                case IMovable movable 
                     when target is Node node && movable.IsCanMoveTo(node):
                    invoker.Execute(new MoveCommand(movable,movable.Node, node));
                    break;
            }
            */
        }
        
    }
}