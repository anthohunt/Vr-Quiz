using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliisionHandler : MonoBehaviour
{
    public NetworkAvatarSpawner networkAvatarSpawner;
    public Transform cameraRigTransform;
    public Transform[] spawnPoints;
    
    // This method is called when a collision occurs.
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the "Player" tag.
        if (collision.gameObject.CompareTag("Player"))
        {
            cameraRigTransform.SetPositionAndRotation(spawnPoints[networkAvatarSpawner.spawnedPosition - 1].position, spawnPoints[networkAvatarSpawner.spawnedPosition - 1].rotation);
        }
    }
}
