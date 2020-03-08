using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Factory;
using System;

namespace Weapons.Factory
{
    //public interface IProjectileFactory : IAbstractFactory<ProjectileKind, Projectile> { }
    //public interface IWeaponFactory : IAbstractFactory<WeaponKind, Weapon> { }
}
namespace Effects.Factory
{
    public interface IAudioPointFactory : IFactory<AudioPoint> { }
    public interface IVisualEffectPointFactory : IFactory<ParticlesPoint> { }
}
