using Managers;

namespace Windows.WindowsData
{
    public class DeletedTransactionWindowData : WindowData
    {
        public DeletedTransactionWindowData()
        {
            type = WindowType.FullScreen;
            behaviourNamespaceName = "Windows.HelperWindows.SettingWindow";
        }
    }
}