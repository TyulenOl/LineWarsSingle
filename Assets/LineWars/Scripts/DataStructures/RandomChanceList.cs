using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace DataStructures
{
    public class RandomChanceList<T> //TODO: Придумать другое название?
    {
        private readonly List<ChanceItem<T>> list = new List<ChanceItem<T>>();
        private int sumChance;

        public void Add(T obj, int chance)
        {
            list.Add(new ChanceItem<T>(obj, sumChance, sumChance + chance));
            sumChance += chance;
        }

        public T PickRandomObject()
        {
            var randomValue = Random.Range(0, sumChance);
            foreach (var item in list)
            {
                if (randomValue >= item.MinChance && randomValue < item.MaxChance)
                    return item.Object;
            }

            throw new ArgumentException("RandomChanceList has failed to choose an object!");
        }

        private class ChanceItem<T1>
        {
            public readonly T1 Object;
            public readonly int MinChance;
            public readonly int MaxChance; //?можно обойтись без этой переменной?

            public ChanceItem(T1 obj, int minChance, int maxChance)
            {
                this.Object = obj;
                this.MinChance = minChance;
                this.MaxChance = maxChance;
            }

        }
    }
}
