using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

namespace MyTools.Menu
{
    public delegate void SetActiveAction(IMenuUIController controller, bool state, bool forced);
    public interface IMenuUIController
    {
        event SetActiveAction OnSetActive;
        bool IsActive { get; }
        GameObject GO { get; }
        void SetActive(bool state, bool forced = false);
    }
    public class MenuUIController<TMe, TUI> : MonoBinderSingleton<TMe, TUI>, IMenuUIController
        where TMe : MenuUIController<TMe, TUI>
        where TUI : MenuUI<TUI, TMe>
    {
        public IMenuUIController Interface => this;
        public event SetActiveAction OnSetActive;
        protected TUI UI => BindedObj1;
        bool m_IsActive = true;
        public bool IsActive => m_IsActive;
        public void SetActive(bool state, bool forced = false)
        {
            UI.SetActive(state, forced);
            m_IsActive = state;
            SetActiveImplement(state, forced);
            OnSetActive?.Invoke(this, state, forced);
        }
        protected virtual void SetActiveImplement(bool state, bool forced) { }
    }
    public class MenuUI<TMe, TCtrl> : MonoBinderSingleton<TMe, TCtrl>
        where TMe : MenuUI<TMe, TCtrl>
        where TCtrl : MenuUIController<TCtrl, TMe>
    {
        protected TCtrl Controller => BindedObj1;
        public bool IsActive => BindedObj1.IsActive;
        public virtual void SetActive(bool state, bool forced) { GO.SetActive(state); }
    }
}


