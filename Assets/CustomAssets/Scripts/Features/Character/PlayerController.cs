using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;

public class PlayerController : MonoSingleton<PlayerController>
{
    NetworkPlayer localPlayer = null;
    public NetworkPlayer LocalPlayer => localPlayer;

    public void Register(NetworkPlayer player)
    {
        Debug.LogError("REGISTER 000", player.gameObject);
        if (localPlayer == player) return;
        if (localPlayer != null) Unregister(localPlayer);
        Debug.LogError("REGISTER 111", player.gameObject);
        localPlayer = player;
    }

    public void Unregister(NetworkPlayer player)
    {
        Debug.LogError("UNREGISTER", player.gameObject);
        if (localPlayer == null) return;
        if (localPlayer != player) return;
        Debug.LogError("UNREGISTER 111", player.gameObject);
        localPlayer = null;
    }
}
