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

        private bool _isMoving = false;
        private Vector3 _targetPosition;
        public float _speed = 1f;
        private float moveDuration;
        private float jumpHeight;
        private Vector3 point;
        private Tween _move;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            JumpTo(moveDuration, jumpHeight, point);
        }

        public void JumpTo(float moveDuration, float jumpHeight, Vector3 point)
        {
            if (_move != null && _move.IsActive())
                _move.Kill();
            var moveSpeed = _characterSO.Speed;

            var movePoint = new Vector3(point.x, _rigidbody.position.y, point.z);
            _move = _rigidbody.DOJump(point, jumpHeight, 1, moveDuration);

            //var positionX = moveDuration;
            //var positionY = jumpHeight;

            //var moveSpeed = _characterSO.Speed;

            //var mousePosition = Input.mousePosition;

            //_rigidbody.velocity = new Vector3(mousePosition.x * moveSpeed, mousePosition.y * positionY, _rigidbody.position.z);

            //if (Input.GetMouseButton(0))
            //{
            //    _targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    _isMoving = true;
            //}

            //if (_isMoving)
            //{
            //    transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.fixedDeltaTime * _speed);
            //    if (transform.position == _targetPosition)
            //    {
            //        _targetPosition = false;
            //    }
            //}

            //if (Input.GetMouseButtonDown(0))
            //{
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //    RaycastHit placeInfo;
            //    if (Physics.Raycast(ray, out placeInfo))
            //    {
            //        if (placeInfo.collider.CompareTag("Ground"))
            //        {
            //            _targetPosition = new Vector3(placeInfo.point.x, transform.position.y, placeInfo.point.z);
            //            _isMoving = true;
            //        }
            //    }
            //}

            //if (_isMoving == true)
            //{
            //    transform.LookAt(_targetPosition);
            //    transform.Translate(new Vector3(Input.mousePosition.x /** _speed * Time.deltaTime*/, jumpHeight));
            //    if (Vector3.Distance(_targetPosition, transform.position) < 0.01)
            //    {
            //        _isMoving = false;
            //    }
            //}
        }

        //private Vector3 FindWhereClicked(MouseState ms)
        //{
        //    Vector3 nearScreenPoint = new Vector3(ms.X, ms.Y, 0);
        //    Vector3 farScreenPoint = new Vector3(ms.X, ms.Y, 1);

        //    Vector3 nearWorldPoint = device.Viewport
        //    .Unproject(nearScreenPoint, cam.projectionMatrix, cam.viewMatrix, Matrix.Identity);

        //    Vector3 farWorldPoint = device.Viewport
        //    .Unproject(farScreenPoint, cam.projectionMatrix, cam.viewMatrix, Matrix.Identity);

        //    Vector3 direction = farWorldPoint - nearWorldPoint;

        //    float zFactor = -nearWorldPoint.Y / direction.Y;

        //    Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;

        //    return zeroWorldPoint;
        //}
    }
}