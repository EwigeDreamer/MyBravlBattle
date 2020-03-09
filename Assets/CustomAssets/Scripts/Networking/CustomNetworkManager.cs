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

    public static CustomNetworkManager I => (CustomNetworkManager)singleton;

    public override void OnStartHost()
    {
        InitIps();
        Debug.LogWarning($"Start host!");
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

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.LogWarning($"Connect player! (method 1) [{playerControllerId}] [{conn.address}]");
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform);
        player.name = $"{typeof(NetworkPlayer).Name} [{playerControllerId}] [{conn.address}]";
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        Debug.LogWarning($"Connect player! (method 2) [{playerControllerId}] [{conn.address}]");
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform);
        player.name = $"{typeof(NetworkPlayer).Name} [{playerControllerId}] [{conn.address}]";
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.LogWarning($"Server Connect! [{conn.address}]");
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.LogWarning($"Client Connect! [{conn.address}]");
    }

    void InitIps()
    {
        List<string> ips = new List<string>();
        IPManager.GetAllIPs(ips, IPManager.ADDRESSFAM.IPv4, false);
        IpAddresses = new ReadOnlyCollection<string>(ips);
    }
}
