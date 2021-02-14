using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Data;
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
            obj.StartCoroutine(InvokeCor(method, time));
        }

        private static IEnumerator InvokeCor(Action method, float time)
        {
            yield return new WaitForSeconds(time);
            method();
        }

        private static IEnumerator InvokeCor(Action method, Func<bool> predicate)
        {
            yield return new WaitUntil(predicate);
            method();
        }
        
        public static void Invoke(Action method, float time)
        {
            Instance.StartCoroutine(InvokeCor(method, time));
        }

        public static void Invoke(Action method, Func<bool> predicate)
        {
            Instance.StartCoroutine(InvokeCor(method, predicate));
        }


        public static void StartCor(IEnumerator routine)
        {
            Instance.StartCoroutine(routine);
        }

        [Button]
        private void EnableLoadingScreen()
        {
            WindowsManager.EnableDisableLoadingWindow();
        }

        [Button]
        private void DisableLoadingScreen()
        {
            WindowsManager.EnableDisableLoadingWindow(false);
        }
        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}