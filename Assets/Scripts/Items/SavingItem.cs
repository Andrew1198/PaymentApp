using Windows;
using Windows.HelperWindows.ManageWalletWindow;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Items
{
    public class SavingItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private new TextMeshProUGUI name;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI currencyType;
        [SerializeField] private TextMeshProUGUI count;
        [SerializeField] private ManageWalletWindow manageWalletWindow;
        private float _downClickTime;

        private bool _pointerDown;
        private readonly float _requireHold = 1f;

        private Saving _saving;

        private void Reset()
        {
            _pointerDown = false;
        }

        private void Update()
        {
            if (_pointerDown)
                if (Time.time >= _downClickTime + _requireHold)
                {
                    manageWalletWindow.Init(_saving);
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

        public void Init(Saving saving)
        {
            name.text = saving.name;
            currencyType.text = saving.currency == Currency.UAH ? "UAH" : "USD";
            count.text = saving.count.ToString();
            _saving = saving;
        }
    }
}