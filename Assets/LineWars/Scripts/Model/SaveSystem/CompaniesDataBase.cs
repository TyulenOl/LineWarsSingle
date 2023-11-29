using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LineWars
{
    public class CompaniesDataBase : MonoBehaviour
    {
        private static CompaniesDataBase Instance { get; set; }
        
        private const string COMPANIES_DIRECTORY_NAME = "Companies";
        private const string SAVE_FILE_EXTENSION = ".json";
        private static DirectoryInfo companiesDirectory;
        private static List<CompanyState> companiesStates;
        
        [SerializeField] private List<CompanyData> companiesDatas;
        [SerializeField, ReadOnlyInspector] private CompanyState choseCompanyState;
        [SerializeField, ReadOnlyInspector] private MissionState choseMissionState;

        public static CompanyState ChooseCompany
        {
            get => Instance.choseCompanyState;
            set => Instance.choseCompanyState = value;
        }

        public static MissionState ChooseMission
        {
            get => Instance.choseMissionState;
            set
            {
                InspectNewMission(value);
                Instance.choseMissionState = value;
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Initialize();
            GameVariables.IsNormalStart = true;
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
                    if (state == null || state.companyData == null || !state.companyData.Equals(companyData))
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

        public static bool IsExists => Instance != null;
        public static void SaveAllStates()
        {
            foreach (var state in CurrenCompanyStates)
            {
                Serializer.WriteObject(GetCompanyFileName(state.companyData), state);
            }
        }

        public static void SaveChooseMission()
        {
            if (ChooseCompany == null || ChooseMission == null)
            {
                Debug.LogError("Невозможно сохранить выбранную миссию!");
                return;
            }
            Serializer.WriteObject(GetCompanyFileName(ChooseCompany.companyData), ChooseCompany);
        }

        public static IReadOnlyList<CompanyState> CurrenCompanyStates => companiesStates;

        private static string GetCompanyFileName(CompanyData company)
        {
            return Path.Join(
                companiesDirectory.FullName,
                $"{company.Name}{SAVE_FILE_EXTENSION}"
            );
        }

        private static void InspectNewMission(MissionState state)
        {
            if (state == null)
                return;
            if (ChooseCompany == null || !ChooseCompany.missionStates.Contains(state))
                Debug.LogError($"Invalid fields state in {nameof(CompaniesDataBase)}");
        }
    }
}