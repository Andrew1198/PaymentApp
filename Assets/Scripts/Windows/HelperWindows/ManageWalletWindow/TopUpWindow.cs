using DefaultNamespace;
using Managers;

namespace Windows.HelperWindows.ManageWalletWindow
{
    public class TopUpWindow : ManageWalletBase
    {
        public override void OnOk()
        {
            SelectedSaving.count += int.Parse(countField.text);
            TabManager.UpdateOpenedTab(); 
            OnClose();
        }
    }
}