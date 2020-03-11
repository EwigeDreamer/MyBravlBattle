using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.ObjectModel;
using System;
using MyTools.Helpers;

public class CustomNetworkManager : NetworkManager
{
    public event Action OnServerStarted = delegate { };
    public event Action OnReadyServer = delegate { };
    public event Action OnReadyHost = delegate { };
    public event Action OnServerStopped = delegate { };
    public event Action OnClientStarted = delegate { };
    public event Action OnClientStopped = delegate { };
    public static CustomNetworkManager I => (CustomNetworkManager)singleton;

    public ReadOnlyCollection<string> IpAddresses { get; private set; } = null;

    public bool IsServer => NetworkServer.active;
    public Transform Tr => transform;

    private void Start()
    {
        InitIps();
    }

    public override void OnStartServer()
    {
        Debug.Log($"Start host!");
        OnServerStarted();
        SceneLoadingManager.LoadGame();
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        if (conn.connectionId == 0)
        {
            Debug.Log($"Host ready!");
            OnReadyHost();
        }
        Debug.Log($"Server ready for new player!");
        OnReadyServer();
    }

    public override void OnStopServer()
    {
        Debug.Log($"Stop host!");
        OnServerStopped();
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

