using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;
using CharacterController = LegoBattaleRoyal.Characters.Controllers.CharacterController;

namespace LegoBattaleRoyal.AI
{
    public class AIController : CharacterController, IDisposable
    {
        private static CharacterModel _characterModel;
        private static AICharacterModel _aicharacterModel;

        private enum State
        {
            Round,
            Player,
            Bot,
        }

        private readonly Dictionary<Guid, State> _stateForCharacters;

        public AIController(CharacterModel characterModel, CharacterView characterView, CharacterRepository characterRepository)
            : base(characterModel, characterView, characterRepository)
        {
            if (characterModel is AICharacterModel)
                _aicharacterModel = (AICharacterModel)characterModel;

            _characterModel = characterModel;
        }

        public void ProcessRoundState()
        {
            // метод реализации ии с помощью панел контроллер
            // OnPanelClick должег перемещать ии

            _stateForCharacters.TryGetValue(_aicharacterModel.Id, out State state);
            state = _stateForCharacters[_aicharacterModel.Id] = new State();
        }

        public void Dispose()
        {
        }
    }
}