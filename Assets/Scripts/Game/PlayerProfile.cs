using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class PlayerProfile : MonoBehaviour
{
    [Header("Components")]
    public GameManager gameManager;
    public DisplayManager displayManager;
    public UserEntitlement userEntitlement;
    public NetworkAvatarSpawner networkAvatarSpawner;

    [Header("Texts")]
    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2NameText;
    public TextMeshProUGUI player2ScoreText;

    [Header("Player info")]
    public int localScore;
    public int remoteScore;
    public string localPlayerName;
    public string remotePlayerName;

    private void Start()
    {
        localScore = 0;
        remoteScore = 0;
    }

    private void Update()
    {
        // Keep the scores to minimum 0
        if (localScore < 0)
        {
            localScore = 0;
        }
        if (remoteScore < 0)
        {
            remoteScore = 0;
        }

        // get the player name
        localPlayerName = userEntitlement.displayName;

        // update the appropriate texts
        if (networkAvatarSpawner.spawnedPosition == 1) 
        {
            player1NameText.text = userEntitlement.displayName;
            player1ScoreText.text = localScore.ToString();
        }
        if (networkAvatarSpawner.spawnedPosition == 2)
        {
            player2NameText.text = userEntitlement.displayName;
            player2ScoreText.text = localScore.ToString();
        }

        // check if connected to the other player
        if (GameObject.Find("Remote Data Handler") != null)
        {
            var remoteDataHandler = GameObject.Find("Remote Data Handler").GetComponent<NetworkDataHandler>();
            
            // and assign the remote info to the appropriate texts
            if (networkAvatarSpawner.spawnedPosition == 1)
            {
                remotePlayerName = remoteDataHandler.playerName;
                player2NameText.text = remotePlayerName;

                remoteScore = remoteDataHandler.playerScore;
                player2ScoreText.text = remoteScore.ToString();
            }
            if (networkAvatarSpawner.spawnedPosition == 2)
            {
                remotePlayerName = remoteDataHandler.playerName;
                player1NameText.text = remotePlayerName;

                remoteScore = remoteDataHandler.playerScore;
                player1ScoreText.text = remoteScore.ToString();
            }
        }
    }

    public void IncreaseLocalPlayerScore()
    {
        // Update the player's score based on the current game mode.
        if (gameManager.currentGamemodeIndex == 1 || gameManager.currentGamemodeIndex == 2)
        {
            localScore = localScore + 100;
        }
        if (gameManager.currentGamemodeIndex == 3)
        {
            localScore = localScore + 100 + Mathf.RoundToInt(displayManager.timerWhenAnswerHasBeenSelected) * 10;
        }
        if (gameManager.currentGamemodeIndex == 4)
        {
            localScore = localScore + 150;
        }
    }

    public void DecreaseLocalPlayerScore()
    {
        if (gameManager.currentGamemodeIndex == 2 && gameManager.hasLocalHitBuzzer)
        {
            localScore = localScore - 100;
        }
    }
}
