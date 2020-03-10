using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

[CreateAssetMenu(fileName = "MapChunkData", menuName = "MyBravlBattle/MapChunkData")]
public class MapChunkData : ScriptableObject
{
    [SerializeField] List<MapChunk> chunks;
    [SerializeField] List<TextAsset> csvPresets;

    ReadOnlyCollection<MapChunk> ro_chunks = null;
    public ReadOnlyCollection<MapChunk> Chunks => ro_chunks ?? (ro_chunks = new ReadOnlyCollection<MapChunk>(chunks));


    ReadOnlyCollection<TextAsset> ro_csvPresets = null;
    public ReadOnlyCollection<TextAsset> CsvPresets => ro_csvPresets ?? (ro_csvPresets = new ReadOnlyCollection<TextAsset>(csvPresets));
}
