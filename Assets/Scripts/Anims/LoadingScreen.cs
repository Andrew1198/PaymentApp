using System;
using System.Collections;
using TMPro;
using UnityEngine;
#pragma warning disable 0649
namespace DefaultNamespace
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loadingField;
        [SerializeField] private GameObject back;

        private void Awake()
        {
            back.SetActive(true);
            StartCoroutine(Anim());
            Events.OnLoadedData += OnLoadedData;
        }

        private IEnumerator Anim()
        {
            string[] points = {"",".", "..", "..."};
            var loadingText = "Loading";
            string result;
            for (var i = 0;; i++)
            {
                result = loadingText + points[i % points.Length];
                loadingField.text = result;
                yield return new WaitForSeconds(.3f);
            }
        }

        private void OnLoadedData()
        {
            Events.OnLoadedData -= OnLoadedData;
            Destroy(gameObject);
        }
    }
}