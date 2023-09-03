using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace LegoBattaleRoyal.App
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private CharacterView _characterViewPrefab;
        [SerializeField] private Transform _levelContainer;
        [SerializeField] private GameSettingsSO _gameSettingsSO;
        private readonly Dictionary<Guid, (Characters.Controllers.CharacterController, PanelController)> _players = new();

        private event Action OnDisposed;

        private void Start()
        {
            var characterSO = _gameSettingsSO.CharacterSO;
            var gridFactory = new GridFactory(_gameSettingsSO.PanelSettings);

            var pairs = gridFactory.CreatePairs(_levelContainer);

            var characterRepository = new CharacterRepository();
            var roundController = new RoundController();

            for (int i = 0; i < _gameSettingsSO.BotCount; i++)
            {
                CreatePlayer(characterSO, characterRepository, pairs, true, roundController);
            }
            CreatePlayer(characterSO, characterRepository, pairs, false, roundController);

            characterRepository
                .GetAll()
                .ToList()
                .ForEach(character =>
                {
                    var availablePair = pairs.First(pair => pair.panelModel.IsJumpBlock);
                    availablePair.panelModel.BuildBase(character.Id);

                    var (characterController, panelController) = _players[character.Id];

                    characterController.ForceMoveCharacter(availablePair.panelView.transform.position);
                    panelController.MarkToAvailableNeighborPanels(availablePair.panelModel.GridPosition, character.JumpLenght);
                });
        }

        private void CreatePlayer(CharacterSO characterSO, CharacterRepository characterRepository,
            (Panels.Models.PanelModel panelModel, Panels.View.PanelView panelView)[] pairs, bool isAi,
            RoundController roundController)
        {
            var characterModel = isAi
                ? new AICharacterModel(characterSO.JumpLenght)
                : new CharacterModel(characterSO.JumpLenght);

            characterRepository.Add(characterModel);

            var characterView = Instantiate(_characterViewPrefab);
            var playerColor = characterModel.Id.ToColor();
            characterView.SetColor(playerColor);

            characterView.SetJumpHeight(characterSO.JumpHeight);
            characterView.SetMoveDuration(characterSO.MoveDuration);

            var characterController = new Characters.Controllers.CharacterController(characterModel, characterView, characterRepository);

            var panelController = new PanelController(pairs, characterModel, _players[characterModel.Id]);
            panelController.OnMoveSelected += characterController.MoveCharacter;

            if (characterModel is AICharacterModel)
            {
                var aiController = new AIController();
                roundController.OnRoundChanged += aiController.ProcessRoundState; //disposed
            }
            else
            {
                //characterController.OnMoved
                //создать метолд он мувд который триггерит OnRoundChanged, который триггерит ботов ходить
            }
            _players[characterModel.Id] = (characterController, panelController);

            OnDisposed += () =>
            {
                panelController.OnMoveSelected -= characterController.MoveCharacter;
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