using DG.Tweening;
using LegoBattaleRoyal.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.View
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private CharacterSO _characterSO;
        private Rigidbody _rigidbody;
        private float _moveSpeed;

        private float moveDuration;
        private float jumpHeight;
        private Vector3 point;
        private Tween _move;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _moveSpeed = _characterSO.Speed;
        }

        private void Update()
        {
            JumpTo(moveDuration, jumpHeight, point);
        }

        public void JumpTo(float moveDuration, float jumpHeight, Vector3 point)
        {
            if (_move != null && _move.IsActive())
                _move.Kill();

            var movePoint = new Vector3(point.x, _rigidbody.position.y, point.z).normalized;
            _move = _rigidbody.DOJump(movePoint * _moveSpeed, jumpHeight, 1, moveDuration);
        }
    }
}