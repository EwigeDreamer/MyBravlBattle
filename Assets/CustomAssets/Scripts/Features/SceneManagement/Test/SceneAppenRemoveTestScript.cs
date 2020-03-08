using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.SceneManagement;
using MyTools.Singleton;
using UnityEngine.SceneManagement;

public class SceneAppenRemoveTestScript : MonoSingleton<SceneAppenRemoveTestScript>
{
#pragma warning disable 649
    [SerializeField] [SceneIndex] int scene0index;
    [SerializeField] [SceneIndex] int scene1index;
    [SerializeField] [SceneIndex] int scene2index;
    [SerializeField] [SceneIndex] int scene3index;
    [SerializeField] [SceneIndex] int sceneCounterindex;
    KeyCode scene0key = KeyCode.Keypad0;
    KeyCode scene1key = KeyCode.Keypad1;
    KeyCode scene2key = KeyCode.Keypad2;
    KeyCode scene3key = KeyCode.Keypad3;
    KeyCode sceneCounterkey = KeyCode.Keypad4;
    bool scene0loaded = false;
    bool scene1loaded = false;
    bool scene2loaded = false;
    bool scene3loaded = false;
#pragma warning restore 649

    protected override void Awake()
    {
        base.Awake();
        MakeDontDestroy();
    }

    private void Update()
    {
        if (Input.GetKeyDown(scene0key))
        {
            if (scene0loaded)
            {
                if (SceneLoader.RemoveScene(scene0index, onUnloaded: scene => { Debug.LogWarning("Scene 0 was removed"); })) scene0loaded = false;
            }
            else
            {
                if (SceneLoader.AppendScene(scene0index, onLoaded: scene => { Debug.Log("Scene 0 was loaded"); })) scene0loaded = true;
                //SceneManager.LoadSceneAsync(scene0index, LoadSceneMode.Additive);
            }
        }
        if (Input.GetKeyDown(scene1key))
        {
            if (scene1loaded)
            {
                if (SceneLoader.RemoveScene(scene1index, onUnloaded: scene => { Debug.LogWarning("Scene 1 was removed"); })) scene1loaded = false;
            }
            else
            {
                if (SceneLoader.AppendScene(scene1index, onLoaded: scene => { Debug.Log("Scene 1 was loaded"); })) scene1loaded = true;
                //SceneManager.LoadSceneAsync(scene1index, LoadSceneMode.Additive);
            }
        }
        if (Input.GetKeyDown(scene2key))
        {
            if (scene2loaded)
            {
                if (SceneLoader.RemoveScene(scene2index, onUnloaded: scene => { Debug.LogWarning("Scene 2 was removed"); })) scene2loaded = false;
            }
            else
            {
                if (SceneLoader.AppendScene(scene2index, onLoaded: scene => { Debug.Log("Scene 2 was loaded"); })) scene2loaded = true;
                //SceneManager.LoadSceneAsync(scene2index, LoadSceneMode.Additive);
            }
        }
        if (Input.GetKeyDown(scene3key))
        {
            if (scene3loaded)
            {
                if (SceneLoader.RemoveScene(scene3index, onUnloaded: scene => { Debug.LogWarning("Scene 3 was removed"); })) scene3loaded = false;
            }
            else
            {
                if (SceneLoader.AppendScene(scene3index, onLoaded: scene => { Debug.Log("Scene 3 was loaded"); })) scene3loaded = true;
                //SceneManager.LoadSceneAsync(scene3index, LoadSceneMode.Additive);
            }
        }
        if (Input.GetKeyDown(sceneCounterkey))
        {
            SceneLoader.LoadScene(sceneCounterindex, onLoaded: scene => { Debug.Log("Shit was loaded"); });
            //SceneManager.LoadSceneAsync(sceneCounterindex, LoadSceneMode.Single);
            scene0loaded = false;
            scene1loaded = false;
            scene2loaded = false;
            scene3loaded = false;
        }
    }

    
}
