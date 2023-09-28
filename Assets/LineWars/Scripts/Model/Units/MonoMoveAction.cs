﻿using System;
using System.Drawing;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoMoveAction : MonoUnitAction
    {
        protected override UnitAction GetAction(ComponentUnit unit)
        {
            var moveAction = new MoveAction(unit, this);
            moveAction.Moved += node =>
            {
                if (node is MonoBehaviour mono)
                    unit.MovementLogic.MoveTo(mono.transform);
            };
            return moveAction;
        }
    }
}