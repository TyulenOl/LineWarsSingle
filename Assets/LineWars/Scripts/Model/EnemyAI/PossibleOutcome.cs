
using System.Collections.Generic;

namespace LineWars.Model
{
    public class PossibleOutcome 
    {
        public readonly int Score;
        public readonly IEnumerable<ICommandBlueprint> Commands;

        public PossibleOutcome(int score, IEnumerable<ICommandBlueprint> commands)
        {
            Score = score;
            Commands = commands;
        }
    }
}
