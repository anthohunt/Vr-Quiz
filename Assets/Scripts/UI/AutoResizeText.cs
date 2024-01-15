using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TextMeshSettings
{
    public TextMeshProUGUI textMesh;
    public float minFontSize = 12f;
    public float maxFontSize = 24f;
}

public class AutoResizeText : MonoBehaviour
{
    [SerializeField] private List<TextMeshSettings> textMeshSettingsList;

    private void Update()
    {
        foreach (TextMeshSettings settings in textMeshSettingsList)
        {
            TextMeshProUGUI textMesh = settings.textMesh;
            float textLength = textMesh.text.Length;

            float minFontSize = settings.minFontSize;
            float maxFontSize = settings.maxFontSize;

            // Calculate the new font size based on text length
            float fontSize = Mathf.Lerp(maxFontSize, minFontSize, textLength / 10); // You can adjust this calculation as needed

            // Update the font size
            textMesh.fontSize = fontSize;
        }
    }
}
