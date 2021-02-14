using System;
using System.Collections;
using System.Collections.Generic;
using Windows.HelperWindows;
using DefaultNamespace;
using HelperScripts;
using HelperWindows;
using Managers;
using UnityEngine;
#pragma warning disable 0649
namespace Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Tab : WindowBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        protected bool Inited;

        public override void Open(Dictionary<string, object> DynamicWindowData = null)
        {
            Utility.StartCor(WaitInit());
        }
        
        private IEnumerator WaitInit()
        {
            WindowsManager.EnableDisableLoadingWindow();
            _canvasGroup.interactable = false;
            yield return new WaitUntil(() => Inited);
            gameObject.SetActive(true);
            WindowsManager.EnableDisableLoadingWindow(false);
            _canvasGroup.interactable = true;
        }
    }
}