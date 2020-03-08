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

        [SerializeField] Button m_PlayBtn;
        [SerializeField] Button m_JoinBtn;
        [SerializeField] Button m_QuitBtn;

        void Awake()
        {
            m_PlayBtn.onClick.AddListener(() => OnPlayPressed());
            m_JoinBtn.onClick.AddListener(() => OnJoinPressed());
            m_QuitBtn.onClick.AddListener(() => OnQuitPressed());
        }
    }
}