using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Current { get; private set; } = null;

    public override void OnStartClient()
    {
        transform.SetParent(CustomNetworkManager.I.Tr);
        transform.localPosition = Vector3.zero;
        var connToClient = connectionToClient;
        var connToServer = connectionToServer;
        var clientAddress = connToClient != null ? $" [{connToClient.address}]" : "";
        var serverAddress = connToServer != null ? $" [{connToServer.address}]" : "";
        name = $"{typeof(NetworkPlayer)}{clientAddress}{serverAddress}";
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log($"Init local player [{name}]");
        Current = this;
    }

    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
        Debug.Log($"Destroy local player [{name}]");
        if (Current == this) Current = null;
    }

    [Command]
    public void CmdDoInServer()
    {
        Debug.LogError($"SERVER COMMAND! [{name}]");
        RpcDoInAllClients();
        TargetDoInTargetClients(connectionToClient);
    }

    [ClientRpc]
    void RpcDoInAllClients()
    {
        Debug.LogError($"CLIENT COMMAND! [{name}]");
    }

    [TargetRpc]
    void TargetDoInTargetClients(NetworkConnection target)
    {
        Debug.LogError($"TARGET CLIENT COMMAND! [{name}]");
    }
}
