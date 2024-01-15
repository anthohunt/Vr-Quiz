using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Unity;

public class ConnectionManager : MonoBehaviour
{
    private NetworkRunner networkRunner;
    private NetworkEvents networkEvents;

    private NetworkObject[] networkObjectArray;

    private void Awake()
    {
        networkObjectArray = GetNetworkObjects();

        networkRunner = GetComponent<NetworkRunner>();
        networkEvents = GetComponent<NetworkEvents>();

        networkEvents.OnConnectedToServer.AddListener(RegisterNetworkObjects);
    }

    private void OnDestroy() => networkEvents.OnConnectedToServer.RemoveListener(RegisterNetworkObjects);

    private void Start()
    {
        networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = SceneManager.GetActiveScene().buildIndex.ToString()
            
        });
    }

    private NetworkObject[] GetNetworkObjects() => FindObjectsOfType<NetworkObject>();

    private void RegisterNetworkObjects(NetworkRunner runner) => runner.RegisterSceneObjects(networkObjectArray);
}
