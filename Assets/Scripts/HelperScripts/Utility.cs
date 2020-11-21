using System;
using System.Collections;
using DefaultNamespace;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace HelperScripts
{
    public class Utility : Singleton<Utility>
    {
        public static void Invoke(MonoBehaviour obj, Action method, float time)
        {
            obj.StartCoroutine(InvokeCor(method,time));
        }

        private static IEnumerator InvokeCor(Action method, float time)
        {
            yield return new WaitForSeconds(time);
            method();
        }
        public static void Invoke(Action method, float time)
        {
            Instance.StartCoroutine(InvokeCor(method,time));
        }

        public static void StartCor(IEnumerator routine)
        {
            Instance.StartCoroutine(routine);
        }
        [Button]
        private void EnableLoadingScreen()
        {
            Events.EnableLoadingScreen.Invoke();
        }
        [Button]
        private void DisableLoadingScreen()
        {
            Events.DisableLoadingScreen.Invoke();
        }
        
        
    }
}