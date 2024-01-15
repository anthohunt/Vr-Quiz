using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsTextSlider : MonoBehaviour
{
    public Slider slider; // Reference to your Slider component
    public TextMeshProUGUI textValue; // Reference to your Text component

    private void Start()
    {
        // Ensure the slider and text components are properly assigned
        if (slider != null && textValue != null)
        {
            // Subscribe to the slider's OnValueChanged event
            slider.onValueChanged.AddListener(delegate { UpdateTextValue(); });

            // Initialize text with slider's initial value
            UpdateTextValue();
        }
        else
        {
            Debug.LogError("Slider or Text component not assigned!");
        }
    }

    // Update the text value based on the slider's value
    public void UpdateTextValue()
    {
        textValue.text = slider.value.ToString(); // Format the value as needed
    }
}
