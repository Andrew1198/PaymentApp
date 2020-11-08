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

        private Coroutine _coroutine;
        private int _count;
        private void Awake()
        {
            Events.EnableLoadingScreen += Show;
            Events.DisableLoadingScreen += Hide;
        }

        private void Show()
        {
            if (_coroutine == null)
            {
                back.SetActive(true);
                _coroutine = StartCoroutine(Anim());
            }

            _count++;
        } 

        private IEnumerator Anim()
        {
            string[] points = {"", ".", "..", "..."};
            var loadingText = "Loading";
            string result;
            for (var i = 0;; i++)
            {
                result = loadingText + points[i % points.Length];
                loadingField.text = result;
                yield return new WaitForSeconds(.3f);
            }
        }

        private void Hide()
        {
            _count--;
            if(_count != 0)
                return;
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            back.SetActive(false);
        }
    }
}