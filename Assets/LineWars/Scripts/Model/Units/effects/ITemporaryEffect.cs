﻿using System;

namespace LineWars.Model
{
    [Obsolete]
    public interface ITemporaryEffect
    {
        public int InitialRounds { get; }
        public int Rounds { get; }

        public event Action<ITemporaryEffect, int, int> RoundsChanged;
    }
}
