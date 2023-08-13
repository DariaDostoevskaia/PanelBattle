using LegoBattaleRoyal.Characters.Interfaces;
using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.ScriptableObjects;
using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private CharacterView _characterViewPrefab;
        [SerializeField] private CharacterSO _characterSO;

        private IInputService _inputService;
        private Characters.Controllers.CharacterController _characterController;

        private void Start()
        {
            var characterModel = new CharacterModel(_characterSO.MoveDuration, _characterSO.JumpHeight, _characterSO.Speed);

            var characterView = Instantiate(_characterViewPrefab);

            _inputService = new InputService();

            _characterController = new Characters.Controllers.CharacterController(characterModel, characterView, _inputService);
        }

        private void Update()
        {
            _inputService.Update();
        }

        private void OnDestroy()
        {
            _characterController.Dispose();
        }
    }
}