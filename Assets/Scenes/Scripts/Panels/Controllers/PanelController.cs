using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using System;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

namespace LegoBattaleRoyal.Panels.Controllers
{
    public class PanelController : IDisposable
    {
        public event Action<Vector3> OnMoveSelected;

        public event Action<Vector3, Color> OnHoverSelected;

        public event Action<Vector3, Color> UnSelected;

        private (PanelModel panelModel, PanelView panelView)[] _pairs;
        private MeshRenderer _renderer;
        private MeshRenderer _startRenderer;

        public PanelController((PanelModel panelModel, PanelView panelView)[] pairs)
        {
            _pairs = pairs;

            foreach (var (panelModel, panelView) in pairs)
            {
                panelView.OnClicked += OnPanelClicked;
                panelView.OnEntered += OnPanelHover;
                panelView.OnDestoyed += OnPanelExit;
            }
        }

        private void OnPanelClicked(PanelView view)
        {
            var panelModel = _pairs.First(pair => pair.panelView == view).panelModel;

            if (!panelModel.IsJumpBlock
                || !panelModel.IsAvailable)
                return;

            var panelViewPosition = view.transform.position;
            OnMoveSelected?.Invoke(panelViewPosition);
        }

        private void OnPanelHover(PanelView view)
        {
            var panelModel = _pairs.First(pair => pair.panelView == view).panelModel;
            var panelView = _pairs.First(pair => pair.panelView == view).panelView;

            _renderer = panelView.GetComponent<MeshRenderer>();
            _startRenderer = panelView.GetComponent<MeshRenderer>();

            var panelViewPosition = view.transform.position;
            var color = _renderer.material.color;

            if (!panelModel.IsAvailable)
            {
                color = _renderer.material.color;
                color = Color.red;
            }
            if (!panelModel.IsJumpBlock)
            {
                color = _startRenderer.material.color;
            }
            else
            {
                color = _renderer.material.color;
                color = Color.green;
            }
            OnHoverSelected?.Invoke(panelViewPosition, color);
        }

        private void OnPanelExit(PanelView view)
        {
            var panelView = _pairs.First(pair => pair.panelView == view).panelView;
            _startRenderer = panelView.GetComponent<MeshRenderer>();

            var startColor = _startRenderer.material.color;

            var panelViewPosition = view.transform.position;

            UnSelected?.Invoke(panelViewPosition, startColor);
        }

        public void Dispose()
        {
            foreach (var (panelModel, panelView) in _pairs)
            {
                panelView.OnClicked -= OnPanelClicked;
                panelView.OnEntered -= OnPanelHover;
                panelView.OnDestoyed -= OnPanelExit;
            }
            OnMoveSelected = null;
            OnHoverSelected = null;
            UnSelected = null;
        }
    }
}