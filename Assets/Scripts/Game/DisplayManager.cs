using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class DisplayManager : MonoBehaviour
{
    [Header("Components")]
    public GameManager gameManager;
    public PlayerProfile playerProfile;
    public EnvironmentManager environmentManager;
    public ButtonsVisuals buttonsVisuals;
    public QuestionReaderFromPrefab questionReaderFromPrefab;
    public ThemeSelector themeSelector;

    [Header("GameObjects")]
    public GameObject quizCanvas;
    public GameObject handRayInteractor;
    public GameObject controllerRayInteractor;
    public GameObject startButton;

    [Header("Buttons")]
    public UnityEngine.UI.Button buttonA;
    public UnityEngine.UI.Button buttonB;
    public UnityEngine.UI.Button buttonC;
    public UnityEngine.UI.Button buttonD;
    public UnityEngine.UI.Slider numberOfQuestionsSlider;
    public UnityEngine.UI.Slider timerSlider;

    [Header("Texts")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answerAText;
    public TextMeshProUGUI answerBText;
    public TextMeshProUGUI answerCText;
    public TextMeshProUGUI answerDText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI questionNumberText;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI winnerText;

    [Header("Audio")]
    public AudioSource winSound;
    public AudioSource loseSound;

    [Header("Variables")]
    public int maxQuestionsPerRound = 5;
    public int currentQuestionIndex;
    public int correctAnswerIndex;
    public int questionPrefabIndex;
    public string correctAnswer;
    private float startTime;
    public float timer;
    public float timeRemaining;
    [HideInInspector] public float timerWhenAnswerHasBeenSelected;
    private bool timerIsRunning = false;
    List<string> answerOptions = new List<string>();
    [HideInInspector] public int shuffleSeed;
    public bool hasUpdatedMaxQuestionsOnce;
    public List<string> randomThemes;

    private void Start()
    {
        // players can't interact with the quiz panel
        QuizPanelInteraction(false);

        // Ensure the slider and text components are properly assigned
        if (numberOfQuestionsSlider != null && timerSlider != null)
        {
            // Subscribe to the slider's OnValueChanged event
            numberOfQuestionsSlider.onValueChanged.AddListener(delegate { UpdateOptionSettings(); });
            timerSlider.onValueChanged.AddListener(delegate { UpdateOptionSettings(); });

            // Initialize text with slider's initial value
            UpdateOptionSettings();
        }
        else
        {
            Debug.LogError("Slider or Text component not assigned!");
        }
    }

    public void UpdateTitlePanel(string state)
    {
        if (state == "players have joined")
        {
            startButton.SetActive(true);
            instructionText.color = Color.green;
            instructionText.text = "Both players have joined.";
            QuizPanelInteraction(true);
        }
        
        if (state == "waiting for remote player")
        {
            instructionText.text = "Waiting for the other player to press start...";
            instructionText.color = Color.grey;
        }
    }

    public void UpdateOptionSettings()
    {
        maxQuestionsPerRound = (int)numberOfQuestionsSlider.value;
        timer = timerSlider.value;    
    }
    
    public void Update()
    {         
        // If the timer is running
        if (timerIsRunning)
        {
            // Calculate time remaining based on the elapsed time.
            timeRemaining = timer - (Time.time - startTime);
            if (timeRemaining > 0)
            {
                timerText.text = "Time: " + timeRemaining.ToString("F1");
            }
            else
            {
                // If time is up, stop the timer, select wrong answer
                timerIsRunning = false;
                CheckAnswers(-1);
            }
        }

        if(gameManager.isHost && environmentManager.remoteSelectedTheme!= null) 
        {
            environmentManager.selectedTheme = environmentManager.remoteSelectedTheme;
            AskQuestion(true);
        }
    }

    public void SelectTheme()
    {
        randomThemes = themeSelector.SelectRandomThemes(4);
        environmentManager.DisplayThemePanel(randomThemes);
        // Allow quiz panel interaction
        if(gameManager.isChoosingTheme)
        {
            QuizPanelInteraction();
        }
    }

    public void AskQuestion(bool isHost)
    {
        // display the questions
        environmentManager.DisplayQuestionPanel();
        // reinitialise the buttons
        buttonsVisuals.ReinitialiseButtonsVisuals();

        QuizPanelInteraction();

        // If questions can be read
        if (questionReaderFromPrefab != null)
        {
            // update the question number text
            var questionindexToPrint = currentQuestionIndex + 1;
            questionNumberText.text = "Question: " + questionindexToPrint + "/" + maxQuestionsPerRound;

            // Start the timer.
            startTime = Time.time;
            timerIsRunning = true;

            if (isHost) 
            {
               // questionReaderFromPrefab.CallRandomTheme(); // this will be called directly after players have moved to next round or next question
                questionReaderFromPrefab.CallSelectedTheme(environmentManager.selectedTheme);
            }
            else
            {
                questionReaderFromPrefab.CallThemeFromIndex(); // this will be called once the host has selected a question
            }

            questionPrefabIndex = questionReaderFromPrefab.questionIndex;

            // Set the answers
            correctAnswer = questionReaderFromPrefab.trueAnswers[questionPrefabIndex];
            answerOptions.Clear();
            answerOptions.Add(correctAnswer);
            answerOptions.AddRange(questionReaderFromPrefab.falseAnswers[questionPrefabIndex]);

            if (gameManager.isHost)
            {
                // Generate a random seed based on the current time, this will be used so both players get the same "randomness"
                shuffleSeed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            }
            
            Shuffle(answerOptions, shuffleSeed);

            // Display the question and answer options.
            questionText.text = questionReaderFromPrefab.questions[questionPrefabIndex];
            answerAText.text = "A: " + answerOptions[0];
            answerBText.text = "B: " + answerOptions[1];
            answerCText.text = "C: " + answerOptions[2];
            answerDText.text = "D: " + answerOptions[3];

            // Find the index of the correct answer in the shuffled options.
            for (int i = 0; i <= 3; i++)
            {
                if (correctAnswer == answerOptions[i])
                {
                    correctAnswerIndex = i;
                }
            }
        }

        // set it initially to -1, as a wrong answer
        gameManager.localSelectedAnswer = -1;
        gameManager.remoteSelectedAnswer = -1;

        // Handle specific actions based on the current game mode.
        if (gameManager.currentGamemodeIndex == 2)
        {
            // If buzzer mode, disable quiz panel interaction.
            QuizPanelInteraction(false);
        }

        if (gameManager.currentGamemodeIndex == 4)
        {
            // If the game mode is 4, shuffle the words in the question text.
            questionText.text = ShuffleWords(questionText.text, shuffleSeed);
        }
    }

    public IEnumerator WaitBeforeRevealingAnswers()
    {
        buttonsVisuals.ActivateSelectedAnswer(gameManager.localSelectedAnswer, "localPlayer");
        buttonsVisuals.ActivateSelectedAnswer(gameManager.remoteSelectedAnswer, "remotePlayer");
        QuizPanelInteraction(false);
        environmentManager.BuzzerActivation("allDeactivated");
        
        timerIsRunning = false;
        
        yield return new WaitForSeconds(2f);
        
        CheckAnswers(gameManager.localSelectedAnswer);
    }
    
    public void CheckAnswers(int selectedAnswer)
    {   
        // Check if the selected answer matches the correct index.
        if (selectedAnswer == correctAnswerIndex)
        {
            winSound.Play();
            playerProfile.IncreaseLocalPlayerScore();
        }
        else
        {
            loseSound.Play();
            playerProfile.DecreaseLocalPlayerScore();
        }

        //reset variables for next round
        gameManager.ResetVariables();
        
        //display explanations
        questionText.text = questionReaderFromPrefab.explanations[questionPrefabIndex];
        
        // show the correct answer
        buttonsVisuals.UpdateCorrectAnswerButtonColor(correctAnswerIndex);

        StartCoroutine(GoToNextQuestion(5f));
    }

    private IEnumerator GoToNextQuestion(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        buttonsVisuals.ReinitialiseButtonsVisuals();
        
        if(gameManager.currentGamemodeIndex == 2)
        {
            environmentManager.BuzzerActivation("allActivated");
        }

        if (currentQuestionIndex < maxQuestionsPerRound - 1)
        {
            currentQuestionIndex++;
            
            // will be alternated with player 1 and 2
            if(gameManager.isChoosingTheme)
            {
                gameManager.isChoosingTheme = false;
            }
            else
            {
                gameManager.isChoosingTheme = true;
            }

            SelectTheme();

            if (gameManager.isHost)
            {
               // AskQuestion(true);
            }
        }
        else
        {
            currentQuestionIndex = 0;
            gameManager.StartCoroutine(gameManager.WaitBeforeNextRound(5f));
        }
    }
    
    public void QuizPanelInteraction(bool state = true)
    {
        handRayInteractor.SetActive(state);
        controllerRayInteractor.SetActive(state);
    }

    public void EndGame() 
    {
        environmentManager.DisplayQuestionPanel(false);
        
        if (playerProfile.localScore > playerProfile.remoteScore)
        {
            winnerText.text = playerProfile.localPlayerName;
            environmentManager.PlaceCrown("local");
        }
        else if (playerProfile.localScore < playerProfile.remoteScore)
        {
            winnerText.text = playerProfile.remotePlayerName;
            environmentManager.PlaceCrown("remote");
        }
        else
        {
            winnerText.text = "It's a draw!";
        }
    }

    private void Shuffle<T>(List<T> list, int seed)
    {
        // Seed the random number generator
        Random.InitState(seed);
        
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static string ShuffleWords(string inputSentence, int seed)
    {
        // Split the sentence into words
        string[] words = inputSentence.Split(' ');

        // Seed the random number generator
        Random.InitState(seed);

        // Use the Fisher-Yates shuffle algorithm to shuffle the words
        for (int i = words.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = words[i];
            words[i] = words[j];
            words[j] = temp;
        }

        // Combine the shuffled words back into a sentence
        string shuffledSentence = string.Join(" ", words);

        return shuffledSentence;
    }
}
