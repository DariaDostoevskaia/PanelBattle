using LegoBattaleRoyal.Characters.Interfaces;
using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.Panels.Models;
using LegoBattaleRoyal.Panels.View;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace LegoBattaleRoyal.App
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private CharacterView _characterViewPrefab;
        [SerializeField] private CharacterSO _characterSO;
        [SerializeField] private PanelSO[] _panelSettings;
        [SerializeField] private Transform _levelContainer;
        //[SerializeField] private PanelController _panelController;

        private IInputService _inputService;
        private Characters.Controllers.CharacterController _characterController;

        private void Start()
        {
            var characterModel = new CharacterModel(_characterSO.MoveDuration, _characterSO.JumpHeight, _characterSO.Speed);

            var characterView = Instantiate(_characterViewPrefab);

            _inputService = new InputService();

            _characterController = new Characters.Controllers.CharacterController(characterModel, characterView, _inputService);

            var gridFactory = new GridFactory(_panelSettings);
            var pairs = gridFactory.CreatePairs(_levelContainer);

            foreach (var (panelModel, panelView) in pairs)
            {
                panelView.OnClicked += GetClick;
                var panelViewPosition = panelView.transform.position;

                _characterController.MoveCharacter(panelViewPosition);

                //_panelController = new PanelController(panelModel, panelView, panelViewPosition);
            }
        }

        private void GetClick(PanelView view)
        {
            //view.OnPointerClick(?);
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