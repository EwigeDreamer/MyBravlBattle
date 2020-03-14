using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerRefreshMessage : MessageBase { }

public class PlayerController : MonoSingleton<PlayerController>
{
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

        this.manager.OnClientStarted += client => client.RegisterHandler(MsgType.Highest + 2, ReceiveRefreshMessage);
        this.manager.OnOtherCientReady += conn => SendRefreshMessage(conn, this.refreshMessage);
    }

    public void Register(NetworkPlayer player) => allPlayers.Add(player);
    public void Unregister(NetworkPlayer player) => allPlayers.Remove(player);

    public void RegisterLocal(NetworkPlayer player)
    {
        if (this.localPlayer == player) return;
        if (this.localPlayer != null) UnregisterLocal(this.localPlayer);
        this.localPlayer = player;
    }

    public void UnregisterLocal(NetworkPlayer player)
    {
        if (this.localPlayer == null) return;
        if (this.localPlayer != player) return;
        this.localPlayer = null;
    }

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
        NetworkServer.SendToClient(conn.connectionId, MsgType.Highest + 1, msg);
    }

    public void ReceiveRefreshMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<PlayerRefreshMessage>();
        if (msg == null) return;
        this.localPlayer?.CmdRefresh();
    }
}
