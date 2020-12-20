using UnityEngine;
#pragma warning disable 0649
namespace HelperScripts
{
    public class JsonHelper
    {
        public static T[] GetJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }
 
        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}