using System.Collections.Generic;
using LineWars.Extensions.Attributes;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    public class Player : BasePlayer
    {
        public static Player LocalPlayer { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            LocalPlayer = this;
        }
    }
}