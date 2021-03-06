﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;
using UnityEngine.Networking;
using System;

[System.Serializable]
public class PlayerRefreshMessage : MessageBase { }

public class PlayerController : MonoSingleton<PlayerController>
{
    const short msgType = MsgType.Highest + 2;

    public event Action<NetworkPlayer> OnRegister = delegate { };
    public event Action<NetworkPlayer> OnUnregister = delegate { };
    public event Action<WeaponKind> OnChangeWeapon = delegate { };

    [SerializeField] CustomNetworkManager manager;
    PlayerRefreshMessage refreshMessage = new PlayerRefreshMessage();
    List<NetworkPlayer> allPlayers = new List<NetworkPlayer>();
    NetworkPlayer localPlayer = null;

    public NetworkPlayer LocalPlayer => localPlayer;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.manager);
    }

    protected override void Awake()
    {
        base.Awake();

        this.manager.OnClientStarted += client => client.RegisterHandler(msgType, ReceiveRefreshMessage);
        this.manager.OnOtherCientReady += conn => SendRefreshMessage(conn, this.refreshMessage);
    }

    public void Register(NetworkPlayer player)
    {
        allPlayers.Add(player);
        OnRegister(player);
    }
    public void Unregister(NetworkPlayer player)
    {
        allPlayers.Remove(player);
        OnUnregister(player);
    }

    public void RegisterLocal(NetworkPlayer player)
    {
        if (this.localPlayer == player) return;
        if (this.localPlayer != null) UnregisterLocal(this.localPlayer);
        this.localPlayer = player;
        player.Combat.OnSetWeapon -= OnWeapoChangeEvent;
        player.Combat.OnSetWeapon += OnWeapoChangeEvent;
    }

    public void UnregisterLocal(NetworkPlayer player)
    {
        if (this.localPlayer == null) return;
        if (this.localPlayer != player) return;
        this.localPlayer = null;
        player.Combat.OnSetWeapon -= OnWeapoChangeEvent;
    }

    void OnWeapoChangeEvent(WeaponKind kind) => OnChangeWeapon(kind);

    public NetworkPlayer GetClosest(NetworkPlayer player)
    {
        if (this.allPlayers.Count < 1) return null;
        var min = float.PositiveInfinity;
        NetworkPlayer closest = null;
        foreach (var pl in this.allPlayers)
        {
            if (pl == player) continue;
            if (!pl.View.IsVisible) continue;
            var dist = (pl.Motor.Position - player.Motor.Position).sqrMagnitude;
            if (dist > min) continue;
            min = dist;
            closest = pl;
        }
        return closest;
    }

    public void SendRefreshMessage(NetworkConnection conn, PlayerRefreshMessage msg)
    {
        if (!NetworkServer.active) return;
        NetworkServer.SendToClient(conn.connectionId, msgType, msg);
    }

    public void ReceiveRefreshMessage(NetworkMessage netMsg)
    {
        if (this.localPlayer == null) return;
        var msg = netMsg.ReadMessage<PlayerRefreshMessage>();
        if (msg == null) return;
        this.localPlayer.CmdRefresh();
    }
}
