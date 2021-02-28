using System.Collections.Generic;
using Windows.WindowsData;
using Managers;
using UnityEngine;

namespace Windows.HelperWindows
{
    public class WindowBase : MonoBehaviour
    {
        public State state;
        
        public WindowData data; 
        public virtual void Open(Dictionary<string,object>DynamicWindowData = null)
        {
            state = State.Opened;
            gameObject.SetActive(true);
        }
        public virtual void Close()
        {
            state = State.Closed;
            gameObject.SetActive(false);
        }
        
        public enum State
        {
            Opened,
            Closed
        }
    }
}