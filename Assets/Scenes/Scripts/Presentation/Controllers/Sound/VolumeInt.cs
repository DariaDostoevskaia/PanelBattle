using UnityEngine;
using UnityEngine.Audio;

namespace LegoBattaleRoyal.Presentation.Controllers.Sound
{
    public class VolumeInt : MonoBehaviour
    {
        public string _volumeParameter = "MasterVolume";

        public AudioMixer _audioMixer;

        private void Start()
        {
            var volumeValue = PlayerPrefs.GetFloat(_volumeParameter, _volumeParameter == "CharacterVolume" ? 0f : -80f);
            _audioMixer.SetFloat(_volumeParameter, volumeValue);
        }
    }
}