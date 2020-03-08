using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Menu;
using MyTools.Tween;
using MyTools.Extensions.Common;
using UnityEngine.UI;
using System;
using MyTools.Helpers;

namespace SpaceTramp
{
    public class MainMenuUI : UIBase
    {
        public event Action OnPlayPressed = delegate { };
        public event Action OnJoinPressed = delegate { };
        public event Action OnQuitPressed = delegate { };

        [SerializeField] Button playBtn;
        [SerializeField] Button joinBtn;
        [SerializeField] Button quitBtn;

        void Awake()
        {
            playBtn.onClick.AddListener(() => OnPlayPressed());
            joinBtn.onClick.AddListener(() => OnJoinPressed());
            quitBtn.onClick.AddListener(() => OnQuitPressed());
        }
    }
}