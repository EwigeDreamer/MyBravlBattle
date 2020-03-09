using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.ObjectModel;

public class CustomNetworkManager : NetworkManager
{
    public bool IsServer => NetworkServer.active;
    //public bool IsClient => IsClientConnected();
    public ReadOnlyCollection<string> IpAddresses { get; private set; } = null;

    public Transform Tr => transform;

    public static CustomNetworkManager I => (CustomNetworkManager)singleton;

    public override void OnStartHost()
    {
        InitIps();
        Debug.Log($"Start host!");
        SceneLoadingManager.LoadGame();
    }

    public override void OnStopHost()
    {
        Debug.Log($"Stop host!");
        SceneLoadingManager.LoadMenu();
    }

    public override void OnStartClient(NetworkClient client)
    {
        Debug.Log($"Start client!");
        SceneLoadingManager.LoadGame();
    }

    public override void OnStopClient()
    {
        Debug.Log($"Stop client!");
        SceneLoadingManager.LoadMenu();
    }

    void InitIps()
    {
        List<string> ips = new List<string>();
        IPManager.GetAllIPs(ips, IPManager.ADDRESSFAM.IPv4, false);
        IpAddresses = new ReadOnlyCollection<string>(ips);
    }
}
