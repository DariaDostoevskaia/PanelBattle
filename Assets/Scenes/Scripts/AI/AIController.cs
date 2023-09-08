using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using CharacterController = LegoBattaleRoyal.Characters.Controllers.CharacterController;

namespace LegoBattaleRoyal.AI
{
    public class AIController : CharacterController, IDisposable
    {
        public event Action OnRound;

        private enum State
        {
            Round,
            Player,
            Bot,
        }

        private State _state;
        private static CharacterModel _characterModel;
        private static AICharacterModel _aicharacterModel;

        public AIController(CharacterModel characterModel, CharacterView characterView, CharacterRepository characterRepository)
            : base(characterModel, characterView, characterRepository)
        {
            if (characterModel is AICharacterModel)
                _aicharacterModel = (AICharacterModel)characterModel;

            _characterModel = characterModel;
        }

        public void ProcessRoundState(AICharacterModel aicharacterModel)
        {
            // метод реализации ии с помощью панел контроллер
            // OnPanelClick должег перемещать ии

            _state = State.Round;
            _aicharacterModel = aicharacterModel;
            //for (int i = 0; i < _gameSettingsSO.BotCount; i++)
            //{
            //}
        }

        public void Dispose()
        {
        }
    }
}