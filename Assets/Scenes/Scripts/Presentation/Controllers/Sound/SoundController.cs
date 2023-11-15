using UnityEngine;

namespace LegoBattaleRoyal.Presentation.Controllers.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        }

        public void Play(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }
    }
}