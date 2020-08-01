using System;
using NaughtyAttributes;
using UnityEngine;
#pragma warning disable 0649
namespace HelperScripts
{
    public class SaveZone : MonoBehaviour
    {
        private enum SimDevice { None, iPhoneX }
        [SerializeField] private  SimDevice Sim = SimDevice.None;
        private Rect _lastSaveArea = new Rect(0f,0f,0f,0f);
        
        Rect[] NSA_iPhoneX =
        {
            new Rect (0f, 102f/ 2436f, 1f, 2202f / 2436f),  // Portrait
            new Rect (132f / 2436f, 63f / 1125f, 2172f / 2436f, 1062f / 1125f)  // Landscape
        };
        
        void Awake ()
        {
            Refresh ();
        }

#if !UNITY_EDITOR
       private void Update()
        {
            Refresh();
        } 
#endif
        private void Refresh()
        {
            var saveArea = GetSaveArea();
            if (saveArea != _lastSaveArea)
                ApplySafeArea(saveArea);
            
            
        }
        
        private Rect GetSaveArea()
        {
            var safeArea = Screen.safeArea;
#if UNITY_EDITOR
            if (Sim == SimDevice.None) return safeArea;
            
            var nsa = new Rect (0, 0, Screen.width, Screen.height);

            switch (Sim)
            {
                case SimDevice.iPhoneX:
                    if (Screen.height > Screen.width)  // Portrait
                        nsa = NSA_iPhoneX[0];
                    else  // Landscape
                        nsa = NSA_iPhoneX[1];
                    break;
            }
            safeArea = new Rect (Screen.width * nsa.x, Screen.height * nsa.y, Screen.width * nsa.width, Screen.height * nsa.height);
#endif

            return safeArea;
        }

        private void ApplySafeArea(Rect r)
        {
            _lastSaveArea = r;
            
            var anchorMin = r.position;
            var anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            var rectTransform = (transform as RectTransform);
            
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
        
    }
}