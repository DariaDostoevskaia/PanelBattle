using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Controllers.AI;
using LegoBattaleRoyal.Controllers.CapturePath;
using LegoBattaleRoyal.Controllers.Panel;
using LegoBattaleRoyal.Controllers.Round;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Presentation.Panel;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class GameBootstrap : MonoBehaviour
    {
        private event Action OnDisposed;

        [SerializeField] private Transform _levelContainer;
        [SerializeField] private GameSettingsSO _gameSettingsSO;

        private readonly Dictionary<Guid, (Controllers.Character.CharacterController, PanelController)> _players = new();

        private RoundController _roundController;

        public void Configur()
        {
            var characterSO = _gameSettingsSO.CharacterSO;

            var gridFactory = new GridFactory(_gameSettingsSO.PanelSettings);

            var pairs = gridFactory.CreatePairs(_levelContainer);

            var characterRepository = new CharacterRepository();

            _roundController = new RoundController();

            for (int i = 0; i < _gameSettingsSO.BotCount; i++)
            {
                CreatePlayer(characterSO, characterRepository, pairs, true, _roundController);
            }
            CreatePlayer(characterSO, characterRepository, pairs, false, _roundController);

            characterRepository
                .GetAll()
                .ToList()
                .ForEach(character =>
                {
                    var availablePair = pairs
                    .OrderBy(pair => Guid.NewGuid())
                    .First(pair => pair.panelModel.IsJumpBlock);

                    availablePair.panelModel.BuildBase(character.Id);

                    var color = character.Id.ToColor();
                    availablePair.panelView.SetColor(color);

                    var (characterController, panelController) = _players[character.Id];

                    characterController.ForceMoveCharacter(availablePair.panelView.transform.position);
                    panelController.MarkToAvailableNeighborPanels(availablePair.panelModel.GridPosition, character.JumpLenght);
                });
        }

        public void CreatePlayer(CharacterSO characterSO, CharacterRepository characterRepository,
            (PanelModel panelModel, PanelView panelView)[] pairs, bool isAi, RoundController roundController)
        {
            var characterModel = isAi
                ? new AICharacterModel(characterSO.JumpLenght)
                : new CharacterModel(characterSO.JumpLenght);

            characterRepository.Add(characterModel);

            var characterView = isAi
                ? Instantiate(_gameSettingsSO.AICharacterSO.PlayerCharacterViewPrefab)
                : Instantiate(_gameSettingsSO.CharacterSO.PlayerCharacterViewPrefab);

            var playerColor = characterModel.Id.ToColor();

            characterView.SetColor(playerColor);

            characterView.SetJumpHeight(characterSO.JumpHeight);
            characterView.SetMoveDuration(characterSO.MoveDuration);

            var capturePathView = Instantiate(_gameSettingsSO.CapturePathViewPrefab);
            capturePathView.SetColor(playerColor);

            var capturePathController = new CapturePathController(capturePathView);

            var panelController = new PanelController(pairs, characterModel, capturePathController);

            var characterController = new Controllers.Character.CharacterController(characterView, capturePathController);

            panelController.OnMoveSelected += characterController.MoveCharacter;

            if (characterModel is AICharacterModel)
            {
                var aiController = new AIController(panelController, pairs, characterModel);
                roundController.OnRoundChanged += aiController.ProcessRound;

                OnDisposed += () => roundController.OnRoundChanged -= aiController.ProcessRound;
            }
            else
            {
                panelController.OnMoveSelected += ChangeRound;
                panelController.SubscribeOnInput();
            }

            _players[characterModel.Id] = (characterController, panelController);

            void ChangeRound(Vector3 vector)
            {
                roundController.ChangeRound();
            }

            OnDisposed += () =>
            {
                panelController.UnsubscribeOnInput();

                panelController.OnMoveSelected -= ChangeRound;
                panelController.OnMoveSelected -= characterController.MoveCharacter;

                characterController.Dispose();
                panelController.Dispose();
                characterModel.Dispose();
            };
        }

        private void OnDestroy()
        {
            OnDisposed?.Invoke();
            OnDisposed = null;
        }
    }
}