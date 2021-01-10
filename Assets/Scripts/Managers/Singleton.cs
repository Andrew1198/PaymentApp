using UnityEngine;

namespace Managers
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static bool Init;
        public static T Instance { get; private set; }

        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
                Init = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}