using DefaultNamespace;
using Managers;

namespace Windows.HelperWindows.ManageWalletWindow
{
    public class RechargeWindow : ManageWalletBase
    {
        public override void OnOk()
        {
            SelectedSaving.count = int.Parse(countField.text);
            TabManager.UpdateOpenedTab();
            OnClose();
        }
    }
}