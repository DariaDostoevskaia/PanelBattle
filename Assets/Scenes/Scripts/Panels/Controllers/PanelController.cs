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

        private (PanelModel panelModel, PanelView panelView)[] _pairs;

        public PanelController((PanelModel panelModel, PanelView panelView)[] pairs)
        {
            _pairs = pairs;

            foreach (var (panelModel, panelView) in pairs)
            {
                panelView.OnClicked += OnPanelClicked;
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

        public void Dispose()
        {
            foreach (var (panelModel, panelView) in _pairs)
            {
                panelView.OnClicked -= OnPanelClicked;
            }
            OnMoveSelected = null;
        }
    }
}