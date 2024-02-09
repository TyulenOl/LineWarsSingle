using System.Collections.Generic;
using UnityEngine;
using LineWars.Model;

namespace LineWars.Interface
{
    public class EffectsOrganizer : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField] private EffectDrawer drawerPrefab;
        private List<EffectDrawer> currentDrawers = new();

        private void Start()
        {
            unit.EffectAdded.AddListener(Redraw);
            unit.EffectRemoved.AddListener(Redraw);
            unit.EffectStacked.AddListener(Redraw);
            Redraw(unit, null);
        }

        private void OnDestroy()
        {
            unit.EffectAdded.RemoveListener(Redraw);
            unit.EffectRemoved.RemoveListener(Redraw);
            unit.EffectStacked.RemoveListener(Redraw);
        }

        private void Redraw(Unit _, Effect<Node, Edge, Unit> _1)
        {
            DeleteDrawers();
            foreach (var effect in unit.Effects)
            {
                var drawer = Instantiate(drawerPrefab, transform);
                drawer.DrawEffect(effect);
                currentDrawers.Add(drawer);
            }
        }

        private void DeleteDrawers()
        {
            var drawersList = new List<EffectDrawer>(currentDrawers);
            foreach (var drawer in drawersList)
            {
                Destroy(drawer.gameObject);
            }
            currentDrawers.Clear();
        }
    }
}
