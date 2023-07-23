using LegoBattaleRoyal.Characters.Domain;
using LegoBattaleRoyal.Characters.View;
using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private CharacterView _characterView;
        private Characters.Controllers.CharacterController _characterController;

        private void Start()
        {
            var characterModel = new CharacterModel(3f);
            _characterController = new Characters.Controllers.CharacterController(characterModel, _characterView);
        }

        private void Update()
        {
            _characterController.Update();
        }
    }
}