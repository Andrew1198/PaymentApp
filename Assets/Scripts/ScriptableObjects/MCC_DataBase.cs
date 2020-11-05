using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [UnityEngine.CreateAssetMenu(fileName = "MCC_DataBase", menuName = "ScriptableObjects/MCC_DataBase", order = 0)]
    public class MCC_DataBase : UnityEngine.ScriptableObject
    {
        public List<MccData> mccDataBase = new List<MccData>();
        
        [Serializable]
        public class MccData
        {
            public int Code;
            public string Description;
        }

        public string GetDescriptionByMccCode(int code)
        {
            var result = mccDataBase.FirstOrDefault(item => item.Code == code);
            return result == null ? "Unknown mcc code" : result.Description;
        }


        [Button]
        public void UpdateFromDoc()
        {
            mccDataBase.Clear();
            using (var sr = new StreamReader("Assets/Data/codes.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var match = Regex.Match(line, @"(\d+)(.+)");
                    mccDataBase.Add(new MccData
                    {
                        Code = int.Parse(match.Groups[1].Value),
                        Description = match.Groups[2].Value
                    });
                }
            }
            Debug.LogError("Finish");
            
        }
    }
}