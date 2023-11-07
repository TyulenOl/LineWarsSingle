using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineWars
{
    public class ChooseCompanyMenu : UIStackElement
    {
        public static ChooseCompanyMenu Instance { get; private set; }

        [FormerlySerializedAs("companyElementPrefab")] [SerializeField]
        private CompanyElementUI companyElementUIPrefab;

        [SerializeField] private Transform contentTransform;

        private List<CompanyElementUI> companyElements;


        protected override void Awake()
        {
            base.Awake();
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.LogError($"Dublicated {nameof(ChooseCompanyMenu)}");
                Destroy(gameObject);
            }
        }

        public void Start()
        {
            companyElements = new List<CompanyElementUI>();
            Redraw();
        }

        public void Redraw()
        {
            var companies = CompaniesDataBase.CurrenCompanyStates;

            foreach (var element in companyElements)
                Destroy(element.gameObject);
            companyElements.Clear();

            foreach (var companyState in companies)
            {
                var companyElement = Instantiate(companyElementUIPrefab, contentTransform);
                companyElement.Initialize(companyState, OnCompanyButtonClick);
                companyElements.Add(companyElement);
            }

            Instantiate(companyElementUIPrefab, contentTransform);
            Instantiate(companyElementUIPrefab, contentTransform);
        }

        private void OnCompanyButtonClick(CompanyState companyState)
        {
            UIStack.Instance.PushElement(ChooseMissionMenu.Instance);
            ChooseMissionMenu.Instance.Initialize(companyState);
            CompaniesDataBase.ChooseCompany = companyState;
        }
    }
}