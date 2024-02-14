using System.Collections.Generic;
using UnityEngine;
using LineWars.Model;

namespace LineWars.Interface
{
    public class EffectsOrganizer : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField] private EffectDrawer drawerPrefab;
        [SerializeField] private List<EffectType> effectExceptions;
        private List<EffectDrawer> currentDrawers = new();

        private void Start()
        {
            unit.EffectAdded.AddListener(Redraw);
            unit.EffectAdded.AddListener(OnEffectAdded);

            unit.EffectRemoved.AddListener(Redraw);
            unit.EffectRemoved.AddListener(OnEffectRemoved);

            unit.EffectStacked.AddListener(Redraw);
            Redraw(unit, null);

            foreach(var effect in unit.Effects)
            {
                effect.ActiveChanged += OnActiveChanged;
            }
        }

        private void OnDestroy()
        {
            unit.EffectAdded.RemoveListener(Redraw);
            unit.EffectAdded.RemoveListener(OnEffectAdded);

            unit.EffectRemoved.RemoveListener(Redraw);
            unit.EffectRemoved.RemoveListener(OnEffectRemoved);

            unit.EffectStacked.RemoveListener(Redraw);
        }

        private void Redraw(Unit _, Effect<Node, Edge, Unit> _1)
        {
            DeleteDrawers();
            foreach (var effect in unit.Effects)
            {
                DrawEffect(effect);
            }
        }

        private void DrawEffect(Effect<Node, Edge, Unit> effect)
        {
            if (effectExceptions.Contains(effect.EffectType))
                return;
            if (!effect.IsActive)
                return;
            var drawer = Instantiate(drawerPrefab, transform);
            drawer.DrawEffect(effect);
            currentDrawers.Add(drawer);
        }

        private void OnActiveChanged(Effect<Node, Edge, Unit> effect, bool prevValue, bool newValue)
        {
            if(prevValue == newValue) return;
            Redraw(unit, effect);
        }

        private void OnEffectAdded(Unit unit, Effect<Node, Edge, Unit> effect)
        {
            effect.ActiveChanged += OnActiveChanged;
            Redraw(unit, effect);
        }

        private void OnEffectRemoved(Unit unit, Effect<Node, Edge, Unit> effect)
        {
            effect.ActiveChanged -= OnActiveChanged;
            Redraw(unit, effect);
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
