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

        public event Action<Material> OnHoverSelected;

        public event Action<Material> UnSelected;

        private (PanelModel panelModel, PanelView panelView)[] _pairs;
        private MeshRenderer _meshRenderer;

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
            //var panelView = _pairs.First(pair => pair.panelView == view).panelView;

            //var panelViewPosition = panelView.transform.position;

            if (!panelModel.IsJumpBlock
                || !panelModel.IsAvailable)
                _meshRenderer.material.color = Color.red;
            //{
            //}
            else
                _meshRenderer.material.color = Color.green;
            //{
            //    var materialColor = _meshRenderer.material.color = Color.green;
            //}
            OnHoverSelected?.Invoke(_meshRenderer.material);
        }

        private void OnPanelExit(PanelView view)
        {
            //var panelView = _pairs.First(pair => pair.panelView == view).panelView;
            //var panelViewPosition = panelView.transform.position;
            UnSelected?.Invoke(_meshRenderer.material);
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