using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

namespace LegoBattaleRoyal.Panels.Controllers
{
    public class PanelController : IDisposable
    {
        public event Action<Vector3> OnMoveSelected;

        private (PanelModel panelModel, PanelView panelView)[] _pairs;
        private CharacterModel _characterModel;
        //private (Characters.Controllers.CharacterController, PanelController) _charactersId;
        //private CharacterRepository _characterRepository;

        public PanelController((PanelModel panelModel, PanelView panelView)[] pairs,
            CharacterModel characterModel,
            //(Characters.Controllers.CharacterController, PanelController) charactersId)
        {
            _pairs = pairs;
            _characterModel = characterModel;
            //_charactersId = charactersId;

            foreach (var (panelModel, panelView) in pairs)
            {
                //subscrideOnInput
                panelView.OnClicked += OnPanelClicked;
                panelView.OnEntered += OnPanelHover;
                panelView.OnPointerExited += OnPanelExit;
            }
        }

        public void MarkToAvailableNeighborPanels(GridPosition gridPosition, int movementRadius)
        {
            for (var rowOffset = -movementRadius; rowOffset <= movementRadius; rowOffset++)
            {
                var neighborRow = gridPosition.Row + rowOffset;
                if (neighborRow < 0)
                    continue;

                for (var columnOffset = -movementRadius; columnOffset <= movementRadius; columnOffset++)
                {
                    if (rowOffset == 0 && columnOffset == 0)
                        continue;

                    var neighborColumn = gridPosition.Column + columnOffset;
                    if (neighborColumn < 0)
                        continue;

                    var neighborGridPosition = new GridPosition(neighborRow, neighborColumn);

                    var (neighborPanelModel, _) = _pairs.FirstOrDefault(pair =>
                    {
                        var model = pair.panelModel;
                        if (!model.IsJumpBlock)
                            return false;

                        return pair.panelModel.GridPosition.Equals(neighborGridPosition);
                    });

                    neighborPanelModel?.SetAvailable(/*_mainCharacterId*/);
                }
            }
        }

        private void OnPanelClicked(PanelView view)
        {
            //var randomPairs = _pairs.OrderBy(pair => Guid.NewGuid());
            var panelModel = _pairs.First(pair => pair.panelView == view).panelModel;

            if (!panelModel.IsJumpBlock
                || !panelModel.IsAvailable
                || panelModel.IsVisiting)
                return;

            var oldPanel = _pairs.First(pair => pair.panelModel.IsVisiting).panelModel;
            oldPanel.Remove();

            panelModel.Add();

            var panelViewPosition = view.transform.position;

            foreach (var (panel, _) in _pairs)
            {
                panel.SetUnavailable();
            }
            MarkToAvailableNeighborPanels(panelModel.GridPosition, _characterModel.JumpLenght);

            OnMoveSelected?.Invoke(panelViewPosition);
        }

        private void OnPanelHover(PanelView view)
        {
            var panelModel = _pairs.First(pair => pair.panelView == view).panelModel;

            if (!panelModel.IsJumpBlock
                || panelModel.IsVisiting)
                return;

            if (!panelModel.IsAvailable)
            {
                view.Highlight(Color.red);
            }
            else
            {
                view.Highlight(Color.green);
            }
        }

        private void OnPanelExit(PanelView view)
        {
            view.CancelHighlight();
        }

        public void Dispose()
        {
            foreach (var (panelModel, panelView) in _pairs)
            {
                panelView.OnClicked -= OnPanelClicked;
                panelView.OnEntered -= OnPanelHover;
                panelView.OnPointerExited -= OnPanelExit;
            }
            OnMoveSelected = null;
        }
    }
}