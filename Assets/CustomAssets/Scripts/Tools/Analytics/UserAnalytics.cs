using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Prefs;

namespace Analytics
{
    public enum EventType
    {
        UserAction,
        GameEvent,
        AdsEvent,
        PurchaseEvent,
        InterfaceEvent,
        Error
    }
    public static class UserAnalytics
    {
        static readonly Dictionary<EventType, string> m_HeadlineDict = new Dictionary<EventType, string>
        {
            { EventType.UserAction, "USER_ACTIONS:{ {0} }" },
            { EventType.GameEvent, "GAME_EVENTS:{ {0} }" },
            { EventType.AdsEvent, "ADS_EVENTS:{ {0} }" },
            { EventType.PurchaseEvent, "PURCHASE_EVENTS:{ {0} }" },
            { EventType.InterfaceEvent, "INTERFACE_EVENTS:{ {0} }" },
            { EventType.Error, "ERROR:{ {0} }" }
        };

        const string m_CanSendKey = "CanSendUserAnalytics";
        static bool? m_CanSend = null;
        public static bool CanSend
        {
            get { return m_CanSend ?? MyPlayerPrefs.GetBool(m_CanSendKey, false); }
            set { MyPlayerPrefs.SetBool(m_CanSendKey, value); m_CanSend = value; }
        }

        public static void PushMessage(string str, EventType type)
        {
            //if (!CanSend) return;
            //if (AppMetrica.Instance == null) return;
            //if (string.IsNullOrWhiteSpace(str)) return;
            //string message = string.Format(m_HeadlineDict[type], str);
            //AppMetrica.Instance.ReportEvent(message);
        }
    }
}