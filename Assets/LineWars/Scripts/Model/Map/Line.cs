using LineWars.Extensions.Attributes;
using UnityEngine;

namespace LineWars.Model
{
    public class Line: MonoBehaviour, IAlive, IHitHandler
    {
        [SerializeField] [Min(1)] private int maxHp;
        [SerializeField] [ReadOnlyInspector] private int hp;
        
        private IEdge edge;

        public int Hp => hp;
        private void Awake()
        {
            edge = GetComponent<IEdge>();
        }

        protected void OnValidate()
        {
            hp = maxHp;
        }

        
        public void Accept(Hit hit)
        {
            //TODO:Реализовать
        }
    }
}