using Managers;

namespace Data.WindowData
{
    public class ConfirmWindowData : Windows.WindowsData.WindowData
    {
        public ConfirmWindowData()
        {
            type = WindowType.FullScreen;
            behaviourNamespaceName = "Windows.HelperWindows";
        }
    }
}