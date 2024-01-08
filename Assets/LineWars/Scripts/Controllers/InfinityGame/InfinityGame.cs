using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DataStructures;
using GraphEditor;
using LineWars.Model;
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
        [SerializeField] private MapCreator mapCreator;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private SingleGame singleGame;
        
        [Header("Debug")]
        [SerializeField] private InfinityGameMode gameMode;

        private MonoGraph monoGraph;

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
            if (settings.PlayersSettings.Players.Count != 2)
            {
                Debug.LogError("Бесконечная игра предназначена только для двух игроков!");
                return;
            }
            
            monoGraph = CreateGraph(settings.InitializeGraphSettings, settings.GraphCreatorSettings);
            CreatePlayers(settings.PlayersSettings);
            CreateGameReferee(settings.PlayersSettings.GameRefereeCreator);
            
            mapCreator.GenerateMap(monoGraph);
            cameraController.gameObject.SetActive(true);
            
            singleGame.StartGame();
        }

        private void CreateGameReferee(GameRefereeCreator creator)
        {
            creator.PrepareNodes(monoGraph.Nodes);
            creator.Initialize();
            var referee = creator.CreateGameReferee();
            referee.transform.SetParent(singleGame.transform);
        }
        
        private void CreatePlayers(PlayersSettings playersSettings)
        {
            var players = playersSettings.Players.ToArray();
            var mostRemoteNodes = monoGraph.FindMostRemoteNodes()
                // .OrderByDescending(x => x.Item1.EdgesCount + x.Item2.EdgesCount) // большее число ребер
                // .ThenBy(x => Math.Abs(x.Item1.EdgesCount - x.Item2.EdgesCount)) // но разбос меньше
                .OrderByDescending(x => (x.Item1.transform.position - x.Item2.transform.position).magnitude)
                .First();

            var nodes = new[] {mostRemoteNodes.Item1, mostRemoteNodes.Item2}
                .OrderByDescending(x => x.EdgesCount)
                .ToArray();

            var instanceOfPlayers = new List<BasePlayer>();
            visitedNodes = new HashSet<Node>();
            
            for (int i = 0; i < players.Length; i++)
            {
                var node = nodes[i];
                var player = players[i];
                var instanceOfPlayer = Instantiate(player.PlayerPrefab, transform);
                instanceOfPlayers.Add(instanceOfPlayer);
                
                ProcessOwnedInformation(instanceOfPlayer, node, player.InitialOwnerInfo);
            }

            singleGame.Player = (Player) instanceOfPlayers[0];
            singleGame.Enemies = instanceOfPlayers.Skip(1).ToList();
            visitedNodes = null;
        }

        private HashSet<Node> visitedNodes;
        private void ProcessOwnedInformation(
            BasePlayer basePlayer,
            Node rootNode,
            InitialOwnerInfo initialOwnerInfo)
        {
            basePlayer.InitialSpawns = new List<Node>();
            basePlayer.InitialNodes = new List<Node>();
            
            var orderByRadius = initialOwnerInfo.NodeInfos
                .OrderBy(x => x.NodeRadius)
                .ToArray();
            var nodesAndDistance = monoGraph.FindDistanceToNodes(rootNode);
            
            foreach (var nodeInfo in orderByRadius)
            {
                foreach (var (node, distance) in nodesAndDistance)
                {
                    if (visitedNodes.Contains(node))
                        continue;
                    
                    if (nodeInfo.NodeRadius <= distance)
                    {
                        if (nodeInfo.IsSpawn)
                            basePlayer.InitialSpawns.Add(node);
                        else
                            basePlayer.InitialNodes.Add(node);

                        node.LeftUnitType = nodeInfo.LeftUnit;
                        node.RightUnitType = nodeInfo.RightUnit;

                        visitedNodes.Add(node);
                        break;
                    }
                }
            }
        } 
            
        private MonoGraph CreateGraph(
            InitializeGraphSettings initializeGraphSettings,
            GraphCreatorSettings graphCreatorSettings)
        {
            graphCreator.LoadSettings(graphCreatorSettings);
            var graph = graphCreator.Restart();
            for (var i = 0; i < initializeGraphSettings.IterationCountBeforeDeleteEdges; i++)
                graphCreator.SimpleIterate();

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
                graphCreator.HardIterate();
            
            graphCreator.RedrawAllEdges();
            return graph;
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