using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Helpers;
using MyTools.Extensions.Vectors;
using MyTools.Extensions.GameObjects;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] NetworkPlayerView view;
    [SerializeField] NetworkPlayerMotor motor;
    [SerializeField] NetworkPlayerCombat combat;

    public NetworkPlayerView View => this.view;
    public NetworkPlayerMotor Motor => this.motor;
    public NetworkPlayerCombat Combat => this.combat;

    private void OnValidate()
    {
        gameObject.ValidateGetComponent(ref this.view);
        gameObject.ValidateGetComponent(ref this.motor);
        gameObject.ValidateGetComponent(ref this.combat);
    }

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
        CmdTeleportToSpawnPoint();
        PlayerController.I.Register(this);
    }

    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
        PlayerController.I.Unregister(this);
    }

    [Command]
    void CmdTeleportToSpawnPoint()
    {
        Debug.LogWarning($"TELEPORT 000! {name}");
        var point = MapController.I.GetRandomSpawnPoint();
        this.motor.CmdTeleport(point);
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
