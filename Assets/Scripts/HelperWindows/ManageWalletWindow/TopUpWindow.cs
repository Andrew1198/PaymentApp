using DefaultNamespace;
using Managers;

namespace HelperWindows.ManageWalletWindow
{
    public class TopUpWindow : ManageWalletBase
    {
        public override void OnOk()
        {
            SelectedSaving.count += int.Parse(countField.text);
            Events.OnUpdateTab?.Invoke();
            OnClose();
        }
    }
}