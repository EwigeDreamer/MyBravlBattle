using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;
using UnityEngine.Networking;
using LW = LumenWorks.Framework.IO.Csv;

public class MapController : MonoSingleton<MapController>
{
    public event System.Action OnMapBuilded = delegate { };

    [SerializeField] CustomNetworkManager manager;
    [SerializeField] MapChunkData chunkData;
    [SerializeField] NetworkMap mapPrefab;

    public MapChunkData ChunkData => chunkData;

    NetworkMap map = null;
    List<MapPreset> presets = null;

    int presetIndex = -1;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref manager);
    }

    protected override void Awake()
    {
        base.Awake();
        manager.OnReadyHost += CreateMap;
        manager.OnServerStopped += DestroyMap;
        manager.OnReadyServer += RefreshMapOnNewClients;
        ReadPresets();
    }

    void CreateMap()
    {
        Debug.LogWarning("SPAWN MAP!");
        var map = Instantiate(mapPrefab);
        NetworkServer.Spawn(map.gameObject);
        presetIndex = Random.Range(0, presets.Count);
        map.RpcBuild(presets[presetIndex]);
    }

    void RefreshMapOnNewClients()
    {
        Debug.LogWarning("TRY TO REFRESH MAP!");
        if (this.map == null) return;
        if (presetIndex < 0) return;
        Debug.LogWarning("REFRESH MAP!");
        this.map.RpcBuild(presets[presetIndex]);
    }

    void DestroyMap()
    {
        Debug.LogWarning("DESTROY MAP!");
        if (this.map == null) return;
        NetworkServer.Destroy(this.map.gameObject);
    }

    public void Register(NetworkMap map)
    {
        if (this.map == map) return;
        if (this.map != null) DestroyMap();
        this.map = map;
        if (map.IsMapBuilded) OnMapBuilded();
        else map.OnMapBuilded += () => OnMapBuilded();
    }

    public void Unregister(NetworkMap map)
    {
        if (this.map != map) return;
        this.map = null;
    }

    void ReadPresets()
    {
        var csv_presets = chunkData.CsvPresets;
        this.presets = new List<MapPreset>(csv_presets.Count);
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

}
