using LegoBattaleRoyal.ScriptableObjects;
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
        [SerializeField] private GameSettingsSO _gameSettingsSO;

        private AudioSource _audioSource;

        private static readonly float Multiplier = 20f;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;

            var volumeValue = PlayerPrefs.GetFloat(MusicVolume, 0f);
            SetMusicVolume(volumeValue);
        }

        public void Play(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }

        public void PLayWinGameMusic()
        {
            _audioSource.loop = false;
            Play(_gameSettingsSO.WinGameMusic);
        }

        public void PlayLoseGameMusic()
        {
            _audioSource.loop = false;
            Play(_gameSettingsSO.LoseGameMusic);
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