using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public static CustomNetworkManager I => (CustomNetworkManager)singleton;

    public override void OnStartHost()
    {
        SceneLoadingManager.LoadGame();
    }

    public override void OnStopHost()
    {
        SceneLoadingManager.LoadMenu();
    }

    public override void OnStartClient(NetworkClient client)
    {
        SceneLoadingManager.LoadGame();
    }

    public override void OnStopClient()
    {
        SceneLoadingManager.LoadMenu();
    }
}
