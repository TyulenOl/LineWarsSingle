using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Extensions
{
    public static class IEnumerableExtension
    {
        public static Stack<T> ToStack<T>(this IEnumerable<T> enumerable, bool useReverse = false)
        {
            var tempList = enumerable.ToList();
            if (useReverse)
                tempList.Reverse();
            
            var result = new Stack<T>();
            foreach (var element in tempList)
            {
                result.Push(element);
            }

            return result;
        }

        public static IEnumerable<TComponent> GetComponentMany<TComponent>(this IEnumerable<MonoBehaviour> enumerable)
        {
            return enumerable
                .Select(x => x.GetComponent<TComponent>())
                .Where(x => x != null);
        }
    }
}