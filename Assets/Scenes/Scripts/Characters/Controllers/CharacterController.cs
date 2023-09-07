using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.Panels.Controllers;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.Controllers
{
    public class CharacterController
    {
        private readonly CharacterView _characterView;
        private CharacterModel _characterModel;
        private AICharacterModel _aicharacterModel;
        private CharacterRepository _characterRepository;
        private PanelController _panelController;

        public CharacterController(CharacterModel characterModel,
            CharacterView characterView,
            CharacterRepository characterRepository)
        {
            _characterView = characterView;
            _characterModel = characterModel;
            _characterRepository = characterRepository;
        }

        public CharacterController(AICharacterModel aicharacterModel,
          CharacterView characterView,
          CharacterRepository characterRepository)
        {
            _characterView = characterView;
            _aicharacterModel = aicharacterModel;
            _characterRepository = characterRepository;
        }

        public void MoveCharacter(Vector3 hitPoint)
        {
            _characterView.JumpTo(hitPoint);
        }

        public void ForceMoveCharacter(Vector3 position)
        {
            _characterView.SetPosition(position);
        }

        public void OnMoved()
        {
            // метод должен триггерить ботов ходить
            var characterController = new CharacterController(_aicharacterModel, _characterView, _characterRepository);
            _panelController.OnMoveSelected += characterController.MoveCharacter;
        }
    }
}