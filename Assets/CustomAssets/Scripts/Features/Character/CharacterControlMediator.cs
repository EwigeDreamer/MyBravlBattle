using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using MyTools.Extensions.Vectors;

public class CharacterControlMediator : MonoValidate
{
    [SerializeField] UserControlScript userControl;
    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref userControl);
    }

    private void Awake()
    {
        userControl.OnMove += dir => PlayerController.I.LocalPlayer.Motor.Move(dir);
        userControl.OnDirectionalShoot += dir =>
        {
            Debug.LogWarning($"DirectionalShoot {dir}");
            PlayerController.I.LocalPlayer.Combat.CmdShoot(dir);
            PlayerController.I.LocalPlayer.View.CmdSetAim(false, false);
            PlayerController.I.LocalPlayer.Motor.SetAimRotation(false);
        };
        userControl.OnDirectionalAim += dir =>
        {
            PlayerController.I.LocalPlayer.View.CmdSetAim(true, false);
            PlayerController.I.LocalPlayer.Motor.SetAimRotation(dir);
        };
        userControl.OnShoot += () =>
        {
            Debug.LogWarning($"Try Shoot");
            var player = PlayerController.I.LocalPlayer;
            var closest = PlayerController.I.GetClosest(player);
            if (closest == null) return;
            var dir = (closest.Motor.Position - player.Motor.Position).ToV2_xz().normalized;
            Debug.LogWarning($"Shoot {dir}");
            PlayerController.I.LocalPlayer.View.CmdSetAim(true, true);
            PlayerController.I.LocalPlayer.Motor.SetAimRotation(dir);
            PlayerController.I.LocalPlayer.Combat.CmdShoot(dir);
            PlayerController.I.LocalPlayer.View.CmdSetAim(false, false);
            PlayerController.I.LocalPlayer.Motor.SetAimRotation(false);
        };
    }
}
