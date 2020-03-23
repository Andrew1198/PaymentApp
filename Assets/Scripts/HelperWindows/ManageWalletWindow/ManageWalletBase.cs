using TMPro;
using UnityEngine;

namespace HelperWindows.ManageWalletWindow
{
    public abstract class ManageWalletBase
    {
        [SerializeField] private ManageWalletWindow _manageWalletWindow;
        [SerializeField] private TextMeshProUGUI _selectedWalletText;


        protected virtual void Init()
        {
            
        }
        
        protected virtual void OnOk()
        {
            
        }
        
        protected virtual void OnClose()
        {
            
        }
        
        protected virtual void OnReset()
        {
            
        }
    }
}