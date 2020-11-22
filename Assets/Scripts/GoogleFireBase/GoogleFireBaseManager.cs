using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Data;
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

        [HideInInspector] public bool init;

        public void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    app = FirebaseApp.DefaultInstance;
                    Debug.Log("Firebase has been inited");
                    init = true;
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
            if (!Instance.init || Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.LogError("firebase doesn't init");
                return;
            }
            var fireBaseBackup = new FireBaseBackup
            {
                _transactions =  UserDataManager.YearlyTransactions,
                categories = UserDataManager.Instance.UserData.categories,
                savings = UserDataManager.Instance.UserData.savings
            };
            var docRef = Instance.Db.Collection("userData").Document("data");
            var docData = new Dictionary<string, object>
            {
                {"json", JsonUtility.ToJson(fireBaseBackup)},
            };
            docRef.SetAsync(docData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                    Debug.LogError("Eror writing Firebase Firestore");
                else
                    Debug.Log("UpdateGoogleFireBase");
            });
        }

        public static void GetData(Action<UserData> onReceived)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                onReceived(null);
                return;
            }

            var docRef = Instance.Db.Collection("userData").Document("data");
            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                var snapshot = task.Result;
                if (snapshot.Exists)
                {
                    var data = snapshot.ToDictionary();
                    var userData = JsonUtility.FromJson<UserData>(data["json"].ToString());
                    onReceived.Invoke(userData);
                }
                else
                {
                    onReceived.Invoke(null);
                    Debug.Log($"Document {snapshot.Id} does not exist!");
                }
            });
        }
    }
}