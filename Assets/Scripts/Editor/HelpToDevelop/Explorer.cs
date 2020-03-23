using UnityEditor;
using UnityEngine;

namespace HelpToDevelop
{
    public class Explorer : MonoBehaviour
    {
        [MenuItem("MyMenu/ShowPersistentDataPath")]
        static void ShowPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}