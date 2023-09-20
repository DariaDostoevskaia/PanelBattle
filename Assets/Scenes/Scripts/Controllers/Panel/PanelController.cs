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

        private readonly (PanelModel panelModel, PanelView panelView)[] _pairs;
        private readonly CharacterModel _characterModel;

        private (PanelModel panelModel, PanelView panelView) _pair;
        private CapturePathController _capturePathController;

        public PanelController((PanelModel panelModel, PanelView panelView)[] pairs,
            CharacterModel characterModel)
        {
            _characterModel = characterModel;
            _pairs = pairs;
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

            foreach (var (panel, _) in _pairs)
            {
                panel.SetUnavailable(_characterModel.Id);
            }
            MarkToAvailableNeighborPanels(panelModel.GridPosition, _characterModel.JumpLenght);

            _pair = (panelModel, view);

            var panelViewPosition = view.transform.position;
            OnMoveSelected?.Invoke(panelViewPosition);
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

        public void ProcessCapture(CapturePathController capturePathController)
        {
            //проверка на VIsitor (доб свойство оккупирровано) и на это свойство провер€ем захват

            //реализаци€ захвата

            //добавл€ем в chararacter model метод capture (передаетс€ модель панели)

            if (!_pair.panelModel.IsVisiting(_characterModel.Id))
                _pair.panelModel.Capture(_characterModel.Id);

            _characterModel.Capture(_pair.panelModel);

            //у Panel model будет метод capture, мен€ющий состо€ние

            _pair.panelModel.Capture(_characterModel.Id);

            var playerColor = _characterModel.Id.ToColor();
            _pair.panelView.SetColor(playerColor);
            // у panelView мен€ем цвет - от цвета »грока-захвата

            // в конце событие OnEndCaptured, на него подписываетс€ Capture pass controller и вызывает resetPath(4*)

            _capturePathController = capturePathController;
            _characterModel.OnEndCaptured += _capturePathController.ResetPath;
        }

        public void Dispose()
        {
            OnMoveSelected = null;
            _characterModel.OnEndCaptured -= _capturePathController.ResetPath;
        }
    }
}