using System;
using UnityEngine;
using Oculus.Avatar2;
using Oculus.Platform;
using Oculus.Platform.Models;
using Fusion;

public class UserEntitlement : MonoBehaviour
{
    public static ulong OculusID;

    [HideInInspector] public string displayName;
    

    public Action OnEntitlementGranted;

    private void Awake() => EntitlementCheck();

    // initialises the Oculus Platform asynchronously and checks if the user is entitled to use the application. If the platform fails to initalise, it logs the exception.

    private void EntitlementCheck()
    {
        try
        {
            Core.AsyncInitialize();
            Entitlements.IsUserEntitledToApplication().OnComplete(IsUserEntitledToApplicationComplete);
        }
        catch (UnityException e)
        {
            Debug.LogError("Platform failed to initialise due to exception.");
            Debug.LogException(e);
        }
    }

    private void IsUserEntitledToApplicationComplete(Message message)
    {
        if (message.IsError)
        {
            Debug.LogError(message.GetError());
            return;
        }

        Debug.Log("You are entitled to use this app.");

        Users.GetAccessToken().OnComplete(GetAccessTokenComplete);
    }

    private void GetAccessTokenComplete(Message<string> message)
    {
        if (message.IsError)
        {
            Debug.LogError(message.GetError());
            return;
        }

        OvrAvatarEntitlement.SetAccessToken(message.Data);

        Users.GetLoggedInUser().OnComplete(GetLoggedInUserComplete);

        Users.GetLoggedInUser().OnComplete(message1 =>
        {
            Users.Get(message1.Data.ID).OnComplete(message2 =>
            {
                displayName = message2.Data.DisplayName;
            });
        });
    }

    private void GetLoggedInUserComplete(Message<User> message)
    {
        if (message.IsError)
        {
            Debug.LogError(message.GetError());
            return;
        }

        OculusID = message.Data.ID;
        OnEntitlementGranted?.Invoke();
    }
}