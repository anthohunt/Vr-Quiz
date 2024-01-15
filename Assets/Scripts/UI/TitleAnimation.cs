using TMPro;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public TMP_Text textToAnimate; // Use TMP_Text for TextMeshProUGUI
    public float minSize = 20f;
    public float maxSize = 30f;
    public float changeSpeed = 1f;

    private bool increasingSize = true;
    private float currentSize;

    void Start()
    {
        if (textToAnimate == null)
        {
            Debug.LogError("Text component not assigned!");
            enabled = false; // Disable the script if Text is not assigned
            return;
        }

        currentSize = minSize;
        textToAnimate.fontSize = currentSize;
    }

    void Update()
    {
        if(textToAnimate.gameObject.activeInHierarchy == true && !GameObject.Find("Game Manager").GetComponent<GameManager>().playersHaveBothJoined)
        {
            AnimateTextSize();
        }
        else
        {
            textToAnimate.fontSize = 5;
        }
    }

    void AnimateTextSize()
    {
        if (increasingSize)
        {
            currentSize += changeSpeed * Time.deltaTime;
            if (currentSize >= maxSize)
            {
                currentSize = maxSize;
                increasingSize = false;
            }
        }
        else
        {
            currentSize -= changeSpeed * Time.deltaTime;
            if (currentSize <= minSize)
            {
                currentSize = minSize;
                increasingSize = true;
            }
        }

        textToAnimate.fontSize = currentSize;
    }
}
