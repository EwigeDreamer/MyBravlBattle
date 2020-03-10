using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.ObjectModel;
using System;

public class CustomNetworkManager : NetworkManager
{
    public event Action OnHostStarted = delegate { };
    public event Action OnHostStopped = delegate { };
    public event Action OnClientStarted = delegate { };
    public event Action OnClientStopped = delegate { };
    public static CustomNetworkManager I => (CustomNetworkManager)singleton;

    public ReadOnlyCollection<string> IpAddresses { get; private set; } = null;

    public bool IsServer => NetworkServer.active;
    public Transform Tr => transform;

    public override void OnStartHost()
    {
        InitIps();
        Debug.Log($"Start host!");
        OnHostStarted();
        SceneLoadingManager.LoadGame();
    }

    public override void OnStopHost()
    {
        Debug.Log($"Stop host!");
        OnHostStopped();
        SceneLoadingManager.LoadMenu();
    }

    public override void OnStartClient(NetworkClient client)
    {
        Debug.Log($"Start client!");
        OnClientStarted();
        SceneLoadingManager.LoadGame();
    }

    public override void OnStopClient()
    {
        Debug.Log($"Stop client!");
        OnClientStopped();
        SceneLoadingManager.LoadMenu();
    }

    void InitIps()
    {
        List<string> ips = new List<string>();
        IPManager.GetAllIPs(ips, IPManager.ADDRESSFAM.IPv4, false);
        IpAddresses = new ReadOnlyCollection<string>(ips);
    }
}

