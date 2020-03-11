using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;

public class CharacterController : MonoSingleton<CharacterController>
{
    NetworkPlayer localPlayer = null;
    public NetworkPlayer LocalPlayer => localPlayer;

    public void Register(NetworkPlayer player)
    {
        if (localPlayer == player) return;
        if (localPlayer != null) Unregister(localPlayer);
        localPlayer = player;
    }

    public void Unregister(NetworkPlayer player)
    {
        if (localPlayer == null) return;
        if (localPlayer != player) return;
        localPlayer = null;
    }

    public void Move(Vector2 dir)
    {
        if (localPlayer == null) return;
        localPlayer.CmdMove(dir);
    }
}
