using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Controllers.CapturePath;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Presentation.Panel;
using System;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

namespace LegoBattaleRoyal.Controllers.Panel
{
    public class PanelController : IDisposable
    {
        public event Action<Vector3> OnMoveSelected;
        public event Action OnCharacterLoss;

        private readonly (PanelModel panelModel, PanelView panelView)[] _pairs;
        private readonly CharacterModel _characterModel;
        private readonly CapturePathController _capturePathController;

        public PanelController((PanelModel panelModel, PanelView panelView)[] pairs,
            CharacterModel characterModel,
            CapturePathController capturePathController)
        {
            _characterModel = characterModel;
            _pairs = pairs;
            _capturePathController = capturePathController;
        }

        public void SubscribeOnInput()
        {
            foreach (var (panelModel, panelView) in _pairs)
            {
                panelView.OnClicked += OnPanelClicked;
                panelView.OnEntered += OnPanelHover;
                panelView.OnPointerExited += OnPanelExit;

            }
        }

        public void UnsubscribeOnInput()
        {
            foreach (var (panelModel, panelView) in _pairs)
            {
                panelView.OnClicked -= OnPanelClicked;
                panelView.OnEntered -= OnPanelHover;
                panelView.OnPointerExited -= OnPanelExit;
            }
        }

        public void SubscribeOnCallBack()
        {
            foreach (var (panelModel, panelView) in _pairs)
            {
                panelModel.OnReleased += OnPanelRealised;
            }
        }

        public void UnscribeOnCallBack()
        {
            foreach (var (panelModel, panelView) in _pairs)
            {
                panelModel.OnReleased -= OnPanelRealised;
            }
        }

        public void OnPanelRealised(Guid characterId)
        {
            if (_characterModel.Id != characterId)
                return;

            var hasCapturedPanelModels = _pairs.Any(pair => pair.panelModel.IsCaptured(characterId));

            if (hasCapturedPanelModels)
                return;

            OnCharacterLoss?.Invoke();
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

                    neighborPanelModel?.SetAvailable(_characterModel.Id);
                }
            }
        }

        public bool CanJump(PanelModel panelModel)
        {
            return panelModel.IsJumpBlock
               && panelModel.IsAvailable(_characterModel.Id)
               && !panelModel.IsVisiting(_characterModel.Id);
        }

        public void OnPanelClicked(PanelView view)
        {
            var panelModel = _pairs.First(pair => pair.panelView == view).panelModel;
            if (!CanJump(panelModel))
                return;

            var oldPanel = _pairs.First(pair => pair.panelModel.IsVisiting(_characterModel.Id)).panelModel;
            oldPanel.Remove(_characterModel.Id);

            panelModel.Add(_characterModel.Id);

            var captureIsReady = panelModel.IsCaptured(_characterModel.Id)
                && _pairs.Any(pair => pair.panelModel.IsOccupated(_characterModel.Id));

            if (captureIsReady)
                ProcessCapture();
            else
                panelModel.Occupate(_characterModel.Id);

            foreach (var (panel, _) in _pairs)
            {
                panel.SetUnavailable(_characterModel.Id);
            }
            MarkToAvailableNeighborPanels(panelModel.GridPosition, _characterModel.JumpLenght);

            var panelViewPosition = view.transform.position;
            OnMoveSelected?.Invoke(panelViewPosition);

            if (captureIsReady)
                _capturePathController.ResetPath();
        }

        private void OnPanelHover(PanelView view)
        {
            var panelModel = _pairs.First(pair => pair.panelView == view).panelModel;

            if (!panelModel.IsJumpBlock
                || panelModel.IsVisiting(_characterModel.Id))
                return;

            if (!panelModel.IsAvailable(_characterModel.Id))
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

        private void ProcessCapture()
        {
            var occupatePanels = _pairs.Where(pair => pair.panelModel.IsOccupated(_characterModel.Id)).ToArray();
            if (occupatePanels.Length == 0)
                throw new Exception("Occupated panels not found");

            var playerColor = _characterModel.Id.ToColor();

            foreach (var pair in occupatePanels)
            {
                _characterModel.Capture(pair.panelModel);
                pair.panelView.SetColor(playerColor);

            }
        }

        public void Dispose()
        {
            OnMoveSelected = null;
            OnCharacterLoss = null;
        }
    }
}