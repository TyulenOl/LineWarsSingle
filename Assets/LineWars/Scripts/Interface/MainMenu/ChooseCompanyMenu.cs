using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public class ChooseCompanyMenu: UIStackElement
    {
        [SerializeField] private CompanyElement companyElementPrefab;
        [SerializeField] private Transform contentTransform;
        
        private List<CompanyElement> companyElements;
        

        protected override void Awake()
        {
            base.Awake();
            companyElements = new List<CompanyElement>();
        }

        public void Start()
        {
            Redraw();
        }

        public void Redraw()
        {
            var companies = CompaniesDataBase.CompanyStates;

            foreach (var element in companyElements)
                Destroy(element.gameObject);
            companyElements.Clear();

            foreach (var companyState in companies)
            {
                var companyElement = Instantiate(companyElementPrefab, contentTransform);
                companyElement.Initialize(companyState);
            }
        }
    }
}