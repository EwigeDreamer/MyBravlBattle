using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.GameObjects;

public class NetworkPlayerXRayFinder : NetworkBehaviour
{
    [SerializeField] PlayerViewTrigger playerTrigger;
    List<NetworkPlayerView> playerList = new List<NetworkPlayerView>();

    private void OnValidate()
    {
        gameObject.ValidateGetComponentInChildren(ref this.playerTrigger);
    }

    private void Awake()
    {
        this.playerTrigger.SetActive(false);
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            this.playerTrigger.OnEnter += AddPlayer;
            this.playerTrigger.OnExit += RemovePlayer;
            this.playerTrigger.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        RemoveAllPlayers();
    }

    void RemoveAllPlayers()
    {
        int i = this.playerList.Count;
        while (i-- > 0) RemovePlayer(this.playerList[i]);
    }

    void AddPlayer(NetworkPlayerView player)
    {
        if (this.playerList.Contains(player)) return;
        this.playerList.Add(player);
        player.SetFounded(true);
    }

    void RemovePlayer(NetworkPlayerView player)
    {
        if (player == null) return;
        this.playerList.Remove(player);
        player.SetFounded(false);
    }
}
