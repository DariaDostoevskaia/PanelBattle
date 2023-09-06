using LegoBattaleRoyal.AI;
using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.Round;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private CharacterView _characterViewPrefab;
        [SerializeField] private Transform _levelContainer;
        [SerializeField] private GameSettingsSO _gameSettingsSO;

        private readonly Dictionary<Guid, (Characters.Controllers.CharacterController, PanelController)> _players = new();

        private Collider _collider;
        private RoundController _roundController;

        private event Action OnDisposed;

        private void Start()
        {
            var characterSO = _gameSettingsSO.CharacterSO;
            var gridFactory = new GridFactory(_gameSettingsSO.PanelSettings);

            var pairs = gridFactory.CreatePairs(_levelContainer);

            var characterRepository = new CharacterRepository();

            _roundController = new RoundController();
            _roundController.OnRoundChanged += _roundController.ChangeRound;

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
            var aicharacterSO = new AICharacterSO();

            var characterModel = isAi
                ? new AICharacterModel(aicharacterSO.JumpLenght)
                : new CharacterModel(characterSO.JumpLenght);

            characterRepository.Add(characterModel);

            var characterView = Instantiate(_characterViewPrefab);
            var playerColor = characterModel.Id.ToColor();
            characterView.SetColor(playerColor);

            characterView.SetJumpHeight(characterSO.JumpHeight);
            characterView.SetMoveDuration(characterSO.MoveDuration);

            var aicharacterController = new AIController((AICharacterModel)characterModel, characterView, characterRepository);

            var characterController = new Characters.Controllers.CharacterController(characterModel, characterView, characterRepository);

            var panelController = new PanelController(pairs, characterModel);
            panelController.OnMoveSelected += characterController.MoveCharacter;

            if (characterModel is AICharacterModel)
            {
                roundController.OnRoundChanged += aicharacterController.ProcessRoundState;

                aicharacterController.OnTriggerEnter(_collider);
                characterController.OnTriggerExit(_collider);
            }
            else
            {
                roundController.OnRoundChanged += characterController.OnMoved;

                aicharacterController.OnTriggerExit(_collider);
                characterController.OnTriggerEnter(_collider);
                //создать метод он мувд который триггерит OnRoundChanged, который триггерит ботов ходить
            }
            _players[characterModel.Id] = (characterController, panelController);
            //_players[characterModel.Id] = (aicharacterController, panelController);

            OnDisposed += () =>
            {
                roundController.OnRoundChanged -= aicharacterController.ProcessRoundState;
                roundController.OnRoundChanged -= characterController.OnMoved;

                panelController.OnMoveSelected -= characterController.MoveCharacter;

                panelController.Dispose();
                roundController.Dispose();
            };
        }

        private void OnDestroy()
        {
            _roundController.OnRoundChanged -= _roundController.ChangeRound;
            OnDisposed?.Invoke();
            OnDisposed = null;
        }
    }
}