using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LegoBattaleRoyal.Presentation.Panel
{
    public class PanelView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<PanelView> OnClicked;

        public event Action<PanelView> OnEntered;

        public event Action<PanelView> OnPointerExited;

        public event Action<PanelView> OnDestoyed;

        [SerializeField] private MeshRenderer _hoverRenderer;
        private MeshRenderer _renderer;

        private Color _defaultColor;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _defaultColor = _renderer.material.color;
            CancelHighlight();
        }

        public void SetColor(Color color)
        {
            _renderer.material.color = color;
        }

        public void ResetColor()
        {
            _renderer.material.color = _defaultColor;
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