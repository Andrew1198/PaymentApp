using Managers;

namespace HelperWindows.ManageWalletWindow
{
    public class TopUpWindow : ManageWalletBase
    {
        public void OnOk()
        {
            SelectedWallet._count += int.Parse(countField.text);
            TabManager.UpdateTab();
            OnClose();
        }
    }
}