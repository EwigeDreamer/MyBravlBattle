using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class MapChunk : MonoValidate
{
    [SerializeField] int id;
    public int Id => id;

    [SerializeField] bool isSpawner;
    public bool IsSpawner => isSpawner;
    public Vector3 ChunkPoint => transform.position;

    public void Init()
    {

    }

}
