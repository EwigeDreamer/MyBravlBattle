using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class MapPreset
{
    public int[] ids;
    public int columns;

    public MapPreset() { }
    public MapPreset(int rows, int columns)
    {
        ids = new int[rows * columns];
        this.columns = columns;
    }

    public int this[int i, int j]
    {
        get => ids[i * columns + j];
        set => ids[i * columns + j] = value;
    }
}

public class NetworkMap : NetworkBehaviour
{
    private void Start()
    {
        MapController.I.Register(this);
    }

    private void OnDestroy()
    {
        MapController.I.Unregister(this);
    }

    [ClientRpc] 
    public void RpcBuild(MapPreset preset)
    {
        Debug.LogError("BUILD MAP!!!");
        //TODO генерация карты
    }

}

