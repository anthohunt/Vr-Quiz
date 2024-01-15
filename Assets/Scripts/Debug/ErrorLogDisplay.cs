using TMPro;
using UnityEngine;

public class ErrorLogDisplay : MonoBehaviour
{
    public TextMeshProUGUI errorText;

    void OnEnable()
    {
        // Redirect Unity's log messages to your TextMeshPro Text
        Application.logMessageReceived += LogMessage;
    }

    void OnDisable()
    {
        // Unsubscribe from the log messages when the script is disabled or the object is destroyed
        Application.logMessageReceived -= LogMessage;
    }

    void LogMessage(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            // Append error or exception messages along with the stack trace to the TextMeshPro Text
            errorText.text += $"\n{logString}\n{stackTrace}";
        }
    }
}
