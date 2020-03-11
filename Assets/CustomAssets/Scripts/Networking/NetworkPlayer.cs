using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Helpers;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] NetworkCharacter characterPrefab;

    NetworkCharacter character = null;

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

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        CmdSpawnCharacterView();
    }

    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
        CmdDestroyCharacterView();
    }

    [Command]
    void CmdSpawnCharacterView()
    {
        CorouWaiter.WaitFor(() => MapController.I.IsMapBuilded, Spawn, () => this == null);
        void Spawn()
        {
            var point = MapController.I.GetRandomSpawnPoint();
            character = Instantiate(characterPrefab, point, Quaternion.identity, CharacterController.I.TR);
            NetworkServer.Spawn(character.gameObject);
        }
    }

    [Command]
    void CmdDestroyCharacterView()
    {
        if (character == null) return;
        NetworkServer.Destroy(character.gameObject);
        character = null;
    }

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
