using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    public NetworkAvatarSpawner networkAvatarSpawner;
    public Transform cameraShow1;
    public Transform cameraShow2;

    public bool hasAssignedToLocal;
    public bool hasAssignedToRemote;

    void Update()
    {
        if(!hasAssignedToLocal)
        {
            if (networkAvatarSpawner.spawnedPosition == 1 && GameObject.Find("Local Avatar"))
            {
                cameraShow1.transform.SetParent(GameObject.Find("Local Avatar").transform.GetChild(1));
                cameraShow1.localPosition = new Vector3(-0.15f, 0.85f, 0.1f);
                cameraShow1.localEulerAngles = new Vector3(76, 112, 21);

                hasAssignedToLocal = true;
            }

            if (networkAvatarSpawner.spawnedPosition == 2 && GameObject.Find("Local Avatar"))
            {
                cameraShow2.transform.SetParent(GameObject.Find("Local Avatar").transform.GetChild(1));
                cameraShow2.localPosition = new Vector3(-0.15f, 0.85f, 0.1f);
                cameraShow2.localEulerAngles = new Vector3(76, 112, 21);

                hasAssignedToLocal = true;
            }
        }

        if (!hasAssignedToRemote)
        {
            if (networkAvatarSpawner.spawnedPosition == 1 && GameObject.Find("Remote Avatar"))
            {
                cameraShow2.transform.SetParent(GameObject.Find("Remote Avatar").transform.GetChild(1));
                cameraShow2.localPosition = new Vector3(-0.15f, 0.85f, 0.1f);
                cameraShow2.localEulerAngles = new Vector3(76, 112, 21);

                hasAssignedToRemote = true;
            }

            if (networkAvatarSpawner.spawnedPosition == 2 && GameObject.Find("Remote Avatar"))
            {
                cameraShow1.transform.SetParent(GameObject.Find("Remote Avatar").transform.GetChild(1));
                cameraShow1.localPosition = new Vector3(-0.15f, 0.85f, 0.1f);
                cameraShow1.localEulerAngles = new Vector3(76, 112, 21);

                hasAssignedToRemote = true;
            }
        }
    }
}
