using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.View
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterView : MonoBehaviour
    {
        private static readonly float MinimumPositionY = 1f;
        private Rigidbody _rigidbody;
        private Tween _move;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void JumpTo(float moveDuration, float jumpHeight, Vector3 endValue)
        {
            if (_move != null && _move.IsActive())
                return;

            _move?.Kill();

            var movePoint = new Vector3(endValue.x, MinimumPositionY, endValue.z);
            _move = _rigidbody.DOJump(movePoint, jumpHeight, 1, moveDuration);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = new Vector3(position.x, MinimumPositionY, position.z);
        }
    }
}