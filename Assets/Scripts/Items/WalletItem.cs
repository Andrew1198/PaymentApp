using Data;
using HelperWindows.ManageWalletWindow;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
#pragma warning disable 0649
namespace Items
{
    public class WalletItem : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private new TextMeshProUGUI name;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI type;
        [SerializeField] private TextMeshProUGUI count;
        [SerializeField] private ManageWalletWindow manageWalletWindow;

        private Wallet _wallet;
        
        public void Init(Wallet wallet)
        {
            name.text = wallet.name;
            type.text = wallet._currency == Currency.UAH ? "UAH" : "USD";
            count.text = wallet._count.ToString();
            _wallet = wallet;
        }

        private bool _pointerDown;
        private float _downClickTime;
        private float _requireHold = 1f;
        private void Update()
        {
            if (_pointerDown)
            {
                if (Time.time >= _downClickTime + _requireHold)
                {
                    manageWalletWindow.Init(_wallet);
                    Reset();
                }
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

        private void Reset()
        {
            _pointerDown = false;
        }
    }
}