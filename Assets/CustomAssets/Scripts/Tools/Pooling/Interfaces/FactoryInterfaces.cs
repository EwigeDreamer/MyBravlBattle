using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Factory;
using System;

public interface IProjectileFactory : IAbstractFactory<ProjectileKind, Projectile> { }
public interface IWeaponFactory : IAbstractFactory<WeaponKind, Weapon> { }

public interface IAudioPointFactory : IFactory<AudioPoint> { }
public interface IVisualEffectPointFactory : IAbstractFactory<string, ParticlesFX> { }
