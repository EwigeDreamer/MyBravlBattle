using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.GameObjects;

public class NetworkPlayerGrassVisibility : NetworkBehaviour
{
    [SerializeField] PlayerVisibilityTrigger grassTrigger;
    [SerializeField] NetworkPlayerView view;
    List<PlayerVisibilitySensor> grassList = new List<PlayerVisibilitySensor>();

    private void OnValidate()
    {
        gameObject.ValidateGetComponentInChildren(ref this.grassTrigger);
        gameObject.ValidateGetComponent(ref this.view);
    }

    private void Awake()
    {
        this.grassTrigger.SetActive(false);
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            this.grassTrigger.OnEnter += AddGrass;
            this.grassTrigger.OnExit += RemoveGrass;
            this.grassTrigger.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        RemoveAllGrass();
    }

    void RemoveAllGrass()
    {
        int i = this.grassList.Count;
        while (i-- > 0) RemoveGrass(this.grassList[i]);
    }

    void AddGrass(PlayerVisibilitySensor player)
    {
        if (this.grassList.Contains(player)) return;
        this.grassList.Add(player);
        this.view.SetVisible(this.grassList.Count < 1);
    }

    void RemoveGrass(PlayerVisibilitySensor player)
    {
        if (player == null) return;
        this.grassList.Remove(player);
        this.view.SetVisible(this.grassList.Count < 1);
    }
}
