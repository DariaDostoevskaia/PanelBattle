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

        private QuickOutline _hoverOutline;
        [SerializeField] private MeshRenderer _ownerHoverRenderer;

        private Color _defaultColor;

        private void Awake()
        {
            _hoverOutline = GetComponent<QuickOutline>();
            _defaultColor = _ownerHoverRenderer.material.color;
            _ownerHoverRenderer.enabled = false;
            CancelHighlight();
        }

        public void SetColor(Color color)
        {
            _ownerHoverRenderer.material.color = color;
            _ownerHoverRenderer.enabled = true;
        }

        public void ResetColor()
        {
            _ownerHoverRenderer.material.color = _defaultColor;
        }

        public void Highlight(Color color)
        {
            _hoverOutline.OutlineColor = color;
            _hoverOutline.enabled = true;
        }

        public void CancelHighlight()
        {
            _hoverOutline.enabled = false;
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