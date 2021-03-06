﻿#pragma warning disable 0649
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HelperScripts
{
    public class TapHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float requireHoldToActivate = 1f;
        private float _downClickTime;
        private bool _pointerDown;
        [SerializeField] private UnityEvent _event;
        private void Reset()
        {
            _pointerDown = false;
        }
        private void Update()
        {
            if (_pointerDown)
                if (Time.time >= _downClickTime + requireHoldToActivate)
                {
                    _event?.Invoke();
                    Reset();
                }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDown = true;
            _downClickTime = Time.time;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pointerDown = false;
        }
    }
}