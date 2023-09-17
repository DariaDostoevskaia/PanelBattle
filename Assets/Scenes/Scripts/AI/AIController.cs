using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LegoBattaleRoyal.AI
{
    public class AIController
    {
        private static CharacterModel _aiCharacterModel;
        private PanelController _panelController;
        private (PanelModel panelModel, PanelView panelView)[] _pairs;

        public AIController(PanelController panelController,
            (PanelModel panelModel, PanelView panelView)[] pairs,
            CharacterModel characterModel)
        {
            _panelController = panelController;
            _pairs = pairs;
            _aiCharacterModel = characterModel;
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