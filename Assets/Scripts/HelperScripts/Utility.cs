using System;
using System.Collections;
using UnityEngine;

namespace HelperScripts
{
    public class Utility
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
    }
}