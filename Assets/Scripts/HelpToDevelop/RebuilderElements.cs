using UnityEditor;
using UnityEngine;
   namespace HelpToDevelop
   {
       
       public class RebuilderElements : MonoBehaviour
       {

       }


       [CustomEditor(typeof(RebuilderElements))]
       public class LayoutRebuilderEditor : UnityEditor.Editor
       {

           public override void OnInspectorGUI()
           {
               if (GUILayout.Button("Rebuild Object"))
               {
                   var rectTranforms = ((UnityEngine.MonoBehaviour) target)
                       .GetComponentsInChildren<UnityEngine.RectTransform>();
                   foreach (var rectTranform in rectTranforms)
                       UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rectTranform);
               }
           }
       }
   }
    
    
    