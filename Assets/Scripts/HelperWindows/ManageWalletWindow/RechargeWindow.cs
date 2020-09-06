using DefaultNamespace;
using Managers;

namespace HelperWindows.ManageWalletWindow
{
    public class RechargeWindow : ManageWalletBase
    {
        public override void OnOk()
        {
            SelectedWallet._count = int.Parse(countField.text);
            Events.OnUpdateTab?.Invoke();
            OnClose();
        }
    }
}