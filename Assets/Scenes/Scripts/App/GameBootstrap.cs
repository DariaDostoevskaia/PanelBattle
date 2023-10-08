using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Controllers.AI;
using LegoBattaleRoyal.Controllers.CapturePath;
using LegoBattaleRoyal.Controllers.EndGame;
using LegoBattaleRoyal.Controllers.Panel;
using LegoBattaleRoyal.Controllers.Round;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Presentation.Panel;
using LegoBattaleRoyal.ScriptableObjects;
using LegoBattaleRoyal.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class GameBootstrap : MonoBehaviour, IDisposable
    {
        private event Action OnDisposed;
        public event Action OnRestarted;
        [SerializeField] private Transform _levelContainer;
        [SerializeField] private GameSettingsSO _gameSettingsSO;
        [SerializeField] private GamePanel _gamePanel; //TODO move to ui container

        private readonly Dictionary<Guid, (Controllers.Character.CharacterController, PanelController)> _players = new();


        public void Configure()
        {
            var characterSO = _gameSettingsSO.CharacterSO;

            var gridFactory = new GridFactory(_gameSettingsSO.PanelSettings);

            var pairs = gridFactory.CreatePairs(_levelContainer);

            var characterRepository = new CharacterRepository();

            var roundController = new RoundController();

            var endGameController = new EndGameController(_gamePanel, characterRepository);
            endGameController.OnGameRestarted += OnRestarted;

            for (int i = 0; i < _gameSettingsSO.BotCount; i++)
            {
                CreatePlayer(characterSO, characterRepository, pairs, true, roundController, endGameController);
            }
            CreatePlayer(characterSO, characterRepository, pairs, false, roundController, endGameController);

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

            OnDisposed += () =>
            {
                foreach (var pair in pairs)
                {
                    pair.panelModel.Dispose();
                    Destroy(pair.panelView.gameObject);
                }
            };
        }

        public void CreatePlayer(CharacterSO characterSO, CharacterRepository characterRepository,
            (PanelModel panelModel, PanelView panelView)[] pairs, bool isAi, RoundController roundController, EndGameController endGameController)
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

            var characterController = new Controllers.Character.CharacterController(characterModel, characterView, capturePathController, characterRepository);

            panelController.OnMoveSelected += characterController.MoveCharacter;

            panelController.SubscribeOnCallBack();
            panelController.OnCharacterLoss += OnCharacterLoss;

            if (characterModel is AICharacterModel)
            {
                var aiController = new AIController(panelController, pairs, characterModel);
                roundController.OnRoundChanged += aiController.ProcessRound;

                panelController.OnCharacterLoss += TryWinGame; //проверить все ли уничт. TryWin - Win

                OnDisposed += () => roundController.OnRoundChanged -= aiController.ProcessRound;

                void TryWinGame()
                {
                    roundController.OnRoundChanged -= aiController.ProcessRound;

                    panelController.OnCharacterLoss -= TryWinGame;

                    endGameController.TryWinGame();
                }
            }
            else // TODO extract method CreateMainPlayerModule and CreateAIPlayerModule
            {
                panelController.OnMoveSelected += ChangeRound;
                panelController.SubscribeOnInput();
                //lose
                panelController.OnCharacterLoss += LoseGame;
            }

            _players[characterModel.Id] = (characterController, panelController);

            OnDisposed += () =>
            {
                panelController.UnsubscribeOnInput();

                panelController.OnMoveSelected -= ChangeRound;
                panelController.OnMoveSelected -= characterController.MoveCharacter;

                characterController.Dispose();
                panelController.Dispose();
                characterModel.Dispose();
            };

            void ChangeRound(Vector3 vector)
            {
                roundController.ChangeRound();
            }

            void OnCharacterLoss()
            {
                capturePathController.ResetPath();
                characterController.KillCharacter();
                panelController.UnscribeOnCallBack();
                panelController.OnMoveSelected -= characterController.MoveCharacter;
                panelController.OnCharacterLoss -= OnCharacterLoss;
            }

            void LoseGame()
            {
                //TODO EndGameController LoseGame
                endGameController.LoseGame();

                panelController.OnMoveSelected -= ChangeRound;
                panelController.UnsubscribeOnInput();
                panelController.OnCharacterLoss -= LoseGame;
            }


        }
        public void Dispose()
        {
            OnDisposed?.Invoke();
            OnDisposed = null;
            OnRestarted = null;
        }

        private void OnDestroy()
        {
            Dispose();
        }

    }
}