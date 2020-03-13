﻿using System.Collections;
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

        Debug.LogError($"AAAAAAAAAAAAAAAAAAAAAAAAAAA is localp layer: {isLocalPlayer}", gameObject);
        if (isLocalPlayer)
        {
            CmdTeleportToSpawnPoint();
            PlayerController.I.Register(this);
        }
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
}
