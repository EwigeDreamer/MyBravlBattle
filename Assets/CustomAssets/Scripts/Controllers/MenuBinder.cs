using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

namespace SpaceTramp
{
    public class MenuBinder : MonoValidate
    {
        [SerializeField] MainMenuUI main;

        protected override void OnValidate()
        {
            base.OnValidate();
            ValidateFind(ref this.main, true);
        }

        private void Awake()
        {
            this.main.OnPlayPressed += () => CustomNetworkManager.I.StartHost();

            this.main.OnJoinPressed += () => PopupManager.OpenPopup<JoinPopup>();

            this.main.OnQuitPressed += () => PopupManager.OpenPopup<QuittingPopup>();
        }
    }
}