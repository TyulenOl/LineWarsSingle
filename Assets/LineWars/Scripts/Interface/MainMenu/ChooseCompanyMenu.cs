using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineWars
{
    public class ChooseCompanyMenu : MonoBehaviour
    {
        [SerializeField] private List<CompanyElementUI> companyElementUis;
        private bool initialized;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (initialized)
            {
                Debug.LogError($"{GetType().Name} is initialized!");
                return;
            }
            initialized = true;
            
            foreach (var companyElementUi in companyElementUis)
                companyElementUi.Initialize();
        }
    }
}