using System;
using System.IO;
using Data;
using UnityEngine;

namespace Managers
{
    public class AppManager : Singleton<AppManager>
    {
        private void Start()
        {
            TabManager.UpdateTab();
        }
    }
}