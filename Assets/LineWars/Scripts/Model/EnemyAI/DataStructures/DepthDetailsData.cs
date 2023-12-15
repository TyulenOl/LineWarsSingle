using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Depth Details Data", menuName = "EnemyAI/ Depth Details")]
    public class DepthDetailsData
        : ScriptableObject
    {
        public class DepthDetailsResponse
        {
            public readonly bool InDepth;
            public readonly IReadOnlyCollection<DepthDetailsData> AvailableCommands;

            public DepthDetailsResponse(bool inDepth, IReadOnlyCollection<DepthDetailsData> availableCommands)
            {
                InDepth = inDepth;
                AvailableCommands = availableCommands;
            }   
        }

        [Serializable]
        public class DepthDetails
        {
            [SerializeField] private int endTurn;
            [SerializeField] private List<CommandType> availableCommands;
            private HashSet<CommandType> availableCommandsHashSet;
            public int EndTurn => endTurn;
            public IReadOnlyCollection<CommandType> Commands => availableCommandsHashSet;

            public void BuildHashSet()
            {
                availableCommandsHashSet = new HashSet<CommandType>(availableCommands);
            }
        }

        [SerializeField] private List<DepthDetails> turnIntervals;

        private void OnEnable()
        {
            foreach(var turnInterval in turnIntervals)
                turnInterval.BuildHashSet();
        }

/*        public DepthDetailsResponse GetDepthDetails(int depth)
        {

        }
*/
    }
}
