using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MyTools.Extensions.Vectors;

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
    List<MapChunk> schunks = new List<MapChunk>();
    List<MapChunk> spawners = new List<MapChunk>();

    public bool IsMapBuilded => schunks.Count > 0;

    public event System.Action OnMapBuilded = delegate { };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
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
#if UNITY_EDITOR
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < preset.rows; ++i)
        {
            for (int j = 0; j < preset.columns; ++j)
                sb.Append($" {preset[i, j]}");
            sb.AppendLine();
        }
        Debug.LogWarning($"BUILD MAP!!!\n{sb}");
#endif

        var chunkData = MapController.I.ChunkData;
        var chunkSize = chunkData.ChunkSize;
        var mapOffset = new Vector2((preset.rows - 1) * chunkSize.x / -2f, (preset.columns - 1) * chunkSize.y / -2f).ToV3_x0y();
        for (int i = 0; i < preset.rows; ++i)
            for (int j = 0; j < preset.columns; ++j)
            {
                var pos = new Vector2(j * chunkSize.x, (preset.rows - i) * chunkSize.y).ToV3_x0y() + mapOffset;
                if (chunkData.Chunks.TryGetValue(preset[i, j], out var chunkPrefab))
                {
                    var chunk = Instantiate(chunkData.Chunks[preset[i, j]], pos, Quaternion.identity, transform);
                    schunks.Add(chunk);
                    chunk.Init();
                }
            }

        foreach (var chunk in schunks)
            if (chunk.IsSpawner) spawners.Add(chunk);

        OnMapBuilded();
    }
    public Vector3 GetRandomSpawnPoint()
    {
        if (spawners.Count < 1) return Vector3.up * 2f;
        return spawners[Random.Range(0, spawners.Count)].ChunkPoint;
    }
}

