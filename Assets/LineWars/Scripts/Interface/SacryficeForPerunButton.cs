using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.LineWars.Scripts.Interface;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class SacryficeForPerunButton : ActionButtonLogic
    {
        protected override void OnClick()
        {
            CommandsManager.Instance.SelectCurrentCommand(CommandType.SacrificePerun);
        }
    }
}
