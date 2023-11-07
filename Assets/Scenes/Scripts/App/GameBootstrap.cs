using LegoBattaleRoyal.ApplicationLayer.SaveSystem;
using LegoBattaleRoyal.Core.Characters.Models;
using LegoBattaleRoyal.Core.Levels;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.Core.Panels.Models;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Presentation.Controllers.AI;
using LegoBattaleRoyal.Presentation.Controllers.CapturePath;
using LegoBattaleRoyal.Presentation.Controllers.EndGame;
using LegoBattaleRoyal.Presentation.Controllers.Levels;
using LegoBattaleRoyal.Presentation.Controllers.Panel;
using LegoBattaleRoyal.Presentation.Controllers.Round;
using LegoBattaleRoyal.Presentation.GameView.Panel;
using LegoBattaleRoyal.Presentation.UI.Container;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class GameBootstrap : MonoBehaviour, IDisposable
    {
        public event Action OnRestarted;

        private event Action OnDisposed;

        [SerializeField] private Transform _levelContainer;
        private LevelModel _currentLevel;
        private readonly Dictionary<Guid, (Presentation.Controllers.Character.CharacterController, PanelController)> _players = new();

        public void Configure(ILevelRepository levelRepository, GameSettingsSO gameSettingsSO, LevelSO[] levelsSO,
            LevelController levelController, UIContainer uiContainer, ISaveService saveService)
        {
            var characterSO = gameSettingsSO.CharacterSO;

            levelController = new LevelController(levelRepository, saveService);

            _currentLevel = levelRepository.GetCurrentLevel();

            var levelSO = levelsSO[_currentLevel.Order - 1];

            var gridFactory = new GridFactory(levelSO);

            var pairs = gridFactory.CreatePairs(_levelContainer);

            var characterRepository = new CharacterRepository();

            var roundController = new RoundController();

            var endGameController = new EndGameController(uiContainer.EndGamePopup, characterRepository);
            endGameController.OnGameRestarted += OnRestarted;

            for (int i = 0; i < levelSO.AICharactersSO.Length; i++)
            {
                CreatePlayer(levelSO.AICharactersSO[i], characterRepository, pairs, roundController, endGameController, gameSettingsSO);
            }
            CreatePlayer(characterSO, characterRepository, pairs, roundController, endGameController, gameSettingsSO);

            characterRepository
                .GetAll()
                .ToList()
                .ForEach(character =>
                {
                    var availablePair = pairs
                    .OrderBy(pair => Guid.NewGuid())
                    .First(pair => pair.panelModel.IsJumpBlock
                    && !pair.panelModel.IsBase);

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

                foreach (var pair in pairs)
                {
                    pair.panelModel.Dispose();
                    pair.panelView.DestroyGameObject();
                }
            };
        }

        public void CreatePlayer(CharacterSO characterSO, CharacterRepository characterRepository,
            (PanelModel panelModel, PanelView panelView)[] pairs, RoundController roundController,
            EndGameController endGameController, GameSettingsSO gameSettingsSO)
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

            var capturePathView = Instantiate(gameSettingsSO.CapturePathViewPrefab);
            capturePathView.SetColor(playerColor);

            var capturePathController = new CapturePathController(capturePathView);

            var panelController = new PanelController(pairs, characterModel, capturePathController);

            var characterController = new Presentation.Controllers.Character.CharacterController
                (characterModel, characterView, capturePathController, characterRepository);

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

                characterView.DestroyGameObject();
                characterModel.Dispose();
                panelController.Dispose();
                roundController.Dispose();
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

        public void CreateMainPlayerModule(PanelController panelController, RoundController roundController,
            EndGameController endGameController)
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
                _currentLevel.Win();
                _currentLevel.Exit();
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