using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineWars.Model
{
    public class BuildCommand : ICommand
    {
        private readonly BuildAction buildAction;
        private readonly IUnit engineer;
        private readonly IEdge edge;

        public BuildCommand([NotNull] IUnit engineer, [NotNull] IEdge edge)
        {
            this.engineer = engineer ?? throw new ArgumentNullException(nameof(engineer));
            this.edge = edge ?? throw new ArgumentNullException(nameof(edge));

            buildAction = engineer.TryGetExecutorAction<BuildAction>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(IUnit)} does not contain {nameof(BuildAction)}");
        }

        public BuildCommand([NotNull] BuildAction buildAction, [NotNull] IEdge edge)
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
