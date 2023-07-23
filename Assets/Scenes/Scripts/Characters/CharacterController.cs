using LegoBattaleRoyal.Characters.Domain;
using LegoBattaleRoyal.Characters.View;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.Controllers
{
    public class CharacterController : IUpdate
    {
        private readonly CharacterModel _characterModel;
        private readonly CharacterView _characterView;
        private readonly Camera _camera;

        public CharacterController(CharacterModel characterModel, CharacterView characterView)
        {
            _characterModel = characterModel;
            _characterView = characterView;
            _camera = Camera.main;
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = Input.mousePosition;
                var ray = _camera.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(ray, out var hit))
                    _characterView.MoveTo(hit.point, _characterModel.MoveDuration);
            }
        }
    }
}