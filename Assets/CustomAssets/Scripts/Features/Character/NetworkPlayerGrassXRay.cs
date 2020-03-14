using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.GameObjects;

public class NetworkPlayerGrassXRay : NetworkBehaviour
{
    [SerializeField] GrassTransparencyTrigger grassTrigger;
    List<GrassTransparencySensor> grassList = new List<GrassTransparencySensor>();

    private void OnValidate()
    {
        gameObject.ValidateGetComponentInChildren(ref this.grassTrigger);
    }

    private void Awake()
    {
        SetActive(false);
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            this.grassTrigger.OnEnter += AddGrass;
            this.grassTrigger.OnExit += RemoveGrass;
        }
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

    void AddGrass(GrassTransparencySensor grass)
    {
        if (this.grassList.Contains(grass)) return;
        this.grassList.Add(grass);
        grass.SetVisibility(false);
    }

    void RemoveGrass(GrassTransparencySensor grass)
    {
        if (grass == null) return;
        this.grassList.Remove(grass);
        grass.SetVisibility(true);
    }
}
