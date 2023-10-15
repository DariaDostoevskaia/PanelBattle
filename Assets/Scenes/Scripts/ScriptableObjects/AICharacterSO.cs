using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    [CreateAssetMenu(fileName = nameof(AICharacterSO), menuName = "Config/Characters/AI Character SO")]
    public class AICharacterSO : CharacterSO
    {
        [SerializeField] private int _blocksToCapture;
        [SerializeField] private Difficulty _difficulty;

        public int BlocksToCapture => _blocksToCapture;

        public Difficulty Difficulty => _difficulty;
    }
}