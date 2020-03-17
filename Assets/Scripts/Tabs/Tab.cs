using System;
using UnityEngine;

namespace Tabs
{
    public abstract class Tab : MonoBehaviour
    {
        private void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            gameObject.SetActive(true);
        }
    }
}