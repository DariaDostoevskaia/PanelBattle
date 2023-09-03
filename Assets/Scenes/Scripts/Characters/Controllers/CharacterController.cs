using LegoBattaleRoyal.Characters.Interfaces;
using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using System;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.Controllers
{
    public class CharacterController
    {
        private readonly CharacterModel _characterModel;
        private readonly CharacterView _characterView;

        public CharacterController(CharacterModel characterModel,
            CharacterView characterView)
        {
            _characterModel = characterModel;
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