using DG.Tweening;
using LegoBattaleRoyal.Characters.Models;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.View
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private CharacterSO _characterSO;
        private Rigidbody _rigidbody;

        //private float _positionX;
        //private float _positionY;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void JumpTo(float moveDuration, float jumpHeight)
        {
            var positionX = moveDuration;
            var positionY = jumpHeight;

            var moveSpeed = _characterSO.Speed;

            var mousePosition = Input.mousePosition;

            _rigidbody.velocity = new Vector3(mousePosition.x * moveSpeed, mousePosition.y * positionY, _rigidbody.position.z);
        }
    }
}