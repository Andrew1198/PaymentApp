using Managers;

namespace Windows.WindowsData.TabData
{
    public class TransactionsTabData : TabData
    {
        public TransactionsTabData()
        {
            this.type = WindowType.Tab;
            this.behaviourNamespaceName = "Windows.Tabs";
        }
    }
}