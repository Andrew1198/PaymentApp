using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EmptyClicker : Graphic
    {
        protected override void Awake()
        {
            color = Color.clear;
        }
    }
}