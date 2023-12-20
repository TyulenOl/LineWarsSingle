using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Depth Details Data", menuName = "EnemyAI/ Depth Details")]
    public class DepthDetailsData
        : ScriptableObject
    {
        [SerializeField] private List<DepthDetails> turnIntervals;
        private List<DepthDetailsResponse> depthDetails;
        private DepthDetailsResponse nullResponse;
        
        public int TotalDepth => depthDetails.Count;


        private void OnEnable()
        {
            InitializeDictionary();
            nullResponse = new DepthDetailsResponse(false, null);
        }

        private void InitializeDictionary()
        {
            depthDetails = new List<DepthDetailsResponse>();
            foreach (var details in turnIntervals)
            {
                var commandsHashSet = new HashSet<CommandType>(details.AvailableCommands);
                var response = new DepthDetailsResponse(true, commandsHashSet);
                for(var i = 0; i < details.TurnDuration; i++)
                {
                    depthDetails.Add(response);
                }
            }
        }

        public DepthDetailsResponse GetDepthDetails(int depth)
        {
            if (depth <= 0)
                throw new InvalidOperationException("Depth should be positive!");
            depth--;
            if (depth < depthDetails.Count)
                return depthDetails[depth];
            return nullResponse;
        }
    }

    [Serializable]
    public class DepthDetails
    {
        [SerializeField] private int turnDuration;
        [SerializeField] private List<CommandType> availableCommands;
        public int TurnDuration => turnDuration;
        public IReadOnlyList<CommandType> AvailableCommands => availableCommands;
    }

    public class DepthDetailsResponse
    {
        public readonly bool InDepth;
        public readonly IReadOnlyCollection<CommandType> AvailableCommands;

        public DepthDetailsResponse(bool inDepth, IReadOnlyCollection<CommandType> availableCommands)
        {
            InDepth = inDepth;
            AvailableCommands = availableCommands;
        }
    }
}
