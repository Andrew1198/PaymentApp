using UnityEngine;

namespace Managers
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance => _instance;
        public static bool Init;

        public virtual void Awake ()
        {
            if (_instance == null) {
                _instance = this as T;
                DontDestroyOnLoad (this.gameObject);
                Init = true;
            } else {
                Destroy (gameObject);
            }
        }
    }
}
