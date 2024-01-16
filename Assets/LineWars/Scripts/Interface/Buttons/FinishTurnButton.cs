using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    public class FinishTurnButton : MonoBehaviour
    {
        public void FinishTurn() => Player.LocalPlayer.FinishTurn();
    }
}