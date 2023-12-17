using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LegoBattaleRoyal.Presentation.GameView.Panel
{
    public class PanelView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<PanelView> OnClicked;

        public event Action<PanelView> OnEntered;

        public event Action<PanelView> OnPointerExited;

        public event Action<PanelView> OnDestoyed;

        [SerializeField] private MeshRenderer _hoverRenderer;
        [SerializeField] private MeshRenderer _ownerHoverRenderer;

        private Color _defaultColor;

        private void Awake()
        {
            _defaultColor = _ownerHoverRenderer.material.color;
            CancelHighlight();
        }

        public void SetColor(Color color)
        {
            _ownerHoverRenderer.material.color = color;
        }

        public void ResetColor()
        {
            _ownerHoverRenderer.material.color = _defaultColor;
        }

        public void Highlight(Color color)
        {
            _hoverRenderer.material.color = color;
            _hoverRenderer.gameObject.SetActive(true);
        }

        public void CancelHighlight()
        {
            _hoverRenderer.gameObject.SetActive(false);
        }

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