using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

using WK = WeaponKind;
using PK = ProjectileKind;
using IE = ImpactEffect;
using DictWKPK = System.Collections.Generic.Dictionary<WeaponKind, ProjectileKind>;
using DictPKIE = System.Collections.Generic.Dictionary<ProjectileKind, System.Collections.ObjectModel.ReadOnlyCollection<ImpactEffect>>;
using RODictWKPK = System.Collections.ObjectModel.ReadOnlyDictionary<WeaponKind, ProjectileKind>;
using RODictPKIE = System.Collections.ObjectModel.ReadOnlyDictionary<ProjectileKind, System.Collections.ObjectModel.ReadOnlyCollection<ImpactEffect>>;

public enum WeaponKind
{
    Unknown = -1,
    Pistol,
    Rifle,
    Shotgun,
}
public enum ProjectileKind
{
    Unknown = -1,
    PistolBullet,
    RifleBullet,
    ShotgunBullet,
    ShotgunSubBullet,
}
public static class WeaponStaticData
{
    public static RODictWKPK WeaponProjectileBindData { get; } = new RODictWKPK(new DictWKPK
        {
            { WK.Pistol, PK.PistolBullet },
            { WK.Rifle, PK.RifleBullet },
            { WK.Shotgun, PK.ShotgunBullet },
            { WK.Unknown, PK.Unknown },
        });

    public static RODictPKIE ProjectileEffectBindData { get; } = new RODictPKIE(new DictPKIE
        {
            { PK.PistolBullet, new List<IE> {
                new DamageImpactEffect(10),
            }.AsReadOnly() },

            { PK.RifleBullet, new List<IE> {
                new DamageImpactEffect(30),
            }.AsReadOnly() },

            { PK.ShotgunBullet, new List<IE> {
                new DamageImpactEffect(20),
            }.AsReadOnly() },
        });
}
