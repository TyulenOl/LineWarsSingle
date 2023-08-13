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
                    return IsCanAttack(edge);
                case Unit unit:
                    return IsCanAttack(unit);
            }

            return false;
        }

        public void Attack(Unit enemy);
        public bool IsCanAttack(Unit unit);
        
        public void Attack(Edge edge);
        public bool IsCanAttack(Edge edge);

    }
}