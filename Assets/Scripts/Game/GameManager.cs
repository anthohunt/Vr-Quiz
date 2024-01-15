using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    public GameManager gameManager;
    public EnvironmentManager environmentManager;
    public DisplayManager displayManager;
    public ButtonsVisuals buttonsVisuals;
    public NetworkAvatarSpawner networkAvatarSpawner;
    public PlayerProfile playerProfile;
    public QuestionReaderFromPrefab questionReaderFromPrefab;
    public DynamicSkyboxController dynamicSkyboxController;

    [Header("GameObjects")]
    public GameObject optionsButton;
    public GameObject blackCurtain;

    [Header("Client properties")]
    public bool isHost;
    
    [Header("Start")]
    public bool playersHaveBothJoined;
    public bool hasPressedStartLocal;
    public bool hasPressedStartRemote;
    public bool startGame;

    [Header("Questions")]
    public bool gameHasStarted;
    public bool questionHasBeenAnswered;
    public bool buzzerHasBeenHit;

    [Header("Game modes")]
    public int maxGameModes = 4;
    public int currentGamemodeIndex = 0;

    [Header("Network logic")]
    public bool localPlayerAnswerInput;
    public bool remotePlayerAnswerInput;
    public bool hasLocalHitBuzzer;
    public bool hasRemoteHitBuzzer; 
    public int localSelectedAnswer;
    public int remoteSelectedAnswer;
    public int localSelectedAnswerInGameMode2;
    public int remoteSelectedAnswerInGameMode2;
    public string previousQuestionPrefabIndex;
    public int previousQuestionIndex;
    public int previousShuffleSeed;
    public bool tempHotFix1;
    public bool isChoosingTheme;

    // invoked when plyaer press the START button
    public void ReadyToStart()
    {
        hasPressedStartLocal = true;
        displayManager.UpdateTitlePanel("waiting for remote player");
    }
    
    public void Update()
    {
        // wait until the scene is loaded, including the avatar, to display the scene to the player
        if (networkAvatarSpawner.localPlayerHasSpawned)
        {
            blackCurtain.SetActive(false);
        }

        // sets the first player to join the game to be HOST
        if (networkAvatarSpawner.spawnedPosition == 1)
        {
            isHost = true;
            optionsButton.SetActive(true);

            if (!gameHasStarted)
            {
                isChoosingTheme = true;
            }
        }

        // STEP 1: check if both players are in the game
        if (!playersHaveBothJoined)
        {
            // check if both players name are not empty
            if (playerProfile.localPlayerName != "" && playerProfile.remotePlayerName != "")
            {
                // prevent from entering this condition again
                playersHaveBothJoined = true;

                // update the main panel display
                displayManager.UpdateTitlePanel("players have joined");

                // stop the waiting animation
                dynamicSkyboxController.isDynamic = false;
            }
        }

        // STEP 2: check if both players are ready to start the game
        if (playersHaveBothJoined && !gameHasStarted)
        {
            // if both players have pressed start button
            if (hasPressedStartLocal && hasPressedStartRemote)
            {
                // update the main panel display
                displayManager.QuizPanelInteraction(false);              
                // prevent from entering this condition again
                gameHasStarted = true;
                startGame = true;
            }
        }

        // STEP 3: Start the game
        if (startGame)
        {
            StartCoroutine(WaitBeforeNextRound(5f));
            startGame = false;
        }

        // when both players have selected their answer (outside of buzzer mode), go to WaitBeforeRevealingAnswers
        if (localPlayerAnswerInput == true && remotePlayerAnswerInput == true && currentGamemodeIndex != 2 && !questionHasBeenAnswered)
        {       
            StartCoroutine(displayManager.WaitBeforeRevealingAnswers());
            questionHasBeenAnswered = true;
        }

        // get answer from remote player in buzzer mode
        if (hasRemoteHitBuzzer && remotePlayerAnswerInput && !questionHasBeenAnswered)
        {         
            StartCoroutine(displayManager.WaitBeforeRevealingAnswers());
            questionHasBeenAnswered = true;
        }

        // if remote player has input in buzzer mode, deactivate buzzer interactions
        if (hasRemoteHitBuzzer && !buzzerHasBeenHit)
        {           
            environmentManager.BuzzerActivation("allDeactivated");
            hasRemoteHitBuzzer = false;
        }

        // get the question data from host in order to display the same questions for both clients
        if (!isHost)
        {
            if (previousQuestionPrefabIndex != questionReaderFromPrefab.themePrefabName && previousQuestionIndex != questionReaderFromPrefab.questionIndex && previousShuffleSeed != displayManager.shuffleSeed)
            {
                displayManager.AskQuestion(false);
                previousQuestionPrefabIndex = questionReaderFromPrefab.themePrefabName;
                previousQuestionIndex = questionReaderFromPrefab.questionIndex;
                previousShuffleSeed = displayManager.shuffleSeed;
            }
        }
    }
    
    public IEnumerator WaitBeforeNextRound(float delayInSeconds)
    {
        // check if the game is not finished
        if (currentGamemodeIndex < maxGameModes)
        {
            // disable the quiz panel
            environmentManager.DisplayQuestionPanel(false);
            // go to next game mode
            currentGamemodeIndex++;
            // update the environment accordingly
            environmentManager.UpdateEnvironment(currentGamemodeIndex);
            // then wait for a few seconds
            yield return new WaitForSeconds(delayInSeconds);
            // go to the next round
            NextRound();
        }
        else
        {
            // 06/12/2023: Found a bug when the second client is at the last question, it will go straight to the end screen, skipping the last question. No time to investigate why, so quick hotfix
            if (gameManager.isHost)
            {
                // update the environment to end game
                environmentManager.UpdateEnvironment(-1);
                // update the quiz panel accordingly
                displayManager.EndGame();
            }
            else
            {
                if (tempHotFix1)
                {
                    // update the environment to end game
                    environmentManager.UpdateEnvironment(-1);
                    // update the quiz panel accordingly
                    displayManager.EndGame();
                }
                else
                {
                    // temporarily disable the quiz panel
                    environmentManager.DisplayQuestionPanel(false);
                    // go to next game mode
                    currentGamemodeIndex++;
                    // update the environment accordingly
                    environmentManager.UpdateEnvironment(currentGamemodeIndex);
                    // then wait for a few seconds
                    yield return new WaitForSeconds(delayInSeconds);
                    // go to the next round
                    NextRound();
                }
                tempHotFix1 = true;
            }
        }
    }

    public void NextRound()
    {
        displayManager.SelectTheme();
        
        // if host, get a new question, if not, wait for the update from remote client
        if (isHost)
        {
            //displayManager.AskQuestion(true);
        }
        // if buzzer mode, activate buzzer interactions
        if (currentGamemodeIndex == 2)
        {
            environmentManager.BuzzerActivation("allActivated");
        }
    }

    // called when an answer is selected from the local player
    public void ConfirmAnswers(int answer)  
    {
        // values updated for the Update method
        localPlayerAnswerInput = true;
        localSelectedAnswer = answer;

        displayManager.QuizPanelInteraction(false);

        // in case of buzzer mode, don't wait for remote answer and go straight to WaitBeforeRevealingAnswers
        if (hasLocalHitBuzzer)
        {
            StartCoroutine(displayManager.WaitBeforeRevealingAnswers());
        }
        if (currentGamemodeIndex == 3) 
        {
            displayManager.timerWhenAnswerHasBeenSelected = displayManager.timeRemaining;
        }     
        
        if (currentGamemodeIndex != 2)
        {
            buttonsVisuals.ButtonsWaitingForAnswer();
        }
    }

    public void ResetVariables()
    {
        hasLocalHitBuzzer = false;
        hasRemoteHitBuzzer = false;
        localPlayerAnswerInput = false;
        remotePlayerAnswerInput = false;
        localSelectedAnswerInGameMode2 = -1;
        remoteSelectedAnswerInGameMode2 = -1;
        localSelectedAnswer = -1;
        remoteSelectedAnswer = -1;
        questionHasBeenAnswered = false;
        buzzerHasBeenHit = false;
        previousQuestionPrefabIndex = "";
        questionReaderFromPrefab.themePrefabName = "";
        previousQuestionIndex = -1;
        questionReaderFromPrefab.questionIndex = -1;
        environmentManager.selectedTheme = "";
        environmentManager.remoteSelectedTheme = "";
    }
    public void HasInputInGamemode2() 
    {
        if (!environmentManager.buzzersHaveBeenSelected)
        {
            displayManager.QuizPanelInteraction();
            hasLocalHitBuzzer = true;
            environmentManager.BuzzerActivation("allDeactivated");
        }
    }
    
    public void ChangeGameMode()
    {
        if (currentGamemodeIndex == 1)
        {
            environmentManager.UpdateEnvironment(1);
        }
        if (currentGamemodeIndex == 2)
        {
            environmentManager.UpdateEnvironment(2);
        }
        if (currentGamemodeIndex == 3)
        {
            environmentManager.UpdateEnvironment(3);
        }
        if (currentGamemodeIndex == 4)
        {
            environmentManager.UpdateEnvironment(4);
        }
    }
}
