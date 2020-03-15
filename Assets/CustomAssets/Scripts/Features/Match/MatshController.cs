using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;
using UnityEngine.Networking;
using System;
using DG.Tweening;

[System.Serializable]
public class PlayerKillMessage : MessageBase { }

public class MatshController : MonoSingleton<MatshController>
{
    const short msgType = MsgType.Highest + 3;

    public event Action OnKill = delegate { };

    PlayerRefreshMessage killMessage = new PlayerRefreshMessage();

    [SerializeField] PlayerController playerController;
    [SerializeField] float waitTime = 3f;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.playerController);
    }

    private void Awake()
    {
        this.playerController.OnRegister += Subscribe;
        this.playerController.OnUnregister += Unsubscribe;
    }

    public void Subscribe(NetworkPlayer player)
    {
        player.Health.OnDeadByKiller -= CheckKill;
        player.Health.OnDeadByKiller += CheckKill;
    }
    public void Unsubscribe(NetworkPlayer player)
    {
        player.Health.OnDeadByKiller -= CheckKill;
    }

    void CheckKill(GameObject killer, NetworkPlayer killed)
    {
        var player = killer.GetComponent<NetworkPlayer>();
        if (player == null) return;
        SendKillMessage(player.connectionToClient);
        DOVirtual.DelayedCall(this.waitTime, () => CustomNetworkManager.I.Respawn(killed));
    }

    public void SendKillMessage(NetworkConnection conn)
    {
        if (!NetworkServer.active) return;
        NetworkServer.SendToClient(conn.connectionId, msgType, this.killMessage);
    }

    public void ReceiveKillMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<PlayerRefreshMessage>();
        if (msg == null) return;
        OnKill();
    }
}
