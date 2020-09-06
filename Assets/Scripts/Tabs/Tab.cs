using System;
using UnityEngine;

namespace Tabs
{
    public abstract class Tab : MonoBehaviour
    {
        public virtual void Init()
        {
            gameObject.SetActive(true);
        }
    }
}