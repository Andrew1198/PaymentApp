using DefaultNamespace;

namespace Windows.HelperWindows.ManageWalletWindow
{
    public class RechargeWindow : ManageWalletBase
    {
        public override void OnOk()
        {
            SelectedSaving.count = int.Parse(countField.text);
            Events.OnUpdateTab?.Invoke();
            OnClose();
        }
    }
}