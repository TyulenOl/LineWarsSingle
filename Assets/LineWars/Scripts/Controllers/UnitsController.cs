using UnityEngine;

namespace LineWars.Controllers
{
    public class UnitsController: MonoBehaviour
    {
        // public static UnitsController Instance { get; private set; }
        //
        // private void Awake()
        // {
        //     Instance = this;
        // }
        //
        // public void SpawnUnit([NotNull] Point point, [NotNull] LW_Player owner, UnitType unitType)
        // {
        //     if (point == null) throw new ArgumentNullException(nameof(point));
        //     if (owner == null) throw new ArgumentNullException(nameof(owner));
        //     
        //     var prefab = owner.GetUnitPrefab(unitType);
        //     var instance = Instantiate(prefab);
        //     
        //     NetworkServer.Spawn(instance, owner.gameObject);
        //     
        //     var unit = instance.GetComponent<Unit>();
        //     unit.CmdRegisterUnit(unit);
        //     point.AddUnitToVacantPosition(unit);
        // }
        //
        // public void MoveUnit([NotNull] Unit unit, [NotNull] Point point)
        // {
        //     if (unit == null) throw new ArgumentNullException(nameof(unit));
        //     if (point == null) throw new ArgumentNullException(nameof(point));
        //     unit.MoveTo(point);
        // }
        //
        // public void Attack(IHitCreator creator, IHitHandler handler)
        // {
        //     var hit = creator.GenerateHit();
        //     handler.Accept(hit);
        // }
    }
}