using LegoBattaleRoyal.Controllers.CapturePath;
using LegoBattaleRoyal.Presentation.Character;
using System;
using UnityEngine;

namespace LegoBattaleRoyal.Controllers.Character
{
    public class CharacterController : IDisposable
    {
        private readonly CharacterView _characterView;
        private readonly CapturePathController _capturePathController;

        public CharacterController(CharacterView characterView,
            CapturePathController capturePathController)
        {
            _characterView = characterView;
            _capturePathController = capturePathController;

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

            _capturePathController.UnBind();
        }

        public void Dispose()
        {
            _characterView.OnJumped -= OnCharacterMoved;
        }
    }
}