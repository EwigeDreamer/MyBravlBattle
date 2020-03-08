using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;

namespace MyTools.Data
{
    public abstract class DataSaverSingleton<TMe, TData> : Singleton<TMe>
        where TMe : DataSaverSingleton<TMe, TData>, new()
        where TData : new()
    {
        protected abstract string PrefsKey { get; }
        static public TData Data { get; private set; }

        public DataSaverSingleton()
        {
            var str = PlayerPrefs.GetString(PrefsKey, null);
            if (!string.IsNullOrWhiteSpace(str))
                Data = JsonUtility.FromJson<TData>(str);
            else Data = new TData();
            Application.quitting += Quit;
            Application.focusChanged += Focus;
        }
        ~DataSaverSingleton()
        {
            Application.quitting -= Quit;
            Application.focusChanged -= Focus;
        }

        void Focus(bool hasFocus) { if (!hasFocus) SaveData(); }
        void Quit() { SaveData(); }

        public void SaveData()
        { PlayerPrefs.SetString(PrefsKey, JsonUtility.ToJson(Data, true)); }
    }
}