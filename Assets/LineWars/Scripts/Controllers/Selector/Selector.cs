using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars
{
    public static class Selector
    {
        public static event Action<GameObject, GameObject> SelectedObjectChanged;
        public static event Action<IEnumerable<GameObject>, IEnumerable<GameObject>> ManySelectedObjectsChanged;
        private static GameObject selectedObject;
        public static GameObject SelectedObject
        {
            get => selectedObject;
            set
            {
                var beforeSelectedObject = selectedObject;
                var beforeSelectedObjects = selectedObjects;
                
                selectedObject = value;
                selectedObjects = value != null ? new[] {selectedObject} : Array.Empty<GameObject>();

                ManySelectedObjectsChanged?.Invoke(beforeSelectedObjects, selectedObjects);
                SelectedObjectChanged?.Invoke(beforeSelectedObject, selectedObject);
            }
        }

        private static GameObject[] selectedObjects = Array.Empty<GameObject>();

        public static IReadOnlyCollection<GameObject> SelectedObjects
        {
            get => selectedObjects;
            set
            {
                var before = selectedObjects;
                //если в коллекции есть хоть что-то
                if (value != null && value.Count > 0)
                {
                    selectedObjects = value.ToArray();
                    SelectedObjectChanged?.Invoke(selectedObject, selectedObjects[0]);
                    selectedObject = selectedObjects[0];
                }
                // если коллекция пуста
                else
                {
                    selectedObjects = Array.Empty<GameObject>();
                    SelectedObjectChanged?.Invoke(selectedObject, null);
                    selectedObject = null;
                }

                ManySelectedObjectsChanged?.Invoke(before, selectedObjects);
            }
        }
    }
}