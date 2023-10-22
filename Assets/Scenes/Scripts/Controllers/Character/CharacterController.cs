using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Controllers.CapturePath;
using LegoBattaleRoyal.Presentation.Character;
using System;
using UnityEngine;

namespace LegoBattaleRoyal.Controllers.Character
{
    public class CharacterController : IDisposable
    {
        private readonly CharacterModel _characterModel;
        private readonly CharacterView _characterView;
        private readonly CapturePathController _capturePathController;
        private readonly CharacterRepository _characterRepository;

        public CharacterController(CharacterModel characterModel,
            CharacterView characterView,
            CapturePathController capturePathController,
            CharacterRepository characterRepository)
        {
            _characterModel = characterModel;
            _characterView = characterView;
            _capturePathController = capturePathController;
            _characterRepository = characterRepository;
            _characterView.OnJumped += OnCharacterMoved;
        }

        public void MoveCharacter(Vector3 hitPoint)
        {
            _characterView.JumpTo(hitPoint);
        }

        public void ForceMoveCharacter(Vector3 position)
        {
            _characterView.SetPosition(position);
        }

        private void OnCharacterMoved(bool isStartMove)
        {
            if (isStartMove)
                _capturePathController.BindTo(_characterView.transform);
            else
                _capturePathController.UnBind();
        }

        public void KillCharacter()
        {
            _characterRepository.Remove(_characterModel.Id);
            UnityEngine.Object.Destroy(_characterView.gameObject);
            Dispose();
        }

        public void Dispose()
        {
            _characterView.OnJumped -= OnCharacterMoved;
        }
    }
}