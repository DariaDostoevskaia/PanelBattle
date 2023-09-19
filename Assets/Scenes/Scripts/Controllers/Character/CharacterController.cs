using LegoBattaleRoyal.Controllers.CapturePath;
using LegoBattaleRoyal.Controllers.Panel;
using LegoBattaleRoyal.Presentation.Character;
using System;
using UnityEngine;

namespace LegoBattaleRoyal.Controllers.Character
{
    public class CharacterController : IDisposable
    {
        private readonly CharacterView _characterView;
        private readonly CapturePathController _capturePathController;
        private readonly PanelController _panelController;

        public CharacterController(CharacterView characterView,
            CapturePathController capturePathController,
            PanelController panelController)
        {
            _characterView = characterView;
            _capturePathController = capturePathController;
            _panelController = panelController;

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
            {
                _capturePathController.UnBind();
                _panelController.ProcessCapture(_capturePathController);
            }
        }

        public void Dispose()
        {
            _characterView.OnJumped -= OnCharacterMoved;
        }
    }
}