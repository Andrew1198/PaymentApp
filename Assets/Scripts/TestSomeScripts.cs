using NaughtyAttributes;
using UnityEngine;

public class TestSomeScripts : MonoBehaviour
{
    private void SetAligment()
    {
        var objects = FindObjectsOfType<TMPro.TextMeshProUGUI>();

        foreach (var textMeshProUgui in objects)
        {
            textMeshProUgui.alignment = TMPro.TextAlignmentOptions.Center;
        }
    }
    
}