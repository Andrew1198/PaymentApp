using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class WalletItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI type;
        [SerializeField] private TextMeshProUGUI count;
        
        public void Init(Wallet wallet)
        {
            name.text = wallet.name;
            type.text = wallet._currency == Currency.UAH ? "UAH" : "USD";
            count.text = wallet._count.ToString();
        }
    }
}