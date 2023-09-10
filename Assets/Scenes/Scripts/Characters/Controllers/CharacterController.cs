using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.Controllers
{
    public class CharacterController
    {
        private readonly CharacterView _characterView;

        public CharacterController(CharacterModel characterModel,
            CharacterView characterView,
            CharacterRepository characterRepository)
        {
            _characterView = characterView;
        }

        public void MoveCharacter(Vector3 hitPoint)
        {
            _characterView.JumpTo(hitPoint);
        }

        public void ForceMoveCharacter(Vector3 position)
        {
            _characterView.SetPosition(position);
        }
    }
}