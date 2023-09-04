using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace LegoBattaleRoyal.Characters.Models
{
    public class CharacterRepository
    {
        private readonly List<CharacterModel> _characterModels = new List<CharacterModel>();

        public void Add(CharacterModel characterModel)
        {
            _characterModels.Add(characterModel);
        }

        public CharacterModel Get(Guid id)
        {
            return _characterModels.First(x => x.Id == id);
        }

        public void Remove(Guid id)
        {
            var character = Get(id);
            _characterModels.Remove(character);
        }

        public IEnumerable<CharacterModel> GetOpponents()
        {
            return _characterModels.OfType<AICharacterModel>();
        }

        public IEnumerable<CharacterModel> GetAll()
        {
            return _characterModels;
        }
    }
}