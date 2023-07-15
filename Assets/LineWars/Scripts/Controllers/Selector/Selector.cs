using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    public class Selector : MonoBehaviour
    {
        private static Selector instance;
        private GameObject selectedObject;
        public static event Action<GameObject, GameObject> SelectedObjectsChanged;

        public static GameObject SelectedObject
        {
            get => instance.selectedObject;
            set
            {
                SelectedObjectsChanged?.Invoke(instance.selectedObject, value);
                instance.selectedObject = value;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
        
    }
}