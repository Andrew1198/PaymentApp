using System;
using System.Collections;
using DefaultNamespace;
using HelperScripts;
using HelperWindows;
using UnityEngine;
#pragma warning disable 0649
namespace Tabs
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Tab : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        protected bool Inited;
        protected readonly TabData data = new TabData();
        
        public virtual void Init()
        {
            Utility.StartCor(WaitInit());
        }

        private IEnumerator WaitInit()
        {
            Events.EnableLoadingScreen.Invoke();
            _canvasGroup.interactable = false;
            yield return new WaitUntil(() => Inited);
            gameObject.SetActive(true);
            Events.DisableLoadingScreen.Invoke();
            _canvasGroup.interactable = true;
        }

        protected void InitTabByTabData(Action onFinish)
        {
            var UpdateCount = 0;
            if (data.needToUpdateBankTransactions)
                TransactionUtils.UpdateAllBankTransactions(() =>
                {
                    UpdateCount++;
                });
            else
                UpdateCount++;
            
            if(data.needToUpdateCurrencyRate)
                TransactionUtils.UpdateCurrencyRates(() =>
                {
                    UpdateCount++;
                });
            else
            {
                UpdateCount++;
            }
            Utility.Invoke(onFinish,()=>UpdateCount==2);
        }

        protected class TabData
        {
            public bool needToUpdateBankTransactions = true;
            public bool needToUpdateCurrencyRate = true;
        }
        
    }
}