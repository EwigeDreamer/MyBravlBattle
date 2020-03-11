using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    private void Start()
    {
        transform.SetParent(CustomNetworkManager.I.transform);
        transform.localPosition = Vector3.zero;
        var connToClient = connectionToClient;
        var connToServer = connectionToServer;
        var clientAddress = connToClient != null ? $" [client: {connToClient.address}]" : "";
        var serverAddress = connToServer != null ? $" [server: {connToServer.address}]" : "";
        name = $"{typeof(NetworkPlayer)}{clientAddress}{serverAddress}";
    }


    //[SerializeField] NetworkCharacter characterPrefab;

    //public static NetworkPlayer Current { get; private set; } = null;

    //private void Start()
    //{
    //    transform.SetParent(CustomNetworkManager.I.Tr);
    //    transform.localPosition = Vector3.zero;
    //    var connToClient = connectionToClient;
    //    var connToServer = connectionToServer;
    //    var clientAddress = connToClient != null ? $" [{connToClient.address}]" : "";
    //    var serverAddress = connToServer != null ? $" [{connToServer.address}]" : "";
    //    name = $"{typeof(NetworkPlayer)}{clientAddress}{serverAddress}";
    //}

    //public override void OnStartLocalPlayer()
    //{
    //    base.OnStartLocalPlayer();
    //    Debug.Log($"Init local player [{name}]");
    //    Debug.LogError("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
    //    Current = this;
    //}

    //public override void OnNetworkDestroy()
    //{
    //    base.OnNetworkDestroy();
    //    Debug.Log($"Destroy local player [{name}]");
    //    Debug.LogError("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
    //    if (Current == this) Current = null;
    //}

    //[Command]
    //void CmdSpawnCharacter()
    //{
    //    var character = Instantiate(characterPrefab);

    //}

    //[Command]
    //void CmdDespawnCharacter()
    //{

    //}

    //[Command]
    //public void CmdDoInServer()
    //{
    //    Debug.LogError($"SERVER COMMAND! [{name}]");
    //    RpcDoInAllClients();
    //    TargetDoInTargetClients(connectionToClient);
    //}

    //[ClientRpc]
    //void RpcDoInAllClients()
    //{
    //    Debug.LogError($"CLIENT COMMAND! [{name}]");
    //}

    //[TargetRpc]
    //void TargetDoInTargetClients(NetworkConnection target)
    //{
    //    Debug.LogError($"TARGET CLIENT COMMAND! [{name}]");
    //}
}
