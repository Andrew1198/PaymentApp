using System;
using System.IO;
using Data;
using DefaultNamespace;
using UnityEngine;

#pragma warning disable 0649
namespace Managers
{
    public class ApplicationLoader : MonoBehaviour
    {
        private void Awake()
        {
            SetUserDataFromLocal();
        }

        private void Start()
        {
            TabManager.UpdateOpenedTab();
        }

        private void OnApplicationPause(bool isPaused)
        {
            Debug.Log($"<color=yellow>OnApplicationPause isPaused = {isPaused}</color>");

            #if !UNITY_EDITOR
          if (isPaused)
                UserDataManager.Save();
            else
            {
                if (!UserDataManager.Inited) return;
                UserDataManager.SelectedDate = DateTime.Now;
                Events.OnUpdateTab?.Invoke();
            }
          #endif
        }

        private void OnApplicationQuit()
        {
            UserDataManager.Save();
        }

        private void SetUserDataFromLocal()
        {
            var path = Path.Combine(Application.persistentDataPath, SystemInfo.deviceUniqueIdentifier + ".json");
            if (!File.Exists(path))
            {
                UserDataManager.Init(new UserData());
            }
            else
            {
                var json = File.ReadAllText(path);
                UserDataManager.Init(JsonUtility.FromJson<UserData>(json));
            }
        }
        
        
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
                UserDataManager.Save(true);
            }
#endif
        }
    }
}