using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;

public class PlayerController : MonoSingleton<PlayerController>
{
    [SerializeField] CustomNetworkManager manager;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.manager);
    }

    protected override void Awake()
    {
        base.Awake();
        this.manager.OnOtherCientReady += _ => RefreshPlayers();
    }

    List<NetworkPlayer> allPlayers = new List<NetworkPlayer>();

    NetworkPlayer localPlayer = null;
    public NetworkPlayer LocalPlayer => localPlayer;

    public void Register(NetworkPlayer player) => allPlayers.Add(player);
    public void Unregister(NetworkPlayer player) => allPlayers.Remove(player);

    public void RegisterLocal(NetworkPlayer player)
    {
        if (localPlayer == player) return;
        if (localPlayer != null) UnregisterLocal(localPlayer);
        localPlayer = player;
    }

    public void UnregisterLocal(NetworkPlayer player)
    {
        if (localPlayer == null) return;
        if (localPlayer != player) return;
        localPlayer = null;
    }

    void RefreshPlayers()
    {
        int i = this.allPlayers.Count;
        while (i --> 0)
        {
            if (this.allPlayers[i] == null) { this.allPlayers.RemoveAt(i); continue; }
            this.allPlayers[i].CmdRefresh();
        }
    }
}
