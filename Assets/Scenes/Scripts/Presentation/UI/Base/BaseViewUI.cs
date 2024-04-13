using System;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.Base
{
    public abstract class BaseViewUI : MonoBehaviour
    {
        public event Action Shown;

        public event Action Closed;

        public void Show()
        {
            gameObject.SetActive(true);
            Shown?.Invoke();
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
            Closed?.Invoke();
        }

        protected virtual void OnDestroy()
        {
            Shown = null;
            Closed = null;
        }
    }
}