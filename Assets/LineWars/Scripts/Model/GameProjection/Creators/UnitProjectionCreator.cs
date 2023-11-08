using Codice.Client.BaseCommands;
using LineWars.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public static class UnitProjectionCreator
    {
        public static UnitProjection FromProjection(IReadOnlyUnitProjection oldUnit, NodeProjection node = null)
        {
            var newUnit = new UnitProjection();
            newUnit.UnitName = oldUnit.UnitName;
            newUnit.CurrentHp = oldUnit.CurrentHp;
            newUnit.MaxHp = oldUnit.MaxHp;
            newUnit.MaxArmor = oldUnit.MaxArmor;
            newUnit.CurrentArmor = oldUnit.CurrentArmor;
            newUnit.CurrentActionPoints = oldUnit.CurrentActionPoints;
            newUnit.MaxActionPoints = oldUnit.MaxActionPoints;
            newUnit.Visibility = oldUnit.Visibility;
            newUnit.Type = oldUnit.Type;
            newUnit.Size = oldUnit.Size;
            newUnit.MovementLineType = oldUnit.MovementLineType;
            newUnit.CommandPriorityData = oldUnit.CommandPriorityData;
            newUnit.UnitDirection = oldUnit.UnitDirection;
            newUnit.Node = node;
            newUnit.Original = oldUnit.Original;
            newUnit.UnitActions = oldUnit.ActionsDictionary.Values;
            newUnit.HasId = oldUnit.HasId;
            newUnit.Id = oldUnit.Id;

            return newUnit;
        }

        public static UnitProjection FromMono(Unit original, NodeProjection node = null)
        {
            var newUnit = new UnitProjection();

            newUnit.UnitName = original.UnitName;
            newUnit.CurrentHp = original.CurrentHp;
            newUnit.MaxHp = original.MaxHp;
            newUnit.MaxArmor = original.MaxArmor;
            newUnit.CurrentArmor = original.CurrentArmor;
            newUnit.CurrentActionPoints = original.CurrentActionPoints;
            newUnit.MaxActionPoints = original.MaxActionPoints;
            newUnit.Visibility = original.Visibility;
            newUnit.Type = original.Type;
            newUnit.Size = original.Size;
            newUnit.MovementLineType = original.MovementLineType;
            newUnit.CommandPriorityData = original.CommandPriorityData;
            newUnit.UnitDirection = original.UnitDirection;
            newUnit.Node = node;
            newUnit.Original = original;
            newUnit.MonoActions = original.MonoActions;
            newUnit.HasId = true;
            newUnit.Id = original.Id;

            return newUnit;
        }
    }
}
