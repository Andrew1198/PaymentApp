using System.Collections;
using System.Collections.Generic;
using Windows.HelperWindows;
using TMPro;
using UnityEngine;


#pragma warning disable 0649
namespace Anims
{
    public class LoadingScreen : WindowBase
    {
        [SerializeField] private TextMeshProUGUI loadingField;
        [SerializeField] private GameObject back;

        [SerializeField]
        private float
            appearTime; // время после которого должен появится загрузочный экран, сделанно для того что бы если загрузка будет короткая окно не появлялось

        private Coroutine _coroutine;
        private int _count;
        
        public override void Open(Dictionary<string, object> DynamicWindowData = null)
        {
            base.Open(DynamicWindowData);
            Show();
        }

        public override void Close()
        {
            Hide();
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

            string[] points = {"",".", "..", "..."};
            const string loadingText = "Loading";
            var i = 0;
            while (true)
            {
                loadingField.text = loadingText + points[i % points.Length];
                yield return new WaitForSeconds(.3f);
                i++;
                if (i % 4 == 0)
                    i = 0;
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
            base.Close();
        }
    }
}