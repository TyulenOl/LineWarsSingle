using System.Collections.Generic;

namespace LineWars.Model
{
    public abstract class BasePlayerProjectionCreator
    {
        public static BasePlayerProjection FromMono(
            BasePlayer original, 
            IReadOnlyCollection<OwnedProjection> ownedObjects = null,
            NodeProjection playerBase = null,
            GameProjection gameProjection = null)
        {
            var newPlayer = new BasePlayerProjection
            {
                Id = original.Id,
                Original = original,
                Base = playerBase,
                Rules = original.Rules,
                Income = original.Income,
                CurrentMoney = original.CurrentMoney,
                PhaseExecutorsData = original.PhaseExecutorsData,
                EconomicLogic = original.EconomicLogic,
                Game = gameProjection
            };

            if (ownedObjects != null)
                newPlayer.OwnedObjects = new HashSet<OwnedProjection>(ownedObjects);
            else
                newPlayer.OwnedObjects = new HashSet<OwnedProjection>();

            return newPlayer;
        }

        public static BasePlayerProjection FromProjection(
            IReadOnlyBasePlayerProjection playerProjection, 
            IReadOnlyCollection<OwnedProjection> ownedObjects = null, 
            NodeProjection playerBase = null,
            GameProjection gameProjection = null)
        {
            var newPlayer = new BasePlayerProjection
            {
                Id = playerProjection.Id,
                Original = playerProjection.Original,
                Base = playerBase,
                Rules = playerProjection.Rules,
                Income = playerProjection.Income,
                CurrentMoney = playerProjection.CurrentMoney,
                PhaseExecutorsData = playerProjection.PhaseExecutorsData,
                EconomicLogic = playerProjection.EconomicLogic,
                Game = gameProjection
            };

            if (ownedObjects != null)
                newPlayer.OwnedObjects = new HashSet<OwnedProjection>(ownedObjects);
            else
                newPlayer.OwnedObjects = new HashSet<OwnedProjection>();

            return newPlayer;
        }
    }
}
