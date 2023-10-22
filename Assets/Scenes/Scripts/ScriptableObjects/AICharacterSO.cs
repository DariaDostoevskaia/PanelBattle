using LegoBattaleRoyal.Strategy.Difficulty;
using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(AICharacterSO), menuName = "Config/Characters/AI Character SO")]
    public class AICharacterSO : CharacterSO
    {
        [SerializeField] private int _blocksToCapture;
        [SerializeField] private Difficulty _difficulty;

        public int BlocksToCapture => _blocksToCapture;

        public Difficulty Difficulty => _difficulty;
    }
}