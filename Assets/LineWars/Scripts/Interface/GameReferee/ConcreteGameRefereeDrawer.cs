using System;
using UnityEngine;

namespace LineWars.Interface
{
    public abstract class ConcreteGameRefereeDrawer: MonoBehaviour
    {
        public abstract Type GameRefereeType { get; }
        public abstract void Show(GameReferee gameReferee);
        public abstract void Hide();
    }
}