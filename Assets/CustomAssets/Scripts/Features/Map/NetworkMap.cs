using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class MapPreset
{
    public int[] ids;
    public int rows;
    public int columns;

    public MapPreset() { }
    public MapPreset(int rows, int columns)
    {
        ids = new int[rows * columns];
        this.rows = rows;
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
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);    
    }

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
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < preset.rows; ++i)
        {
            for (int j = 0; j < preset.columns; ++j)
                sb.Append($" {preset[i,j]}");
            sb.AppendLine();
        }

        Debug.LogError($"BUILD MAP!!!\n{sb}");
        //TODO генерация карты
    }

}

