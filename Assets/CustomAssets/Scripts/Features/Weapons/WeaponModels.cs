using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneLine;
using System.Collections.ObjectModel;

[CreateAssetMenu(fileName = "WeaponModels", menuName = "MyBravlBattle/WeaponModels")]
public class WeaponModels : ScriptableObject
{

    [SerializeField, OneLine(Header = LineHeader.Short)]
    WeaponKindModelPair[] weapons;

    ReadOnlyDictionary<WeaponKind, WeaponModel> weaponModelDict = null;
    public ReadOnlyDictionary<WeaponKind, WeaponModel> WeaponModelDict
    {
        get
        {
            if (weaponModelDict != null) return weaponModelDict;
            var dict = new Dictionary<WeaponKind, WeaponModel>(weapons.Length);
            foreach (var pair in weapons) dict[pair.kind] = pair.model;
            weaponModelDict = new ReadOnlyDictionary<WeaponKind, WeaponModel>(dict);
            return weaponModelDict;
        }
    }


    [System.Serializable]
    public class WeaponKindModelPair
    {
        public WeaponKind kind;
        public WeaponModel model;
    }
}
