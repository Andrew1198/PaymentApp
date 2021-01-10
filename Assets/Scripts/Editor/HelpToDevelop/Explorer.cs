using UnityEditor;
using UnityEngine;

namespace HelpToDevelop
{
    public class Explorer : MonoBehaviour
    {
        [MenuItem("MyMenu/ShowPersistentDataPath")]
        private static void ShowPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}