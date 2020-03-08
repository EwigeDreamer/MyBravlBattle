using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public bool IsServer => NetworkServer.active;
    //public bool IsClient => IsClientConnected();
    public string IpAddress { get; private set; } = null;

    public static CustomNetworkManager I => (CustomNetworkManager)singleton;

    public override void OnStartHost()
    {
        IpAddress = IPManager.GetIP(IPManager.ADDRESSFAM.IPv4);
        Debug.LogWarning($"Start host! [{IpAddress}]");
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
        Debug.LogWarning($"Connect player! [{conn.address}]");
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform);
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
}
