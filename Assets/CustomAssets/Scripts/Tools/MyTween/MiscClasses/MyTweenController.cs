using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyTools.Helpers;
using MyTools;

namespace MyTools.Tween.Internal
{
    public delegate void TweenUpdate(float deltaTime, float unscaledDeltaTime);
    public class MyTweenController : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            GameObject go = new GameObject(typeof(MyTweenController).Name);
            DontDestroyOnLoad(go);
            go.AddComponent<MyTweenController>();
        }

        public static event TweenUpdate OnUpdate = delegate { };
        public static event TweenUpdate OnLateUpdate = delegate { };
        public static event TweenUpdate OnFixedUpdate = delegate { };

        void Update() => OnUpdate(TimeManager.DeltaTime, TimeManager.UnscaledDeltaTime);
        void LateUpdate() => OnLateUpdate(TimeManager.DeltaTime, TimeManager.UnscaledDeltaTime);
        void FixedUpdate() => OnFixedUpdate(TimeManager.DeltaTime, TimeManager.UnscaledDeltaTime);
    }
}
