using Managers;

namespace Windows.WindowsData
{
    public class GraphWindowData : WindowData
    {
        public GraphWindowData()
        {
            this.type = WindowType.FullScreen;
            this.behaviourNamespaceName = "Windows.HelperWindows";
        }
    }
}