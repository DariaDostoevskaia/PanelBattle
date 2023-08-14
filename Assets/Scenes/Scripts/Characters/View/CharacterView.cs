using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.View
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterView : MonoBehaviour
    {
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

            var movePoint = new Vector3(endValue.x, _rigidbody.position.y, endValue.z);
            _move = _rigidbody.DOJump(movePoint, jumpHeight, 1, moveDuration);
        }
    }
}