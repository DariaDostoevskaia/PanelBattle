using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using System;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.Panels.Controllers
{
    public class PanelController : IDisposable
    {
        public event Action<Vector3> OnMoveSelected;

        public event Action<Vector3, Color> OnHoverSelected;

        public event Action<Vector3, Color> UnSelected;

        private (PanelModel panelModel, PanelView panelView)[] _pairs;
        private MeshRenderer _renderer;

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

            var color = _renderer.material.color;

            if (!panelModel.IsJumpBlock
                || !panelModel.IsAvailable)
            {
                _renderer.material.color = Color.red;
                color = _renderer.material.color;
                var panelViewPosition = view.transform.position;
                OnHoverSelected?.Invoke(panelViewPosition, color);
                return;
            }
            else
            {
                _renderer.material.color = Color.green;
                color = _renderer.material.color;
                var panelViewPosition = view.transform.position;
                OnHoverSelected?.Invoke(panelViewPosition, color);
                return;
            }
        }

        private void OnPanelExit(PanelView view)
        {
            var panelView = _pairs.First(pair => pair.panelView == view).panelView;
            _renderer = panelView.GetComponent<MeshRenderer>();

            var startColor = _renderer.material.color;

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