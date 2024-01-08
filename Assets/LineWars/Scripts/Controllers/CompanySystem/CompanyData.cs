using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "New CompanyData", menuName = "Data/Create CompanyData", order = 50)]
    public class CompanyData: ScriptableObject
    {
        [SerializeField] private string companyName;
        [SerializeField] [TextArea(5,10)] private string companyDescription;
        [SerializeField] private Sprite companyImage;
        [SerializeField] private NationType companyNation;
        
        public string Name => companyName;
        public string Description => companyDescription;
        public Sprite Image => companyImage;
        public NationType Nation => companyNation;
    }
}