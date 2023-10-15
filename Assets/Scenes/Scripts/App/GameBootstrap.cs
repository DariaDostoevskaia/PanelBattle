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
using LegoBattaleRoyal.UI.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class GameBootstrap : MonoBehaviour, IDisposable
    {
        public event Action OnRestarted;

        public event Action OnExited;

        private event Action OnDisposed;

        [SerializeField] private Transform _levelContainer;
        [SerializeField] private GameSettingsSO _gameSettingsSO;
        [SerializeField] private UIContainer _uIContainer;

        private readonly Dictionary<Guid, (Controllers.Character.CharacterController, PanelController)> _players = new();

        public void Configure()
        {
            var characterSO = _gameSettingsSO.CharacterSO;

            var gridFactory = new GridFactory(_gameSettingsSO.PanelSettings, _gameSettingsSO.GridPanelSettings);

            var pairs = gridFactory.CreatePairs(_levelContainer);

            var characterRepository = new CharacterRepository();

            var roundController = new RoundController();

            var endGameController = new EndGameController(_uIContainer.GamePanel, _uIContainer.MenuPanel, characterRepository);
            endGameController.OnGameRestarted += OnRestarted;
            endGameController.OnExitedMenu += OnExited;

            for (int i = 0; i < _gameSettingsSO.AICharactersSO.Length; i++)
            {
                CreatePlayer(_gameSettingsSO.AICharactersSO[i], characterRepository, pairs, roundController, endGameController);
            }
            CreatePlayer(characterSO, characterRepository, pairs, roundController, endGameController);

            characterRepository
                .GetAll()
                .ToList()
                .ForEach(character =>
                {
                    var availablePair = pairs
                    .OrderBy(pair => Guid.NewGuid())
                    .First(pair => pair.panelModel.IsJumpBlock);

                    character.Move(availablePair.panelModel);

                    availablePair.panelModel.BuildBase(character.Id);

                    var color = character.Id.ToColor();
                    availablePair.panelView.SetColor(color);

                    var (characterController, panelController) = _players[character.Id];

                    characterController.ForceMoveCharacter(availablePair.panelView.transform.position);
                    panelController.MarkToAvailableNeighborPanels(availablePair.panelModel.GridPosition, character.JumpLenght);
                });

            OnDisposed += () =>
            {
                endGameController.OnGameRestarted -= OnRestarted;
                endGameController.OnExitedMenu -= OnExited;

                foreach (var pair in pairs)
                {
                    pair.panelModel.Dispose();
                    pair.panelView.DestroyGameObject();
                }
            };
        }

        public void CreatePlayer(CharacterSO characterSO, CharacterRepository characterRepository,
            (PanelModel panelModel, PanelView panelView)[] pairs, RoundController roundController,
            EndGameController endGameController)
        {
            var characterModel = characterSO is AICharacterSO aiCharacterSO
                ? new AICharacterModel(aiCharacterSO.JumpLenght, aiCharacterSO.BlocksToCapture,
                aiCharacterSO.Difficulty, pairs.Select(pair => pair.panelModel).ToArray())
                : new CharacterModel(characterSO.JumpLenght);

            characterRepository.Add(characterModel);

            var characterView = Instantiate(characterSO.ViewPrefab);

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
                CreateAIPlayerModule(panelController, pairs, (AICharacterModel)characterModel, roundController, endGameController);
            }
            else
            {
                CreateMainPlayerModule(panelController, roundController, endGameController);
            }

            _players[characterModel.Id] = (characterController, panelController);

            OnDisposed += () =>
            {
                panelController.OnMoveSelected -= characterController.MoveCharacter;

                panelController.UnsubscribeOnInput();

                panelController.OnCharacterLoss -= OnCharacterLoss;

                characterModel.Dispose();
                panelController.Dispose();
                endGameController.Dispose();
            };

            void OnCharacterLoss()
            {
                capturePathController.ResetPath();

                characterController.KillCharacter();

                panelController.UnscribeOnCallBack();

                panelController.OnMoveSelected -= characterController.MoveCharacter;
                panelController.OnCharacterLoss -= OnCharacterLoss;
            }
        }

        public void CreateMainPlayerModule(PanelController panelController, RoundController roundController, EndGameController endGameController)
        {
            panelController.OnMoveSelected += ChangeRound;
            panelController.SubscribeOnInput();

            panelController.OnCharacterLoss += LoseGame;

            void ChangeRound(Vector3 vector)
            {
                roundController.ChangeRound();
            }

            void LoseGame()
            {
                endGameController.LoseGame();

                panelController.OnMoveSelected -= ChangeRound;

                panelController.UnsubscribeOnInput();

                panelController.OnCharacterLoss -= LoseGame;
            }
        }

        public void CreateAIPlayerModule(PanelController panelController, (PanelModel panelModel, PanelView panelView)[] pairs,
            AICharacterModel characterModel, RoundController roundController, EndGameController endGameController)
        {
            var aiController = new AIController(panelController, pairs, characterModel);
            roundController.OnRoundChanged += aiController.ProcessRound;

            panelController.OnCharacterLoss += TryWinGame;

            void TryWinGame()
            {
                roundController.OnRoundChanged -= aiController.ProcessRound;

                panelController.OnCharacterLoss -= TryWinGame;

                endGameController.TryWinGame();
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