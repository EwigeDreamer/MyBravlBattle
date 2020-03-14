using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneLine;
using System.Collections.ObjectModel;

[CreateAssetMenu(fileName = "ProjectilePrefabs", menuName = "MyBravlBattle/ProjectilePrefabs")]
public class ProjectilePrefabs : ScriptableObject
{
    [SerializeField, OneLine(Header = LineHeader.Short)]
    ProjectilePrefabPair[] projectiles;

    ReadOnlyDictionary<ProjectileKind, Projectile> projectilePrefabDict = null;
    public ReadOnlyDictionary<ProjectileKind, Projectile> ProjectilePrefabDict
    {
        get
        {
            if (projectilePrefabDict != null) return projectilePrefabDict;
            var dict = new Dictionary<ProjectileKind, Projectile>(projectiles.Length);
            foreach (var pair in projectiles) dict[pair.kind] = pair.prefab;
            projectilePrefabDict = new ReadOnlyDictionary<ProjectileKind, Projectile>(dict);
            return projectilePrefabDict;
        }
    }

    [System.Serializable]
    public class ProjectilePrefabPair
    {
        public ProjectileKind kind;
        public Projectile prefab;
    }
}
