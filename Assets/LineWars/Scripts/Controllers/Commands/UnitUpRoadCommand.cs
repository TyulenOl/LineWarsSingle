using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class UnitUpRoadCommand: ICommand
    {
        private readonly ComponentUnit engineer;
        private readonly Edge edge;

        public UnitUpRoadCommand([NotNull] ComponentUnit engineer, [NotNull] Edge edge)
        {
            this.engineer = engineer ? engineer : throw new ArgumentNullException(nameof(engineer));
            this.edge = edge ? edge : throw new ArgumentNullException(nameof(edge));
        }

        public void Execute()
        {
            engineer.GetExecutorAction<ComponentUnit.BuildAction>().UpRoad(edge);
        }

        public bool CanExecute()
        {
           return engineer.TryGetExecutorAction<ComponentUnit.BuildAction>(out var action) &&
                  action.CanUpRoad(edge);
        }

        public string GetLog()
        {
            return $"Инженер {engineer.gameObject.name} улучшил дорогу {edge.name}";
        }
    }
}