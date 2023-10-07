using LegoBattaleRoyal.Controllers.CapturePath;
using LegoBattaleRoyal.Presentation.Character;
using UnityEngine;

namespace LegoBattaleRoyal
{
    public class Destroyer
    {
        private CapturePathController _capturePathController;
        private CharacterView _characterView;

        public Destroyer(CapturePathController capturePathController, CharacterView characterView)
        {
            _capturePathController = capturePathController;
            _characterView = characterView;
        }

        public void DestroyCharacter()
        {
            _capturePathController.ResetPath();
            _characterView.gameObject.SetActive(false);
            GameObject.Destroy(_characterView);
        }

    }
}
