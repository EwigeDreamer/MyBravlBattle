using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;
using UnityEngine.Networking;
using LW = LumenWorks.Framework.IO.Csv;
using MyTools.Extensions.Vectors;


[System.Serializable]
public class MapPreset : MessageBase
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

public class MapController : MonoSingleton<MapController>
{
    public event System.Action OnMapBuilded = delegate { };

    [SerializeField] CustomNetworkManager manager;
    [SerializeField] MapChunkData chunkData;

    [SerializeField] LayerMask playerMask;

    List<MapPreset> presets = new List<MapPreset>();

    List<MapChunk> mapCchunks = new List<MapChunk>();
    List<MapChunk> mapSpawners = new List<MapChunk>();

    public MapChunkData ChunkData => chunkData;
    public bool IsMapBuilded => mapCchunks.Count > 0;


    int presetIndex = -1;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.manager);
    }

    protected override void Awake()
    {
        base.Awake();

        manager.OnServerStarted += BuildMap;
        manager.OnClientStopped += DestroyMap;

        manager.OnClientStarted += client => client.RegisterHandler(MsgType.Highest + 1, ReceiveMapPreset);
        manager.OnOtherCientReady += conn => SendMapPreset(conn, this.presets[this.presetIndex]);

        ReadPresets();
    }


    void BuildMap()
    {
        if (IsMapBuilded) return;
        this.presetIndex = Random.Range(0, this.presets.Count);
        BuildMap(this.presets[this.presetIndex]);
    }
    void DestroyMap()
    {
        foreach (var chunck in this.mapCchunks) Destroy(chunck.GO);
        this.mapCchunks.Clear();
        this.mapSpawners.Clear();
    }
    void RestoreMap(MapPreset preset)
    {
        BuildMap(preset);
    }

    void BuildMap(MapPreset preset)
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
        DestroyMap();
        var chunks = this.chunkData.Chunks;
        var chunkSize = this.chunkData.ChunkSize;
        var mapOffset = new Vector2((preset.rows - 1) * chunkSize.y / -2f, (preset.columns - 1) * chunkSize.x / -2f).ToV3_x0y();
        for (int i = 0; i < preset.rows; ++i)
            for (int j = 0; j < preset.columns; ++j)
            {
                var pos = new Vector2(j * chunkSize.x, (preset.rows - i) * chunkSize.y).ToV3_x0y() + mapOffset;
                if (chunks.TryGetValue(preset[i, j], out var chunkPrefab))
                {
                    var chunk = Instantiate(chunks[preset[i, j]], pos, Quaternion.identity, transform);
                    this.mapCchunks.Add(chunk);
                    chunk.Init();
                }
            }

        foreach (var chunk in this.mapCchunks)
            if (chunk.IsSpawner) this.mapSpawners.Add(chunk);

        OnMapBuilded();
    }


    public void SendMapPreset(NetworkConnection conn, MapPreset preset)
    {
        if (!NetworkServer.active) return;
        NetworkServer.SendToClient(conn.connectionId, MsgType.Highest + 1, preset);
    }

    public void ReceiveMapPreset(NetworkMessage netMsg)
    {
        var preset = netMsg.ReadMessage<MapPreset>();
        if (preset == null) return;
        RestoreMap(preset);
    }
    
    void ReadPresets()
    {
        var csv_presets = chunkData.CsvPresets;
        List<int[]> rows = new List<int[]>();
        foreach (var csv_preset in csv_presets)
        {
            var strReader = new System.IO.StringReader(csv_preset.text);
            using (var csv = new LW.CsvReader(strReader, false))
            {
                rows.Clear();
                int columnCount = csv.FieldCount;
                while (csv.ReadNextRecord())
                {
                    var row = new int[columnCount];
                    for (int i = 0; i < row.Length; ++i)
                        row[i] = int.Parse(csv[i]);
                    rows.Add(row);
                }
                int rowCount = rows.Count;
                var preset = new MapPreset(rowCount, columnCount);
                for (int i = 0; i < rowCount; ++i)
                    for (int j = 0; j < columnCount; ++j)
                        preset[i, j] = rows[i][j];
                this.presets.Add(preset);
            }
        }
    }

    List<MapChunk> freeSpawnersTmp = new List<MapChunk>();
    Collider[] intersectsTmp = new Collider[100];

    public Vector3 GetRandomSpawnPoint()
    {
        freeSpawnersTmp.Clear();
        foreach (var spawner in mapSpawners)
        {
            if (Physics.OverlapSphereNonAlloc(spawner.ChunkPoint + Vector3.up, 0.5f, intersectsTmp, playerMask) < 1)
                freeSpawnersTmp.Add(spawner);
        }
        if (freeSpawnersTmp.Count < 1)
            return mapSpawners[Random.Range(0, mapSpawners.Count)].ChunkPoint;
        return freeSpawnersTmp[Random.Range(0, freeSpawnersTmp.Count)].ChunkPoint;

    }
}
