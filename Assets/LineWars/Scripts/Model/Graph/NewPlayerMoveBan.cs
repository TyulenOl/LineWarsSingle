using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public class NewPlayerMoveBan : MonoBehaviour
    {
        [SerializeField] private BasePlayer bannedPlayer;

        public bool CanPlayerMove(int ownerId)
        {
            return bannedPlayer.Id != ownerId;
        }
    }
}
