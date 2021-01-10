using TMPro;
using UnityEngine;

public class TestSomeScripts : MonoBehaviour
{
    private void SetAligment()
    {
        var objects = FindObjectsOfType<TextMeshProUGUI>();

        foreach (var textMeshProUgui in objects) textMeshProUgui.alignment = TextAlignmentOptions.Center;
    }
}