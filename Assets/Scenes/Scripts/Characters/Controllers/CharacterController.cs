using LegoBattaleRoyal.Characters.Interfaces;
using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using System;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.Controllers
{
    public class CharacterController : IDisposable
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

        public void MoveCharacter(Vector3 hitPoint)
        {
            _characterView.JumpTo(_characterModel.MoveDuration, _characterModel.JumpHeight, hitPoint);
        }

        public void Dispose()
        {
            _inputService.OnClicked -= MoveCharacter;
        }
    }
}