using LegoBattaleRoyal.Characters.Interfaces;
using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.Controllers
{
    public class CharacterController
    {
        private readonly CharacterModel _characterModel;
        private readonly CharacterView _characterView;
        private readonly IInputService _inputService;

        public CharacterController(CharacterModel characterModel,
            CharacterView characterView,
            IInputService inputService)
        {
            _characterModel = characterModel;
            _characterView = characterView;
            _inputService = inputService;
            _inputService.OnClicked += MoveCharacter;
        }

        private void MoveCharacter(Vector3 hitPoint)
        {
            hitPoint.x = _characterModel.MoveDuration;
            hitPoint.y = _characterModel.JumpHeight;

            _characterView.JumpTo(hitPoint.x, hitPoint.y, hitPoint);
        }
    }
}