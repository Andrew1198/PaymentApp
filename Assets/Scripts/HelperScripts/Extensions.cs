﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace HelperScripts
{
    public static class Extensions
    {
        public static void DeleteChildren(this Transform tr)
        {
            foreach (Transform child in tr)
                Object.Destroy(child.gameObject);
        }
    }
}