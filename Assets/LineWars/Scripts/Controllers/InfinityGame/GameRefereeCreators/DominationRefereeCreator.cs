using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(menuName = "InfinityGame/GameRefereeCreators/Domination", order = 59)]
    public class DominationRefereeCreator : GameRefereeCreator
    {
        [SerializeField] private int rounds;
        public override GameReferee CreateGameReferee()
        {
            var referee = new GameObject().AddComponent<NewDominationGameReferee>();
            referee.SetRounds(rounds);
            return referee;
        }

        public override void Initialize()
        {
        }
    }
}