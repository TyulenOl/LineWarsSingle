using System;
using AYellowpaper.SerializedCollections;
using DataStructures;
using GraphEditor;
using UnityEngine;

namespace LineWars.Controllers
{
    public class InfinityGame: Singleton<InfinityGame>
    {
        private const SceneName SceneToLoad = SceneName.InfinityGame;

        private static LoadingType? loadingType;
        private static InfinityGameMode? modeToLoad;
        private static InfinityGameSettings settingsToLoad;
        
        [SerializeField] private SerializedDictionary<InfinityGameMode, InfinityGameSettings> gameModeSettings;
        
        [Header("References")]
        [SerializeField] private GraphCreator graphCreator;
        [SerializeField] private MaskRendererV3AutoManagement maskRendererV3AutoManagement;
        [SerializeField] private SingleGame singleGame;
        
        [Header("Debug")]
        [SerializeField] private InfinityGameMode gameMode;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        public void Initialize()
        {
            if (loadingType == null)
            {
                InitializeBySettings(gameModeSettings[gameMode]);
                return;
            }
            
            switch (loadingType)
            {
                case LoadingType.ByMode:
                    InitializeBySettings(gameModeSettings[modeToLoad.Value]);
                    break;
                case LoadingType.BySettings:
                    InitializeBySettings(settingsToLoad);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            modeToLoad = null;
            settingsToLoad = null;
        }

        public void InitializeBySettings(InfinityGameSettings settings)
        {
            CreateGraph(settings.InitializeGraphSettings, settings.GraphCreatorSettings);
        }

        private void CreateGraph(
            InitializeGraphSettings initializeGraphSettings,
            GraphCreatorSettings graphCreatorSettings)
        {
            graphCreator.LoadSettings(graphCreatorSettings);
            graphCreator.Restart();
            for (var i = 0; i < initializeGraphSettings.IterationCountBeforeDeleteEdges; i++)
                graphCreator.Iterate();

            switch (initializeGraphSettings.EdgeRemovalType)
            {
                case EdgeRemovalType.ByIntersectionsCount:
                    graphCreator.DeleteIntersectingEdgesByIntersectionsCount();
                    break;
                case EdgeRemovalType.ByEdgeLength:
                    graphCreator.DeleteIntersectingEdgesByLength();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            for (var i = 0; i < initializeGraphSettings.IterationCountAfterDeleteEdges; i++)
                graphCreator.Iterate();
            
            graphCreator.RedrawAllEdges();
        }

        public static void Load(InfinityGameMode gameMode)
        {
            loadingType = LoadingType.ByMode;
            modeToLoad = gameMode; 
            SceneTransition.LoadScene(SceneToLoad);
        }

        public static void Load(InfinityGameSettings gameSettings)
        {
            loadingType = LoadingType.BySettings;
            settingsToLoad = gameSettings;
            SceneTransition.LoadScene(SceneToLoad);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (gameModeSettings != null)
                foreach (InfinityGameMode mode in Enum.GetValues(typeof(InfinityGameMode)))
                {
                    gameModeSettings.TryAdd(mode, new InfinityGameSettings());
                }
        }
#endif

        private enum LoadingType
        {
            ByMode,
            BySettings
        }
    }
}