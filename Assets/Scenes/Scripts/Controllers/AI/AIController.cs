using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Controllers.Panel;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Presentation.Panel;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.Controllers.AI
{
    public class AIController
    {
        private readonly AICharacterModel _aiCharacterModel;
        private readonly PanelController _panelController;
        private readonly (PanelModel panelModel, PanelView panelView)[] _pairs;

        public AIController(PanelController panelController,
            (PanelModel panelModel, PanelView panelView)[] pairs,
            AICharacterModel aiCharacterModel)
        {
            _aiCharacterModel = aiCharacterModel;
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
            var jumpedPairs = _pairs
                .Where(pair => _panelController
                .CanJump(pair.panelModel));

            var panelToJump = _aiCharacterModel.DecideMove();

            return jumpedPairs.First(pair => pair.panelModel == panelToJump);
        }
    }
}