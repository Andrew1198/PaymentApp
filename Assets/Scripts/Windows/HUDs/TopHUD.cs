using System;
using Windows.WindowsData;
using Managers;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649

namespace DefaultNamespace.Windows.HUDs
{
    public class TopHUD : MonoBehaviour
    {
        [SerializeField] private Button settingButton;

        public void Awake()
        {
            settingButton.onClick.AddListener(()=>WindowsManager.OpenWindow<SettingsWindowData>());
        }

        public void OnDestroy()
        {
            settingButton.onClick.RemoveAllListeners();
        }
    }
}