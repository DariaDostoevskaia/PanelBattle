using DG.Tweening;
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

        public void MoveTo(Vector3 point, float moveDuration)
        {
            if (_move != null && _move.IsActive())
                _move.Kill();

            var movePoint = new Vector3(point.x, _rigidbody.position.y, point.z);
            _move = _rigidbody.DOMove(movePoint, moveDuration);
        }
    }
}