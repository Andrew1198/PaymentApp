using System.Collections.Generic;
using System.Runtime.InteropServices;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using Managers;
using UnityEngine;

namespace GoogleFireBase
{
    public class GoogleFireBaseManager : Singleton<GoogleFireBaseManager>
    {
        private FirebaseApp app;
        private FirebaseFirestore _db;

        private FirebaseFirestore Db => _db ?? (_db = FirebaseFirestore.DefaultInstance);

        private bool _init;

        public override void Awake()
        {
            base.Awake();
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    app = FirebaseApp.DefaultInstance;

                    _init = true;
                }
                else
                {
                    Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }
        
        public static void UpdateUserData()
        {
            if(!Instance._init)
                Debug.LogError("firebase doesn't init");
            
            var docRef = Instance.Db.Collection("userData").Document("data");
            var docData = new Dictionary<string, object>
            {
                {"json", JsonUtility.ToJson(PlayerData.Instance.userData)},
            };
            docRef.SetAsync(docData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                    Debug.LogError("Eror writing Firebase Firestore");
            });
        }
    }
}