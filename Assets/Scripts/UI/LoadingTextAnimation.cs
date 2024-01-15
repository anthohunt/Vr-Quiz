using UnityEngine;
using TMPro;

public class LoadingTextAnimation : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public float animationSpeed = 1.0f; // Adjust the speed of the animation here

    private float timer;
    private int dotCount;

    private void Start()
    {
        timer = 0f;
        dotCount = 1;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= animationSpeed)
        {
            timer = 0f;
            dotCount = (dotCount % 3) + 1; // Cycle between 1, 2, and 3 for the dots

            // Update the text with "Loading..." and the appropriate number of dots
            loadingText.text = "Loading" + new string('.', dotCount);
        }
    }
}
