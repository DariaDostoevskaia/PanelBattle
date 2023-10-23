using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.GameView.Character
{
    [RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
    public class CharacterView : MonoBehaviour
    {
        public event Action<bool> OnJumped;

        private static readonly float MinimumPositionY = 1f;
        private Rigidbody _rigidbody;
        private MeshRenderer _meshRenderer;
        private Tween _move;
        private float _moveDuration;
        private float _jumpHeight;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void JumpTo(Vector3 endValue)
        {
            if (_move != null
                && _move.IsActive())
                return;

            _move?.Kill();

            var movePoint = new Vector3(endValue.x, MinimumPositionY, endValue.z);

            OnJumped?.Invoke(true);

            _move = _rigidbody
                .DOJump(movePoint, _jumpHeight, 1, _moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    transform.position = movePoint;
                    OnJumped?.Invoke(false);
                });
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

        private void OnDestroy()
        {
            OnJumped = null;
        }
    }
}