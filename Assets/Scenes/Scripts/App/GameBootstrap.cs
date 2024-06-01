using Cinemachine;
using EasyButtons;
using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Core.Characters.Models;
using LegoBattaleRoyal.Core.Levels.Contracts;
using LegoBattaleRoyal.Core.Panels.Models;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Infrastructure.Firebase.Analytics;
using LegoBattaleRoyal.Infrastructure.Unity.Ads;
using LegoBattaleRoyal.Presentation.Controllers.AI;
using LegoBattaleRoyal.Presentation.Controllers.CapturePath;
using LegoBattaleRoyal.Presentation.Controllers.EndGame;
using LegoBattaleRoyal.Presentation.Controllers.General;
using LegoBattaleRoyal.Presentation.Controllers.Leaderboard;
using LegoBattaleRoyal.Presentation.Controllers.Panel;
using LegoBattaleRoyal.Presentation.Controllers.Round;
using LegoBattaleRoyal.Presentation.Controllers.Sound;
using LegoBattaleRoyal.Presentation.Controllers.Wallet;
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
        [SerializeField] private CinemachineFreeLook _cinemachineCamera;
        [SerializeField] private UIContainer _uiContainer;

        private EndGameController _endGameController;
        private CharacterRepository _characterRepository;
        private ILevelRepository _levelRepository;
        private readonly Dictionary<Guid, (Presentation.Controllers.Character.CharacterController, PanelController)> _players = new();

        public void Configure(ILevelRepository levelRepository,
            GameSettingsSO gameSettingsSO,
            WalletController walletController,
            SoundController soundController,
           FirebaseAnalyticsProvider analyticsProvider,
           UnityAdsProvider adsProvider,
           CameraController cameraController,
           LeaderboardController leaderboardController)
        {
            _levelRepository = levelRepository;
            var characterSO = gameSettingsSO.CharacterSO;

            var currentLevel = levelRepository.GetCurrentLevel();
            Debug.Log($"Level: {currentLevel.Order}");
            var levelSO = gameSettingsSO.Levels[currentLevel.Order - 1];

            var music = levelSO.LevelMusic;
            soundController.Play(music);

            var gridFactory = new GridFactory(levelSO);

            var pairs = gridFactory.CreatePairs(_levelContainer);

            _characterRepository = new CharacterRepository();

            var roundController = new RoundController();
            var generalController = new GeneralController(_uiContainer.GeneralPopup, walletController, levelRepository, cameraController);

            _endGameController = new EndGameController(_characterRepository,
                levelRepository,
                soundController,
                walletController,
                generalController,
                adsProvider,
                leaderboardController);

            _endGameController.OnGameRestarted += OnRestarted;

            for (int i = 0; i < levelSO.AICharactersSO.Length; i++)
            {
                CreatePlayer(levelSO.AICharactersSO[i], _characterRepository, pairs, roundController,
                    _endGameController, gameSettingsSO, analyticsProvider);
            }
            CreatePlayer(characterSO, _characterRepository, pairs, roundController,
                _endGameController, gameSettingsSO, analyticsProvider);

            _characterRepository
                .GetAll()
                .ToList()
                .ForEach(character =>
                {
                    var availablePair = pairs
                    .OrderBy(pair => Guid.NewGuid())
                    .First(pair => pair.panelModel.IsJumpBlock
                    && !pair.panelModel.IsBase);

                    //var perimeterBlocks = pairs
                    //.OrderBy(pair => Guid.NewGuid())
                    //.Where(pair => pair.panelModel.IsJumpBlock)
                    //.Where(pair => pair.panelModel.GridPosition.Row == 0
                    //|| pair.panelModel.GridPosition.Row == /*gridRowCount - 1*/ levelSO.Rect[0]
                    //|| pair.panelModel.GridPosition.Column == 0
                    //|| pair.panelModel.GridPosition.Column == /*gridColumnCount - 1*/levelSO.Rect[1]);

                    character.Move(availablePair.panelModel);

                    availablePair.panelModel.BuildBase(character.Id);

                    var color = character.Id.ToColor();
                    availablePair.panelView.SetColor(color);

                    var (characterController, panelController) = _players[character.Id];

                    characterController.ForceMoveCharacter(availablePair.panelView.transform.position);
                    panelController.MarkToAvailableNeighborPanels(availablePair.panelModel.GridPosition, character.JumpLenght);
                });

            void GetPerimeterPanels()
            {
                var perimeterBlocks = pairs;

                foreach (var pair in pairs)
                {
                    var pairPosition = pair.panelModel.GridPosition;
                    int column = pairPosition.Column;
                    int row = pairPosition.Row;

                    var panelsRow = levelSO.Rect.First();
                    var panelsColumn = levelSO.Rect.Last();

                    //SortPerimetrArray(levelSO.Rect);

                    // pair => pair.panelModel.GridPosition.Row == 0
                    //|| pair.panelModel.GridPosition.Row == /*gridRowCount - 1*/ levelSO.Rect.First()
                    //|| pair.panelModel.GridPosition.Column == 0
                    //|| pair.panelModel.GridPosition.Column == /*gridColumnCount - 1*/levelSO.Rect.Last();
                }
            }

            OnDisposed += () =>
            {
                _endGameController.OnGameRestarted -= OnRestarted;

                foreach (var pair in pairs)
                {
                    pair.panelModel.Dispose();
                    pair.panelView.DestroyGameObject();
                }
            };
        }

        //public  List<Pair> GetPairsOnPerimeter(List<Pair> pairs, int[] rect)
        //{
        //    List<Pair> perimeterPairs = new List<Pair>();

        //    foreach (Pair pair in pairs)
        //    {
        //        if(pair.row == 0
        //            || pair.row == rect[0] - 1
        //            || pair.column == 0
        //            || pair.column == rect[1] - 1)
        //        {
        //            perimeterPairs.Add(pair);
        //        }
        //    }

        //    return perimeterPairs;
        //}

        private int[] GetPerimetrValue(int[,] array)
        {
            int arraySide = (int)Math.Sqrt(array.Length);
            int[] result = new int[(arraySide * 4 - 4)];
            int next = 0;

            for (int i = 0; i < arraySide; i++)
                result[next++] = array[0, i];

            for (int i = 1; i < arraySide - 1; i++)
                result[next++] = array[i, arraySide - 1];

            for (int i = arraySide - 1; i > 0; i--)
                result[next++] = array[arraySide - 1, i];

            for (int i = arraySide - 1; i > 0; i--)
                result[next++] = array[i, 0];

            return result;
        }

        public int[,] SortPerimetrArray(int[,] array)
        {
            int[] sortedValue = GetPerimetrValue(array);
            Array.Sort(sortedValue);
            int arraySide = (int)Math.Sqrt(array.Length);

            int next = 0;

            for (int i = 0; i < arraySide; i++)
                array[0, i] = sortedValue[next++];

            for (int i = 1; i < arraySide - 1; i++)
                array[i, arraySide - 1] = sortedValue[next++];

            for (int i = arraySide - 1; i > 0; i--)
                array[arraySide - 1, i] = sortedValue[next++];

            for (int i = arraySide - 1; i > 0; i--)
                array[i, 0] = sortedValue[next++];

            return array;
        }

        public void CreatePlayer(CharacterSO characterSO, CharacterRepository characterRepository,
            (PanelModel panelModel, PanelView panelView)[] pairs, RoundController roundController,
            EndGameController endGameController, GameSettingsSO gameSettingsSO,
            FirebaseAnalyticsProvider analyticsProvider)
        {
            var characterModel = characterSO is AICharacterSO aiCharacterSO

                ? new AICharacterModel(aiCharacterSO.JumpLenght, aiCharacterSO.BlocksToCapture,
                aiCharacterSO.Difficulty, pairs.Select(pair => pair.panelModel).ToArray())

                : new CharacterModel(characterSO.JumpLenght);

            characterRepository.Add(characterModel);

            var characterView = Instantiate(characterSO.ViewPrefab);

            var playerColor = characterModel.Id.ToColor();

            characterView.SetJumpHeight(characterSO.JumpHeight);
            characterView.SetMoveDuration(characterSO.MoveDuration);

            var capturePathView = Instantiate(gameSettingsSO.CapturePathViewPrefab);
            capturePathView.SetColor(playerColor);

            var capturePathController = new CapturePathController(capturePathView);

            var panelController = new PanelController(pairs, characterModel, characterView, capturePathController);

            var characterController = new Presentation.Controllers.Character.CharacterController
                (characterModel, characterView, capturePathController, characterRepository);

            panelController.OnMoveSelected += characterController.MoveCharacter;
            panelController.SubscribeOnCallBack();
            panelController.OnCharacterLoss += OnCharacterLoss;

            if (characterModel is AICharacterModel)
            {
                CreateAIPlayerModule(panelController, pairs, (AICharacterModel)characterModel,
                    roundController, endGameController, analyticsProvider);
            }
            else
            {
                CreateMainPlayerModule(panelController, roundController, endGameController, analyticsProvider);

                _cinemachineCamera.Follow = characterView.transform;
                _cinemachineCamera.LookAt = characterView.transform;
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
            EndGameController endGameController, FirebaseAnalyticsProvider analyticsProvider)
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
                analyticsProvider.SendEvent(AnalyticsEvents.Lose);

                panelController.OnMoveSelected -= ChangeRound;

                panelController.UnsubscribeOnInput();

                panelController.OnCharacterLoss -= LoseGame;
            }
        }

        public void CreateAIPlayerModule(PanelController panelController, (PanelModel panelModel, PanelView panelView)[] pairs,
            AICharacterModel characterModel, RoundController roundController, EndGameController endGameController,
            FirebaseAnalyticsProvider analyticsProvider)
        {
            var aiController = new AIController(panelController, pairs, characterModel);
            roundController.OnRoundChanged += aiController.ProcessRound;

            panelController.OnCharacterLoss += TryWinGame;

            void TryWinGame()
            {
                roundController.OnRoundChanged -= aiController.ProcessRound;

                panelController.OnCharacterLoss -= TryWinGame;

                endGameController.TryWinGame();
                analyticsProvider.SendEvent(AnalyticsEvents.Win);
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

#if DEBUG

        [Button]
        public void LoseLevel()
        {
            _endGameController.LoseGame();
        }

        [Button]
        public void WinGame()
        {
            var opponents = _characterRepository.GetOpponents().ToArray();
            foreach (var opponent in opponents)
            {
                _characterRepository.Remove(opponent.Id);
            }
            _endGameController.TryWinGame();
        }

        [Button]
        public void InvokeLevel(int order)
        {
            var currentLevel = _levelRepository.GetCurrentLevel();
            currentLevel.Exit();

            var nextLevel = _levelRepository.Get(order);
            nextLevel.Launch();
            OnRestarted?.Invoke();
        }

#endif
    }
}