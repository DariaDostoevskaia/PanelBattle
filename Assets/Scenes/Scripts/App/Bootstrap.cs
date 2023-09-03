using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private CharacterView _characterViewPrefab;
        [SerializeField] private Transform _levelContainer;
        [SerializeField] private GameSettingsSO _gameSettingsSO;

        private Characters.Controllers.CharacterController _characterController;

        private event Action OnDisposed;

        private void Start()
        {
            var characterSO = _gameSettingsSO.CharacterSO;
            var characterModel = new CharacterModel(characterSO.JumpLenght);

            var characterView = Instantiate(_characterViewPrefab);
            characterView.SetJumpHeight(characterSO.JumpHeight);
            characterView.SetMoveDuration(characterSO.MoveDuration);

            _characterController = new Characters.Controllers.CharacterController(characterModel, characterView);

            var gridFactory = new GridFactory(_gameSettingsSO.PanelSettings);

            var pairs = gridFactory.CreatePairs(_levelContainer);

            var panelController = new PanelController(pairs, characterModel);
            panelController.OnMoveSelected += _characterController.MoveCharacter;

            var availablePair = pairs.First(pair => pair.panelModel.IsJumpBlock);

            availablePair.panelModel.BuildBase();

            _characterController.ForceMoveCharacter(availablePair.panelView.transform.position);
            panelController.MarkToAvailableNeighborPanels(availablePair.panelModel.GridPosition, characterModel.JumpLenght);

            OnDisposed += () =>
            {
                panelController.OnMoveSelected -= _characterController.MoveCharacter;
                panelController.Dispose();
            };
        }

        private void OnDestroy()
        {
            OnDisposed?.Invoke();
            OnDisposed = null;
        }
    }
}