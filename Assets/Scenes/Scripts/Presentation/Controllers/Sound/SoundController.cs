using UnityEngine;
using UnityEngine.Audio;

namespace LegoBattaleRoyal.Presentation.Controllers.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : MonoBehaviour
    {
        public static readonly string MusicVolume = nameof(MusicVolume);
        public static readonly string SoundVolume = nameof(SoundVolume);

        [SerializeField] private AudioMixer _audioMixer;

        private AudioSource _audioSource;

        private static readonly float Multiplier = 20f;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            OnLoop();

            var volumeValue = PlayerPrefs.GetFloat(MusicVolume, 0f);
            SetMusicVolume(volumeValue);
        }

        public void Play(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }

        public void OffLoop()
        {
            _audioSource.loop = false;
        }

        public void OnLoop()
        {
            _audioSource.loop = true;
        }

        public void SetMusicVolume(float volume)
        {
            SetVolume(MusicVolume, volume);
        }

        public void SetSoundVolume(float volume)
        {
            SetVolume(SoundVolume, volume);
        }

        private void SetVolume(string mixerGroupName, float volume)
        {
            var volumeValue = Mathf.Log10(volume) * Multiplier;
            _audioMixer.SetFloat(mixerGroupName, volumeValue);
        }
    }
}