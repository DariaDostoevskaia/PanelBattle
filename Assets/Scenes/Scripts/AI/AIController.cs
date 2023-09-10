using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using System;
using System.Linq;

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
            var panelView = _pairs
                .OrderBy(pair => Guid.NewGuid())
                .First(pair => pair.panelModel.IsAvailable(_aiCharacterModel.Id)).panelView;

            _panelController.OnPanelClicked(panelView);
        }
    }
}