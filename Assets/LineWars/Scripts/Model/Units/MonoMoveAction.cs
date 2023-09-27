using System;
using System.Drawing;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoMoveAction : MonoUnitAction
    {
        public override UnitAction GetAction(ComponentUnit unit) => new MoveAction(unit, this);
    }
}