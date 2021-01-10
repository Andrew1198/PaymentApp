using System;
using System.Collections.Generic;
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
        [HideInInspector] public bool init;
        private FirebaseFirestore _db;
        private FirebaseApp app;

        private FirebaseFirestore Db => _db ?? (_db = FirebaseFirestore.DefaultInstance);

        public void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    app = FirebaseApp.DefaultInstance;
                    Debug.Log("Firebase has been inited");
                    init = true;
                }
                else
                {
                    Debug.LogError(string.Format(
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
                _transactions = UserDataManager.YearlyTransactions,
                categories = UserDataManager.Instance.UserData.categories,
                savings = UserDataManager.Instance.UserData.savings
            };
            var jsonFireBaseBackUp = JsonUtility.ToJson(fireBaseBackup);
            GetData((serverFireBaseBackup, doc) =>
            {
                if (doc == null)
                    return;

                var jsonServerFireBaseBackUp = JsonUtility.ToJson(serverFireBaseBackup);
                if (jsonFireBaseBackUp == jsonServerFireBaseBackUp)
                    return;

                var docRef = Instance.Db.Collection("userData").Document("data");
                doc[SystemInfo.deviceUniqueIdentifier] = jsonFireBaseBackUp;
                docRef.SetAsync(doc).ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                        Debug.LogError("Eror writing Firebase Firestore");
                    else
                        Debug.Log("UpdateGoogleFireBase");
                });
            });
        }

        public static void GetData(Action<FireBaseBackup, Dictionary<string, object>> onReceived)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                onReceived(null, null);
                return;
            }

            var docRef = Instance.Db.Collection("userData").Document("data");
            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                var snapshot = task.Result;
                if (snapshot.Exists)
                {
                    var data = snapshot.ToDictionary();
                    if (data.ContainsKey(SystemInfo.deviceUniqueIdentifier))
                    {
                        var userData =
                            JsonUtility.FromJson<FireBaseBackup>(data[SystemInfo.deviceUniqueIdentifier].ToString());
                        onReceived.Invoke(userData, data);
                    }
                    else
                    {
                        onReceived.Invoke(null, data);
                        Debug.Log($"Field {snapshot.Id} does not exist!");
                    }
                }
                else
                {
                    onReceived.Invoke(null, null);
                    Debug.Log($"Document {snapshot.Id} does not exist!");
                }
            });
        }
    }
}