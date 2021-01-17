using TMPro;
using UnityEngine;
#pragma warning disable 0649
namespace Prefabs.Console
{
    public class FullTextOfError : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textField;

        public void Init(string stackTrace)
        {
            gameObject.SetActive(true);
            textField.text = stackTrace;
        }
    }
}