using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using System;
using System.Linq;
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
            var pair = _pairs
                .OrderBy(pair => Guid.NewGuid())
                .First(pair => pair.panelModel.IsAvailable(_aiCharacterModel.Id)
                && !pair.panelModel.IsVisiting(_aiCharacterModel.Id)
                && pair.panelModel.IsJumpBlock);

            _panelController.OnPanelClicked(pair.panelView);
            Debug.Log($"{nameof(ProcessRound)}: {_aiCharacterModel.Id} move to {pair.panelModel.GridPosition}");
        }
    }
}