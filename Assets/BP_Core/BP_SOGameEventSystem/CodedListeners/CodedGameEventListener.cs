using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BP.Core
{
    [System.Serializable]
    public class CodedGameEventListener<T> : IGameEventListener<T>
    {
        [SerializeField] private BaseGameEvent<T> m_event = null;
        public void SetEvent(BaseGameEvent<T> newEvent) { m_event = newEvent; }
        private Action<T> m_onResponse;

        public void OnEventRaised(T val)
        {
            m_onResponse?.Invoke(val);
        }

        public void Register(BaseGameEvent<T> eventToRegister, Action<T> response)
        {
            m_event = eventToRegister;
            if(!m_event) { Debug.Log("missing event"); }
            if (m_event != null) m_event.RegisterListener(this);
            m_onResponse = response;
        }

        public void Unregister()
        {
            if (m_event != null) m_event.UnregisterListener(this);
            m_onResponse = null;
        }
    }
}

