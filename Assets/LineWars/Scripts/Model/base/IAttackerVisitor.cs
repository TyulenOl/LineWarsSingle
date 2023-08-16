using System;
using Newtonsoft.Json.Bson;

namespace LineWars.Model
{
    public interface IAttackerVisitor
    {
        public void Attack(IAlive target)
        {
            switch (target)
            {
                case Edge edge:
                    Attack(edge);
                    break;
                case Unit unit:
                    Attack(unit);
                    break;
            }
        }

        public bool CanAttack(IAlive target)
        {
            switch (target)
            {
                case Edge edge:
                    return CanAttack(edge);
                case Unit unit:
                    return CanAttack(unit);
            }

            return false;
        }

        public void Attack(Unit enemy);
        public bool CanAttack(Unit unit);
        
        public void Attack(Edge edge);
        public bool CanAttack(Edge edge);

    }
}