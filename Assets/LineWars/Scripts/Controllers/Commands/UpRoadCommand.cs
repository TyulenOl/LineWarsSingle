using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class UpRoadCommand: ICommand
    {
        private readonly Engineer engineer;
        private readonly Edge edge;

        public UpRoadCommand([NotNull] Engineer engineer, [NotNull] Edge edge)
        {
            this.engineer = engineer ? engineer : throw new ArgumentNullException(nameof(engineer));
            this.edge = edge ? edge : throw new ArgumentNullException(nameof(edge));
        }

        public void Execute()
        {
            engineer.UpRoad(edge);
        }

        public bool CanExecute()
        {
           return engineer.CanUpRoad(edge);
        }

        public string GetLog()
        {
            return $"Инженер {engineer.gameObject.name} улучшил дорогу {edge.name}";
        }
    }
}