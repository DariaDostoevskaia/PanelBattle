using LegoBattaleRoyal.Characters.Interfaces;
using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.Panels.Controllers;
using LegoBattaleRoyal.ScriptableObjects;
using UnityEngine;

namespace LegoBattaleRoyal.App
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private CharacterView _characterViewPrefab;
        [SerializeField] private CharacterSO _characterSO; //создать геймсеттинг с so, переместить grid panel settings so
        [SerializeField] private PanelSO[] _panelSettings;
        [SerializeField] private Transform _levelContainer;

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

            //panel controller
            //foreach (var (panelModel, panelView) in pairs)
            //{
            //    panelView.OnClicked;
            //    panelView.transform.position;
            //    _characterController.MoveCharacter();
            //}
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