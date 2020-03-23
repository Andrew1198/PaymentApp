
using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class SetAligment : UnityEngine.MonoBehaviour
    {
        [Button]
        private void SetAligment1()
        {
            var obj = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var textMeshProUgui in obj)
            {
                textMeshProUgui.alignment = TextAlignmentOptions.Midline;
            }
        }
            
        
    }
}