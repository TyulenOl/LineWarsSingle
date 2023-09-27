namespace LineWars.Model
{
    public class DistanceAttackAction: AttackAction
    {
        public uint Distance { get; private set; }
        
        public DistanceAttackAction(IUnit unit, DistanceUnitAttackAction data) : base(unit, data)
        {
        }
        
        public override bool CanAttackForm(INode node, IUnit enemy, bool ignoreActionPointsCondition = false)
        {
            return !AttackLocked
                   && Damage > 0
                   && enemy.Owner != MyUnit.Owner
                   && Graph.FindShortestPath(node, enemy.Node).Count - 1 <= Distance
                   && (ignoreActionPointsCondition || ActionPointsCondition());
        }

        public override void Attack(IUnit enemy)
        {
            DistanceAttack(enemy, Damage);
            CompleteAndAutoModify();
        }

        protected void DistanceAttack(IAlive enemy, int damage)
        {
            enemy.TakeDamage(new Hit(damage, MyUnit, enemy, IsPenetratingDamage, true));
        }
        
        public override uint GetPossibleMaxRadius() => Distance;
        public override CommandType GetMyCommandType() => CommandType.Fire;
    }
}