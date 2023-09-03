using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(CharacterSO), menuName = "Config/Characters/Character SO")]
    public class CharacterSO : ScriptableObject
    {
        [SerializeField] private float _moveDuration = 0.5f;
        [SerializeField] private float _jumpHeight = 5f;
        [SerializeField] private int _jumpLenght = 1;

        public float MoveDuration => _moveDuration;

        public float JumpHeight => _jumpHeight;

        public int JumpLenght => _jumpLenght;
    }
}