using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LegoBattaleRoyal.Panels.View
{
    public class PanelView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<PanelView> OnClicked;

        public event Action<PanelView> OnEntered;

        public event Action<PanelView> OnPointerExited;

        public event Action<PanelView> OnDestoyed;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEntered?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExited?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnDestoyed?.Invoke(this);
            OnDestoyed = null;

            OnClicked = null;
            OnEntered = null;
            OnPointerExited = null;
        }
    }
}