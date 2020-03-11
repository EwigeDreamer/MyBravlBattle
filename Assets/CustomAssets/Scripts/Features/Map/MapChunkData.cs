using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

[CreateAssetMenu(fileName = "MapChunkData", menuName = "MyBravlBattle/MapChunkData")]
public class MapChunkData : ScriptableObject
{
    [SerializeField] Vector2 chunkSize = new Vector2(2f, 2f);
    [SerializeField] MapChunk border;
    [SerializeField] List<MapChunk> chunks;
    [SerializeField] List<TextAsset> csvPresets;

    public Vector2 ChunkSize => chunkSize;
    public MapChunk Border => border;

    ReadOnlyDictionary<int, MapChunk> ro_chunks = null;
    public ReadOnlyDictionary<int, MapChunk> Chunks => ro_chunks ?? (ro_chunks = CreateDict());

    ReadOnlyCollection<TextAsset> ro_csvPresets = null;
    public ReadOnlyCollection<TextAsset> CsvPresets => ro_csvPresets ?? (ro_csvPresets = new ReadOnlyCollection<TextAsset>(csvPresets));

    ReadOnlyDictionary<int, MapChunk> CreateDict()
    {
        var dict = new Dictionary<int, MapChunk>();
        foreach (var chunk in chunks)
            dict[chunk.Id] = chunk;
        return new ReadOnlyDictionary<int, MapChunk>(dict);
    }
}
