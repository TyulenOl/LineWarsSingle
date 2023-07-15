using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    
    public class UnitsController: MonoBehaviour
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
        public void MoveOrAttackUnit([NotNull] Player owner, [NotNull] Unit unit, [NotNull] Point target)
        {
            if (owner.IsMyOwn(unit))
            {
                if (owner.IsMyOwn(target))
                    invoker.Execute(new MoveCommand(unit, target));
                else
                    invoker.Execute(new AttackCommand(unit, target));
            }
            else
            {
                Debug.LogError("Нельзя передвинуть юнита, потому что он вам не принадлежит!");
            }
        }
    }
}