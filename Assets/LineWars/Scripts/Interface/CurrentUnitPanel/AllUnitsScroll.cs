using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class AllUnitsScroll : MonoBehaviour
    {
        [SerializeField] private UnitTracker unitTrackerPrefab;
        [SerializeField] private LayoutGroup unitTrackersLayoutGroup;
        
        public void Init()
        {
            Player.LocalPlayer.OwnedAdded += OnOwnedAdded;
            var allUnits = Player.LocalPlayer.MyUnits;
            foreach (var unit in allUnits)
            {
                AddTracker(unit);
            }
        }

        private void AddTracker(Unit unit)
        {
            var instance = Instantiate(unitTrackerPrefab, unitTrackersLayoutGroup.transform);
            instance.Init(unit);
        }
        
        private void OnOwnedAdded(Owned owned)
        {
            if(owned is Unit unit)
                AddTracker(unit);
        }
    }
}