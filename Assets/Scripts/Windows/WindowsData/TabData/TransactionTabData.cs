using Managers;

namespace Windows.WindowsData.TabData
{
    public class TransactionTabData : TabData
    {
        public TransactionTabData()
        {
            this.type = WindowType.Tab;
            this.behaviourNamespaceName = "Windows.Tabs";
        }
    }
}