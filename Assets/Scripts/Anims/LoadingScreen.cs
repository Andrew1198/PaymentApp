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

        [SerializeField]
        private float
            appearTime; // время после которого должен появится загрузочный экран, сделанно для того что бы если загрузка будет короткая окно не появлялось

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
                _coroutine = StartCoroutine(Anim());


            _count++;
        }

        private IEnumerator Anim()
        {
            yield return new WaitForSeconds(appearTime);
            back.SetActive(true);

            string[] points = {"", ".", "..", "..."};
            var loadingText = "Loading";
            string result;
            while (true)
            {
                result = loadingText + points[Random.Range(0, int.MaxValue) % points.Length];
                loadingField.text = result;
                yield return new WaitForSeconds(.3f);
            }
        }

        private void Hide()
        {
            _count--;
            if (_count != 0)
                return;
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            back.SetActive(false);
        }
    }
}