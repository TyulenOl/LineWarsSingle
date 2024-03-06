using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    [RequireComponent(typeof(Node))]
    public class NodeScore: MonoBehaviour
    {
        [SerializeField, Min(0)] private int score = 1;
        public int Score
        {
            get => score;
            set => score = value;
        }
    }
}