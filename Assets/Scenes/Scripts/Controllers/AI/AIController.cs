using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Controllers.Panel;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Presentation.Panel;
using System;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.Controllers.AI
{
    public class AIController
    {
        private static CharacterModel _aiCharacterModel;
        private readonly PanelController _panelController;
        private readonly (PanelModel panelModel, PanelView panelView)[] _pairs;

        public AIController(PanelController panelController,
            (PanelModel panelModel, PanelView panelView)[] pairs,
            CharacterModel characterModel)
        {
            _aiCharacterModel = characterModel;
            _panelController = panelController;
            _pairs = pairs;
        }

        public void ProcessRound()
        {
            var (panelModel, panelView) = GetJumpPair();

            _panelController.OnPanelClicked(panelView);

            Debug.Log($"{nameof(ProcessRound)}: {_aiCharacterModel.Id} move to {panelModel.GridPosition}");
        }

        private (PanelModel, PanelView) GetJumpPair()
        {
            var pair = _pairs
              .OrderBy(pair => Guid.NewGuid())
              .First(pair => _panelController.CanJump(pair.panelModel));

            return pair;
        }
    }
}