using LegoBattaleRoyal.Characters.Controllers;
using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.Characters.View;
using System;

namespace LegoBattaleRoyal.AI
{
    public class AIController : CharacterController, IDisposable
    {
        private static readonly CharacterModel _characterModel;

        public AIController(AICharacterModel aicharacterModel, CharacterView characterView, CharacterRepository characterRepository)
            : base(_characterModel, characterView, characterRepository)
        {
        }

        public void ProcessRoundState()
        {
            // ����� ���������� �� � ������� ����� ����������
            // OnPanelClick ������ ���������� ��
        }

        public void Dispose()
        {
        }
    }
}