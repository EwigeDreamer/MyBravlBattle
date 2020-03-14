using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.GameObjects;

public class NetworkPlayerGrassHider : NetworkBehaviour
{
    [SerializeField] GrassTransparencyTrigger grassTrigger;
    List<GrassField> grassList = new List<GrassField>();

    private void OnValidate()
    {
        gameObject.ValidateGetComponentInChildren(ref this.grassTrigger);
    }

    private void Awake()
    {
        this.grassTrigger.OnEnter += AddGrass;
        this.grassTrigger.OnExit += RemoveGrass;
        SetActive(false);
    }

    public void SetActive(bool state) => this.grassTrigger.SetActive(state);

    private void OnDestroy()
    {
        RemoveAllGrass();
    }

    void RemoveAllGrass()
    {
        int i = this.grassList.Count;
        while (i --> 0) RemoveGrass(this.grassList[i]);
    }

    void AddGrass(GrassField grass)
    {
        if (this.grassList.Contains(grass)) return;
        this.grassList.Add(grass);
        grass.SetVisibility(false);
    }

    void RemoveGrass(GrassField grass)
    {
        if (grass == null) return;
        this.grassList.Remove(grass);
        grass.SetVisibility(true);
    }
}
