using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/SpawnBlessing")]
    public class SpawnBlessing: BaseBlessing
    {
        [SerializeField] private List<Unit> unitPrefabsToSpawn;
        
        public override event Action Completed;
        public override bool CanExecute()
        {
            return Player.InitialSpawns.Any(node => unitPrefabsToSpawn.Any(node.UnitIsFit));
        }

        public override void Execute()
        {
            var tempList = unitPrefabsToSpawn.ToList();
            
            foreach (var spawn in Player.InitialSpawns)
            {
                var unitsToSpawn = tempList
                    .Where(unit => spawn.UnitIsFit(unit))
                    .ToArray();

                foreach (var unit in unitsToSpawn)
                {
                    Player.SpawnUnit(spawn, unit);
                    tempList.Remove(unit);
                    if (!spawn.AnyIsFree)
                        break;
                }
            }
            
            Completed?.Invoke();
        }

        protected override string DefaultName => "Благословление \"Призыв\"";
        protected override string DefaultDescription => $"Призывает {string.Join(", ", unitPrefabsToSpawn.Select(x => x.UnitName))}" +
                                                        " на ваших изначальных спавнах";
    }
}