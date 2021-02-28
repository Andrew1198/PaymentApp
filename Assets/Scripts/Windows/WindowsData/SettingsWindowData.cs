using Managers;

namespace Windows.WindowsData
{
    public class SettingsWindowData : WindowData
    {
        public SettingsWindowData()
        {
            this.type = WindowType.FullScreen;
            this.behaviourNamespaceName = "Windows.HelperWindows.SettingWindow";
        }
    }
}