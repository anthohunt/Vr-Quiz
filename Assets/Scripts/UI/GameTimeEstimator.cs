using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameTimeEstimator : MonoBehaviour
{
    public Slider numberOfQuestions;
    public Slider timer;
    public TextMeshProUGUI timeEstimation;
    public GameManager gameManager;
    
    // Update is called once per frame
    void Update()
    {
        timeEstimation.text = "Duration of the game: " + (Mathf.Round(numberOfQuestions.value * timer.value * gameManager.maxGameModes / 60)).ToString("F2") + " minutes.";
    }
}
