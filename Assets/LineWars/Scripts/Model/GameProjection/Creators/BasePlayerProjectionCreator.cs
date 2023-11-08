using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace LineWars.Model
{
    public abstract class BasePlayerProjectionCreator
    {
        public static BasePlayerProjection FromMono(BasePlayer original, 
            IReadOnlyCollection<OwnedProjection> ownedObjects = null, NodeProjection playerBase = null,
            GameProjection gameProjection = null)
        {
            var newPlayer = new BasePlayerProjection();

            newPlayer.Id = original.Id;
            newPlayer.Original = original;
            newPlayer.Base = playerBase;
            newPlayer.Rules = original.Rules;
            newPlayer.Income = original.Income;
            newPlayer.CurrentMoney = original.CurrentMoney;
            newPlayer.PhaseExecutorsData = original.PhaseExecutorsData;
            newPlayer.EconomicLogic = original.EconomicLogic;
            newPlayer.Game = gameProjection;
            if (ownedObjects != null)
                newPlayer.OwnedObjects = new HashSet<OwnedProjection>(ownedObjects);
            else
                newPlayer.OwnedObjects = new HashSet<OwnedProjection>();

            return newPlayer;
        }

        public static BasePlayerProjection FromProjection
            (IReadOnlyBasePlayerProjection playerProjection, 
            IReadOnlyCollection<OwnedProjection> ownedObjects = null, NodeProjection playerBase = null,
            GameProjection gameProjection = null)
        {
            var newPlayer = new BasePlayerProjection();

            newPlayer.Id = playerProjection.Id;
            newPlayer.Original = playerProjection.Original;
            newPlayer.Base = playerBase;
            newPlayer.Rules = playerProjection.Rules;
            newPlayer.Income = playerProjection.Income;
            newPlayer.CurrentMoney = playerProjection.CurrentMoney;
            newPlayer.PhaseExecutorsData = playerProjection.PhaseExecutorsData;
            newPlayer.EconomicLogic = playerProjection.EconomicLogic;
            newPlayer.Game = gameProjection;
            if (ownedObjects != null)
                newPlayer.OwnedObjects = new HashSet<OwnedProjection>(ownedObjects);
            else
                newPlayer.OwnedObjects = new HashSet<OwnedProjection>();

            return newPlayer;
        }
    }
}
