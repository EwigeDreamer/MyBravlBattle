using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using MyTools.Extensions.GameObjects;
using DG.Tweening;

public class MainInitializator : MonoValidate
{
    [SerializeField] List<GameObject> forDontDestroy = new List<GameObject>();
    [SerializeField] List<GameObject> forDestroy = new List<GameObject>();
    private void Awake()
    {
        InitDOTween();
    }
    private void Start()
    {
        DontDestroyItems();
        DestroyItems();
        LoadGame();
        Destroy(gameObject);
    }

    void InitDOTween()
    {
        DOTween.Init(true, false, LogBehaviour.Verbose).SetCapacity(200, 25);
    }

    void DontDestroyItems()
    {
        foreach (var item in this.forDontDestroy)
        {
            item.ClearParent();
            DontDestroyOnLoad(item);
        }
    }
    void DestroyItems()
    {
        foreach (var item in this.forDestroy)
            Destroy(item);
    }
    void LoadGame()
    {
        SceneLoadingManager.FirstLoad();
    }
}
