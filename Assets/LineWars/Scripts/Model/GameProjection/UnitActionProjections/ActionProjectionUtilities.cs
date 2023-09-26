using LineWars.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public static class ActionProjectionUtilities 
    {
        public static void IsUnitValidForAction<T>(IReadOnlyGameProjection projection, 
            T actionProjection, ComponentUnit unit) where T : UnitActionProjection
        {
            var readOnlyUnit = projection.GraphProjection.AllUnits[unit];

            if (!readOnlyUnit.ActionProjections.Any(action => action is T))
                throw new ArgumentException($"This Unit doesn't contain {typeof(T)}!");

            if (!readOnlyUnit.TryGetReadOnlyAction(out T action1) || action1 != actionProjection)
                throw new ArgumentException("Invalid Unit!");
        }

        public static void CyclePhase(GameProjection projection, PhaseOrderData order)
        {
            //проверить может ли кто-то выполнить текущую фазу
            //если нет, то перключить фазу
            //repeat step 1
            var thisPhaseIndex = order.Order.FindIndex(projection.CurrentPhase);
            var phaseCount = 0;
            while (!IsPhaseRunnable(order.Order[thisPhaseIndex]) && phaseCount <= order.Order.Count)
            {
                thisPhaseIndex = (thisPhaseIndex + 1) % order.Order.Count;
                phaseCount++;
                if (order.Order[thisPhaseIndex] == PhaseType.Replenish)
                {
                    SimulateReplenish(projection);
                }
            }
            
            //выбрать нового игрока

            bool IsPhaseRunnable(PhaseType phase)
            {
                if (phase == PhaseType.Replenish) return false;
                foreach(var actor in projection.PhaseManager.Actors)
                {
                    if (actor is not BasePlayer player) continue;
                    foreach(var owned in player.OwnedObjects)
                    {
                        if(owned is not ComponentUnit unit) continue;
                        var potentialExecutors = player.PhaseExecutorsData.PhaseToUnits[phase];
                        if(!potentialExecutors.Contains(unit.Type)) continue;
                        if (projection.GraphProjection.AllUnits[unit].CurrentActionPoints <= 0) continue;
                        return true;
                    }
                }
                return false; 
            }
        }

        public static void SimulateReplenish(GameProjection projection)
        {
            projection.CurrentPhase = PhaseType.Replenish;
            foreach(var unit in projection.Graph.Units.Values)
            {
                unit.CurrentActionPoints = unit.Unit.InitialActionPoints;
            }
        }
        
        
    }
}
