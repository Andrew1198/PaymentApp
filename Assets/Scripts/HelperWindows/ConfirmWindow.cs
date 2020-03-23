using System;
using UnityEngine;
using UnityEngine.UI;

namespace HelperWindows
{
    public class ConfirmWindow : MonoBehaviour
    {
        [SerializeField] private Button Ok;
        [SerializeField] private Button No;
        
        public void Open(Action OnOk)
        {
            gameObject.SetActive(true);
            Ok.onClick.RemoveAllListeners();
            Ok.onClick.AddListener(()=>
            {
                OnOk();
                gameObject.SetActive(false);
            });
        }
    }
}