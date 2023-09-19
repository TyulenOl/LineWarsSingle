using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class UnitUpRoadCommand: ICommand
    {
        private readonly ComponentUnit.BuildAction buildAction;
        private readonly ComponentUnit engineer;
        private readonly Edge edge;

        public UnitUpRoadCommand([NotNull] ComponentUnit engineer, [NotNull] Edge edge)
        {
            this.engineer = engineer ? engineer : throw new ArgumentNullException(nameof(engineer));
            this.edge = edge ? edge : throw new ArgumentNullException(nameof(edge));
            
            buildAction = engineer.TryGetExecutorAction<ComponentUnit.BuildAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ComponentUnit)} does not contain {nameof(ComponentUnit.BuildAction)}");
        }

        public UnitUpRoadCommand([NotNull] ComponentUnit.BuildAction buildAction, [NotNull] Edge edge)
        {
            this.buildAction = buildAction ?? throw new ArgumentNullException(nameof(buildAction));
            this.edge = edge ? edge : throw new ArgumentNullException(nameof(edge));

            engineer = this.buildAction.MyUnit;
        }

        public void Execute()
        {
            buildAction.UpRoad(edge);
        }

        public bool CanExecute()
        {
           return buildAction.CanUpRoad(edge);
        }

        public string GetLog()
        {
            return $"Инженер {engineer.gameObject.name} улучшил дорогу {edge.name}";
        }
    }
}