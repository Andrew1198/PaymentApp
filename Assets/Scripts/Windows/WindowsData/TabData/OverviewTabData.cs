using Managers;

namespace Windows.WindowsData.TabData
{
    public class OverviewTabData : TabData
    {
        public OverviewTabData()
        {
            this.type = WindowType.Tab;
            this.behaviourNamespaceName = "Windows.Tabs";
        }
    }
}