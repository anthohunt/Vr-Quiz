using Fusion;
using System.Collections;
using UnityEngine;

public class NetworkAvatarSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NetworkRunner networkRunner;
    [SerializeField] private NetworkEvents networkEvents;
    [SerializeField] private UserEntitlement userEntitlement;

    [Header("Prefabs")]
    [SerializeField] private NetworkObject avatarPrefab;
    [SerializeField] private NetworkObject dataHandler;

    [Header("Ovr Rig")]
    [SerializeField] Transform cameraRigTransform;

    [Header("Spawn Points")]
    [SerializeField] Transform[] spawnPoints;

    private bool isServerConnected;
    private bool isEntitlementGranted;

    public int spawnedPosition;

    public AvatarEntityState localPlayer;
    public bool localPlayerHasSpawned;

    private void Awake()
    {
        networkEvents.OnConnectedToServer.AddListener(ConnectedToServer);
        userEntitlement.OnEntitlementGranted += EntitlementGranted;
    }

    private void OnDestroy()
    {
        networkEvents.OnConnectedToServer.RemoveListener(ConnectedToServer);
        userEntitlement.OnEntitlementGranted -= EntitlementGranted;
    }

    private void ConnectedToServer(NetworkRunner runner)
    {
        isServerConnected = true;
        TrySpawnAvatar();
    }

    private void EntitlementGranted()
    {
        isEntitlementGranted = true;
        TrySpawnAvatar();
    }

    private void TrySpawnAvatar()
    {
        if (isServerConnected || !isEntitlementGranted) return;

        StartCoroutine(SpawnAvatar());
    }

    private void SetPlayerSpawnPosition()
    {
        if (GameObject.Find("Remote Avatar"))
        {
            cameraRigTransform.SetPositionAndRotation(spawnPoints[1].position, spawnPoints[1].rotation);
            spawnedPosition = 2;
        }
        else
        {
            cameraRigTransform.SetPositionAndRotation(spawnPoints[0].position, spawnPoints[0].rotation);
            spawnedPosition = 1;
        }
    }

    //coroutine to spawn the avatar (wait 6 seconds otherwise the avatar does not load correctly)
    private IEnumerator SpawnAvatar()
    {
        yield return new WaitForSeconds(6f);
        
        var avatar = networkRunner.Spawn(avatarPrefab, cameraRigTransform.position, cameraRigTransform.rotation, networkRunner.LocalPlayer);
        avatar.transform.SetParent(cameraRigTransform);

        var dataH = networkRunner.Spawn(dataHandler, cameraRigTransform.position, cameraRigTransform.rotation);
        
        SetPlayerSpawnPosition();

        StartCoroutine(ModifyAvatarActiveView());
    }

    private IEnumerator ModifyAvatarActiveView()
    {
        yield return new WaitForSeconds(2f);
        if (!localPlayerHasSpawned)
        {
            localPlayer = GameObject.Find("Local Avatar").GetComponent<AvatarEntityState>();
            localPlayer.ConfigureAvatarToThirdPersonView();
            localPlayerHasSpawned = true;
        }
    }
}
