using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    [RequireComponent(typeof(Node))]
    public class NodeScore: MonoBehaviour
    {
        [SerializeField, Min(0)] private int score;
        public int Score => score;
    }
}