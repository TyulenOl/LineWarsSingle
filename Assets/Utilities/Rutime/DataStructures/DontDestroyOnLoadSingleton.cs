using UnityEngine;

namespace DataStructures
{
    public class DontDestroyOnLoadSingleton<T>: MonoBehaviour
        where T: DontDestroyOnLoadSingleton<T>
    {
        private static T instance;

        public static T Instance => instance;

        protected virtual void Awake()
        {
            if (GetType() != typeof(T))
            {
                Debug.LogError($"The singleton object {typeof(T).Name} does not refer to himself");
                return;
            }

            if (instance == null)
            {
                instance = (T) this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}