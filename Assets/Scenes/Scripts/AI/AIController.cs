using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using System;
using CharacterController = LegoBattaleRoyal.Characters.Controllers.CharacterController;

namespace LegoBattaleRoyal.AI
{
    public class AIController : CharacterController, IDisposable
    {
        public AIController(AICharacterModel characterModel, CharacterView characterView, CharacterRepository characterRepository)
            : base(characterModel, characterView, characterRepository)
        {
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