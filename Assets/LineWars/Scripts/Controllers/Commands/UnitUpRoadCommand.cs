using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class UnitUpRoadCommand: ICommand
    {
        private readonly ModelComponentUnit.BuildAction buildAction;
        private readonly ModelComponentUnit engineer;
        private readonly ModelEdge edge;

        public UnitUpRoadCommand([NotNull] ModelComponentUnit engineer, [NotNull] ModelEdge edge)
        {
            this.engineer = engineer ?? throw new ArgumentNullException(nameof(engineer));
            this.edge = edge ?? throw new ArgumentNullException(nameof(edge));
            
            buildAction = engineer.TryGetExecutorAction<ModelComponentUnit.BuildAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ModelComponentUnit)} does not contain {nameof(ModelComponentUnit.BuildAction)}");
        }

        public UnitUpRoadCommand([NotNull] ModelComponentUnit.BuildAction buildAction, [NotNull] ModelEdge edge)
        {
            this.buildAction = buildAction ?? throw new ArgumentNullException(nameof(buildAction));
            this.edge = edge ?? throw new ArgumentNullException(nameof(edge));

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
            return $"Инженер {engineer} улучшил дорогу {edge}";
        }
    }
}