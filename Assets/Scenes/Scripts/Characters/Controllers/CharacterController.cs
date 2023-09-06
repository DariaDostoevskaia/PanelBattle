using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.Controllers
{
    public class CharacterController
    {
        private readonly CharacterView _characterView;
        private CharacterModel _characterModel;

        private GameObject _gameObject;

        public CharacterController(CharacterModel characterModel,
            CharacterView characterView,
            CharacterRepository characterRepository)
        {
            _characterView = characterView;
            _characterModel = characterModel;
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
            //создать метод он мувд который триггерит OnRoundChanged, который триггерит ботов ходить
        }

        public void OnTriggerEnter(Collider collider)
        {
            {
                if (collider.gameObject.CompareTag("Player"))
                    _gameObject.SetActive(false);
            }
        }

        public void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
                _gameObject.SetActive(true);
        }
    }
}