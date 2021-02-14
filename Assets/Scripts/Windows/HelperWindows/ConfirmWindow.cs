using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Windows.HelperWindows
{
    public class ConfirmWindow : WindowBase
    {
        [SerializeField] private Button Ok;
        [SerializeField] private Button No;
        
        public override void Open(Dictionary<string, object> DynamicWindowData = null)
        {
            base.Open(DynamicWindowData);
            Ok.onClick.RemoveAllListeners();
            Ok.onClick.AddListener(() =>
            {
                (DynamicWindowData["OnOkAction"] as Action)();
                Close();
            });
        }
    }
}