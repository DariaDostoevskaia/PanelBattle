using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.GameView.Character
{
    [RequireComponent(typeof(Rigidbody), (typeof(Animator)))]
    public class CharacterView : MonoBehaviour
    {
        public event Action<bool> OnJumped;

        [SerializeField] private AudioClip _jumpAudioClip;
        [SerializeField] private AudioClip _killCharacterAudioClip;
        [SerializeField] private AudioClip _capturePanelsAudioClip;

        private static readonly float MinimumPositionY = 1f;

        private Rigidbody _rigidbody;
        private AudioSource _audioSource;
        private Animator _animator;
        private Tween _move;
        private float _moveDuration;
        private float _jumpHeight;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();

            _audioSource.loop = false;
        }

        public void JumpTo(Vector3 endValue)
        {
            if (_move != null
                && _move.IsActive())
                return;

            _move?.Kill();

            var movePoint = new Vector3(endValue.x, MinimumPositionY, endValue.z);

            OnJumped?.Invoke(true);

            transform.LookAt(movePoint);

            _move = _rigidbody
                .DOJump(movePoint, _jumpHeight, 1, _moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    transform.position = movePoint;

                    PlaySound(_jumpAudioClip);
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
            //_meshRenderer.material.color = newColor;
        }

        private void PlaySound(AudioClip audioClip)
        {
            if (audioClip == null)
                return;
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }

        public void Die()
        {
            PlaySound(_killCharacterAudioClip);
        }

        public void Capture()
        {
            PlaySound(_capturePanelsAudioClip);
            _animator.SetTrigger("Jump"); //TODO animation const .cs
        }

        private void OnDestroy()
        {
            OnJumped = null;
        }
    }
}