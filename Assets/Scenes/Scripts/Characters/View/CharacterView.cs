using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.View
{
    [RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
    public class CharacterView : MonoBehaviour
    {
        private static readonly float MinimumPositionY = 1f;
        private Rigidbody _rigidbody;
        private Tween _move;
        private float _moveDuration;
        private float _jumpHeight;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void JumpTo(Vector3 endValue)
        {
            if (_move != null && _move.IsActive())
                return;

            _move?.Kill();

            var movePoint = new Vector3(endValue.x, MinimumPositionY, endValue.z);
            _move = _rigidbody.DOJump(movePoint, _jumpHeight, 1, _moveDuration);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = new Vector3(position.x, MinimumPositionY, position.z);
        }

        public void SetJumpHeight(float jumpHeight)
        {
            _jumpHeight = jumpHeight;
        }

        public void SetMoveDuration(float moveDuration)
        {
            _moveDuration = moveDuration;
        }

        public void SetColor(Color newColor)
        {
            _meshRenderer.material.color = newColor;
        }
    }
}