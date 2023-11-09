using LegoBattaleRoyal.Core.Characters.Models;
using LegoBattaleRoyal.Core.Panels.Models;
using LegoBattaleRoyal.Presentation.Controllers.Panel;
using LegoBattaleRoyal.Presentation.GameView.Panel;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.Controllers.AI
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