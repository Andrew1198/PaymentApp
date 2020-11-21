using System;
using System.Collections;
using DefaultNamespace;
using HelperScripts;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Tabs
{
    public abstract class Tab : MonoBehaviour
    {
        protected bool Inited;
        public virtual void Init()
        {
            Utility.StartCor(WaitInit());
        }

        IEnumerator WaitInit()
        {
            Events.EnableLoadingScreen.Invoke();
            yield return  new WaitUntil(()=>Inited);
            gameObject.SetActive(true);
            Events.DisableLoadingScreen.Invoke();
        }
    }
}