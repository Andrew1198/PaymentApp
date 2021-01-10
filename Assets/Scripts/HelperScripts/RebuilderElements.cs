﻿using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace HelperScripts
{
    public class RebuilderElements : MonoBehaviour
    {
        [Button]
        private void RebuildOneObject()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        [Button]
        private void RebuildAllChildren()
        {
            var rectTranforms = (transform as RectTransform)
                .GetComponentsInChildren<RectTransform>();
            foreach (var rectTranform in rectTranforms)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTranform);
        }
    }
}