using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Prefabs.Console
{
    public class Console : MonoBehaviour
    {
        [SerializeField] private LogIcons logIcons;
        [SerializeField] private ConsoleItem consoleItemPrefab;
        [SerializeField] private Transform consoleItemContainer;
        [SerializeField] private Transform mainZone;
        [SerializeField] private Toggle logToogle;
        [SerializeField] private Toggle warningToogle;
        [SerializeField] private Toggle errorToogle;


        private int _clickCount;
        private int _errorCount;
        private float _lastClickTime;
        private int _logCount;
        private readonly List<LogData> _logs = new List<LogData>();
        private bool _showError = true;
        private bool _showLog = true;
        private bool _showWarning = true;
        private readonly List<ConsoleItem> _spawnedConsoleItems = new List<ConsoleItem>();
        private int _warningCount;


        private void Awake()
        {
            Application.logMessageReceivedThreaded += HandleLog;
            logToogle.onValueChanged.AddListener(isOn =>
            {
                _showLog = !isOn;
                logToogle.targetGraphic.color = isOn ? Color.red : Color.clear;
                DrawLogs();
            });

            warningToogle.onValueChanged.AddListener(isOn =>
                {
                    _showWarning = !isOn;
                    warningToogle.targetGraphic.color = isOn ? Color.red : Color.clear;
                    DrawLogs();
                }
            );
            errorToogle.onValueChanged.AddListener(isOn =>
            {
                _showError = !isOn;
                errorToogle.targetGraphic.color = isOn ? Color.red : Color.clear;
                DrawLogs();
            });
        }

        private void Update()
        {
        }

        private void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= HandleLog;
            logToogle.onValueChanged.RemoveAllListeners();
            warningToogle.onValueChanged.RemoveAllListeners();
            errorToogle.onValueChanged.RemoveAllListeners();
        }

        private void OpenMainZone()
        {
            mainZone.gameObject.SetActive(true);
            DrawLogs();
        }


        public void DrawLogs()
        {
            SpawnLogObject();
            HideLogObjectByToggles();
            SetTooglesCount();
        }

        private void SpawnLogObject()
        {
            var childCount = _spawnedConsoleItems.Count;
            if (childCount > _logs.Count)
            {
                Debug.LogError("Incorrect behavior Console");
                return;
            }

            for (var i = childCount; i < _logs.Count; i++)
                CreateConsoleItem(_logs[i], logIcons);
        }

        private void HideLogObjectByToggles()
        {
            bool flag;
            foreach (var spawnedConsoleItem in _spawnedConsoleItems)
            {
                switch (spawnedConsoleItem._type)
                {
                    case LogType.Assert:
                    case LogType.Log:
                        flag = _showLog;
                        break;
                    case LogType.Warning:
                        flag = _showWarning;
                        break;
                    case LogType.Exception:
                    case LogType.Error:
                        flag = _showError;
                        break;
                    default:
                        flag = false;
                        break;
                }

                spawnedConsoleItem.gameObject.SetActive(flag);
            }
        }

        private void SetTooglesCount()
        {
            logToogle.transform.Find("Count").GetComponent<TextMeshProUGUI>().text = _logCount.ToString();
            warningToogle.transform.Find("Count").GetComponent<TextMeshProUGUI>().text = _warningCount.ToString();
            errorToogle.transform.Find("Count").GetComponent<TextMeshProUGUI>().text = _errorCount.ToString();
        }

        public void Clear()
        {
            foreach (Transform child in consoleItemContainer)
                Destroy(child.gameObject);

            _spawnedConsoleItems.Clear();
            _logs.Clear();
            _logCount = 0;
            _warningCount = 0;
            _errorCount = 0;
            DrawLogs();
        }

        public void OnExitBttn()
        {
            mainZone.gameObject.SetActive(false);
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            var logData = new LogData
            {
                LogString = logString,
                StackTrace = stackTrace,
                Type = type
            };
            _logs.Add(logData);
            switch (type)
            {
                case LogType.Assert:
                case LogType.Log:
                    _logCount++;
                    break;
                case LogType.Warning:
                    _warningCount++;
                    break;
                case LogType.Exception:
                case LogType.Error:
                    _errorCount++;
                    break;
            }
        }

        private void CreateConsoleItem(LogData logData, LogIcons icons)
        {
            var consoleItem = Instantiate(consoleItemPrefab, consoleItemContainer);
            consoleItem.Init(logData, logIcons);
            _spawnedConsoleItems.Add(consoleItem);
        }

        public void OnActivationButtonClick()
        {
            if (Time.time - _lastClickTime >= .5f)
                _clickCount = 0;

            _clickCount++;
            _lastClickTime = Time.time;
            if (_clickCount == 3)
                OpenMainZone();
        }

        [Serializable]
        public class LogIcons
        {
            public Sprite LogIcon;
            public Sprite WarningIcon;
            public Sprite ErrorIcon;
        }

        public class LogData
        {
            public string LogString;
            public string StackTrace;
            public LogType Type;
        }
    }
}