using Fusion;
using UnityEngine;

public class NetworkDataHandler : NetworkBehaviour
{
    [SerializeField] private NetworkObject networkObject;
    private PlayerProfile playerProfile;
    private GameManager gameManager;
    public QuestionReaderFromPrefab questionReaderFromPrefab;
    private DisplayManager displayManager;
    public EnvironmentManager environmentManager;

    [Networked] public string playerName { get; set; }
    [Networked] public int playerScore { get; set; }
    [Networked] public int selectedAnswer { get; set; }
    [Networked] public bool hasSelectedAnswer { get; set; }
    [Networked] public bool hasLocalHitBuzzer { get; set; }
    [Networked] public bool readyToPlay { get; set; }
    [Networked, Capacity(100)] public string theme { get; set; }
    [Networked] public int questionIndex { get; set; }
    [Networked, Capacity(20)] public int shuffleSeed { get; set; }
    [Networked] public int numberOfQuestions { get; set; }
    [Networked] public float timer { get; set; }
    [Networked, Capacity(100)] public string remoteTheme { get; set; }




    private void Start()
    {
        playerProfile = GameObject.Find("Game Manager").GetComponent<PlayerProfile>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        questionReaderFromPrefab = GameObject.Find("Game Manager").GetComponent<QuestionReaderFromPrefab>();
        displayManager = GameObject.Find("Game Manager").GetComponent<DisplayManager>();
    }

    void FixedUpdate()
    {
        if (networkObject.HasStateAuthority)
        {
            gameObject.name = "Local Data Handler";
            readyToPlay = gameManager.hasPressedStartLocal;
            playerName = playerProfile.localPlayerName;
            playerScore = playerProfile.localScore;
            
            selectedAnswer = gameManager.localSelectedAnswer;
            hasSelectedAnswer = gameManager.localPlayerAnswerInput;
            hasLocalHitBuzzer = gameManager.hasLocalHitBuzzer;

            if (gameManager.isHost)
            {
                theme = questionReaderFromPrefab.themePrefabName;
                questionIndex = questionReaderFromPrefab.questionIndex;
                shuffleSeed = displayManager.shuffleSeed;
                numberOfQuestions = displayManager.maxQuestionsPerRound;

                if (!gameManager.gameHasStarted)
                {
                    timer = displayManager.timer;
                }
            }
            else
            {
                remoteTheme = environmentManager.remoteSelectedTheme;
            }
        }
        else
        {
            gameObject.name = "Remote Data Handler";
            gameManager.hasPressedStartRemote = readyToPlay;
            playerProfile.remotePlayerName = playerName;
            playerProfile.remoteScore = playerScore;
            
            gameManager.remoteSelectedAnswer = selectedAnswer;
            gameManager.remotePlayerAnswerInput = hasSelectedAnswer;
            gameManager.hasRemoteHitBuzzer = hasLocalHitBuzzer;

            if(gameManager.isHost) 
            {
                environmentManager.remoteSelectedTheme = remoteTheme;
            }

            if (!gameManager.isHost)
            {
                questionReaderFromPrefab.themePrefabName = theme;
                questionReaderFromPrefab.questionIndex = questionIndex;
                displayManager.shuffleSeed = shuffleSeed;
                displayManager.maxQuestionsPerRound = numberOfQuestions;

                if (!gameManager.gameHasStarted)
                {
                    displayManager.timer = timer;
                }
            }
        }
    }
}
