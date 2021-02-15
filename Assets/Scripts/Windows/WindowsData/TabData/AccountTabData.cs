using Managers;

namespace Windows.WindowsData.TabData
{
    public class AccountTabData : TabData
    {
        public AccountTabData()
        {
            behaviourNamespaceName = "Windows.Tabs";
            type = WindowType.Tab;
        }
    }
}