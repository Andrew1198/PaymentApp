using System;
using System.Collections;
using DefaultNamespace;
using HelperScripts;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Tabs
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Tab : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        protected bool Inited;
        public virtual void Init()
        {
            Utility.StartCor(WaitInit());
        }

        IEnumerator WaitInit()
        {
            Events.EnableLoadingScreen.Invoke();
            _canvasGroup.interactable = false;
            yield return  new WaitUntil(()=>Inited);
            gameObject.SetActive(true);
            Events.DisableLoadingScreen.Invoke();
            _canvasGroup.interactable = true;
        }
    }
}