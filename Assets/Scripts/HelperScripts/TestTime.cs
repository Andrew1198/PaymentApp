using System;
using NaughtyAttributes;
using UnityEngine;

namespace HelperScripts
{
    public class TestTime : MonoBehaviour
    {
        [Button()]
        private void Test()
        {
            var a = -8586148816602275128;

            var dt = DateTime.FromBinary(a);
            Debug.LogError(dt);
        }
        
    }
}