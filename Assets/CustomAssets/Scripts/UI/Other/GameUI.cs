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
    public class GameUI : UIBase
    {
        public event Action OnMenuPressed = delegate { };

        [SerializeField] Button menuBtn;

        void Awake()
        {
            menuBtn.onClick.AddListener(() => PopupManager.OpenPopup<GameMenuPopup>());
        }
    }
}