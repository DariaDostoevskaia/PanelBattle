using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(CharacterSO), menuName = "Config/Character SO")]
    public class CharacterSO : ScriptableObject
    {
        [SerializeField] private float _moveDuration = 3f;
        [SerializeField] private float _jumpHeight = 5f;

        public float MoveDuration => _moveDuration;

        public float JumpHeight => _jumpHeight;
    }
}