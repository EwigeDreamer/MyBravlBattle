using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VersionChecker
{
    static bool m_IsDifferentVersion = false;
    public static bool IsDifferentVersion => m_IsDifferentVersion;

    const string m_AppVersionPrefsKey = "AppVersion";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        string currentVer = Application.version;
        string savedVer = PlayerPrefs.GetString(m_AppVersionPrefsKey, null);
        if (currentVer == savedVer) return;
        m_IsDifferentVersion = true;
        PlayerPrefs.SetString(m_AppVersionPrefsKey, currentVer);
    }
}
