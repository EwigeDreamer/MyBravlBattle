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
        public event Action OnExitPressed = delegate { };
        public event Action OnCreditsPressed = delegate { };
        public event Action OnRatePressed = delegate { };
        public event Action OnSharePressed = delegate { };
        public event Action OnLangPressed = delegate { };
        public event Action OnSettingsPressed = delegate { };

        [SerializeField] Button m_PlayBtn;
        [SerializeField] Button m_ExitBtn;
        [SerializeField] Button m_CreditsBtn;
        [SerializeField] Button m_RateBtn;
        [SerializeField] Button m_ShareBtn;
        [SerializeField] Button m_LangBtn;
        [SerializeField] Button m_SettingsBtn;

        void Awake()
        {
            m_PlayBtn.onClick.AddListener(() => OnPlayPressed());
            m_ExitBtn.onClick.AddListener(() => OnExitPressed());
            m_CreditsBtn.onClick.AddListener(() => OnCreditsPressed());
            m_RateBtn.onClick.AddListener(() => OnRatePressed());
            m_ShareBtn.onClick.AddListener(() => OnSharePressed());
            m_LangBtn.onClick.AddListener(() => OnLangPressed());
            m_SettingsBtn.onClick.AddListener(() => OnSettingsPressed());
        }
    }
}