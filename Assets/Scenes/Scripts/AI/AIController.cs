using LegoBattaleRoyal.Characters.Controllers;
using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using LegoBattaleRoyal.Panels.Controllers;
using System;
using UnityEngine;
using CharacterController = LegoBattaleRoyal.Characters.Controllers.CharacterController;
using Random = UnityEngine.Random;

namespace LegoBattaleRoyal.AI
{
    public class AIController : CharacterController, IDisposable
    {
        private static readonly CharacterModel _characterModel;
        private AICharacterModel _aicharacterModel;
        private CharacterView _characterView;

        public AIController(AICharacterModel aicharacterModel, CharacterView characterView, CharacterRepository characterRepository)
            : base(_characterModel, characterView, characterRepository)
        {
            _characterView = characterView;
            _aicharacterModel = aicharacterModel;
        }

        public void ProcessRoundState()
        {
            // метод реализации ии с помощью панел контроллер
            // OnPanelClick должег перемещать ии
        }

        public void Dispose()
        {
        }
    }
}