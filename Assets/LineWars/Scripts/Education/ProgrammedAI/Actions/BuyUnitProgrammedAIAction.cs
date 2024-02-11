using UnityEngine;

namespace LineWars.Model
{
    public class BuyUnitProgrammedAIAction : ProgrammedAIAction,
        IAIActionWithNeedProgrammedPlayer
    {
        [SerializeField] private Node node;
        [SerializeField] private UnitType unitType;

        private ProgrammedAI programmedAI;

        public void Prepare(ProgrammedAI programmedAI) => this.programmedAI = programmedAI;

        public override void Execute()
        {
            programmedAI.SpawnUnit(node, unitType);
        }
    }
}