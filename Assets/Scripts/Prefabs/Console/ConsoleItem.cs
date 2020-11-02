using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

#pragma warning disable 0649
namespace Prefabs.Console
{
    public class ConsoleItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField] private Image errorIcon;
        [SerializeField] private FullTextOfError fullTextOfError;
        [HideInInspector] public string _logString;
        [HideInInspector] public string _stackTrace;
        [HideInInspector] public LogType _type;
        
        public void Init(Console.LogData logData,Console.LogIcons logIcons)
        {
            _logString = logData.LogString;
            _stackTrace = logData.StackTrace;
            _type = logData.Type;
            SetIcon(logData.Type,logIcons);
            textField.text = logData.LogString;
        }

        private void SetIcon(LogType type, Console.LogIcons logIcons)
        {
            switch (type)
            {
                case LogType.Assert:
                case LogType.Log:
                    errorIcon.sprite = logIcons.LogIcon;
                    break;
                case LogType.Warning:
                    errorIcon.sprite = logIcons.WarningIcon;
                    break;
                case LogType.Exception:
                case LogType.Error:
                    errorIcon.sprite = logIcons.ErrorIcon;
                    break;
                default:
                    errorIcon.sprite = logIcons.ErrorIcon;
                    break;
            }
        }

        public void OnClick()
        {
            fullTextOfError.Init(_stackTrace);
        }
    }
}
