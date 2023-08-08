using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LineWars
{
    public class CompaniesDataBase : MonoBehaviour
    {
        public static CompaniesDataBase Instance { get; private set; }

        public const string COMPANIES_DIRECTORY_NAME = "Companies";
        public const string SAVE_FILE_EXTENSION = ".json";
        private static DirectoryInfo companiesDirectory;
        private static List<CompanyState> companiesStates;
        
        public static IReadOnlyList<CompanyState> CurrenCompanyStates => companiesStates;

        [SerializeField] private List<CompanyData> companiesDatas;
        
        private void Awake()
        {
            //Debug.Log("Awake DataBase");
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                Debug.LogError($"Too many {nameof(CompaniesDataBase)}");
            }
        }

        private void Start()
        {
            //Debug.Log("Start DataBase");
            Initialize();
        }

        private void Initialize()
        {
            companiesDirectory = new DirectoryInfo
            (
                Path.Join(
                    Application.persistentDataPath,
                    COMPANIES_DIRECTORY_NAME)
            );

            if (!companiesDirectory.Exists)
                companiesDirectory.Create();
            
            companiesStates = new List<CompanyState>(companiesDatas.Count);

            foreach (var companyData in companiesDatas)
            {
                var companyFileName = GetCompanyFileName(companyData);
                if (File.Exists(companyFileName))
                {
                    var state = Serializer.ReadObject<CompanyState>(companyFileName);
                    if (state == null || state.CompanyData == null || !state.CompanyData.Equals(companyData))
                    {
                        state = new CompanyState(companyData);
                        Serializer.WriteObject(companyFileName, state);
                    }
                    
                    companiesStates.Add(state);
                }
                else
                {
                    var state = new CompanyState(companyData);
                    Serializer.WriteObject(companyFileName, state);
                    
                    companiesStates.Add(state);
                }
            }
        }
        

        private static string GetCompanyFileName(CompanyData company)
        {
            return Path.Join(
                companiesDirectory.FullName,
                $"{company.Name}{SAVE_FILE_EXTENSION}"
            );
        }
    }
}