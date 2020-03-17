using System;
using UnityEngine;

namespace Managers
{
    public class TimeManager : Singleton<TimeManager>
    {
        public static DateTime CurrentTime => DateTime.Now;
        public static int CurrentMonth => DateTime.Now.Month;

        public void OnPreviousMonth()
        {
            PlayerData.SelectedDate = PlayerData.SelectedDate.AddMonths(-1);
            TabManager.UpdateTab();
        }
        
        public void OnNextMonth()
        {
            PlayerData.SelectedDate = PlayerData.SelectedDate.AddMonths(1);
            TabManager.UpdateTab();
        }
    }
}