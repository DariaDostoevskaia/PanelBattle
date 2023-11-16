using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.Controllers.Sound
{
    public class VolumeController : MonoBehaviour
    {
        public string _volumeParameter = "MasterVolume";

        public AudioMixer _audioMixer;
        public Slider _slider;

        private float _volumeValue;
        private const float _multiplier = 20f;

        private void Awake()
        {
            _slider.onValueChanged.AddListener(HandleSliderValueChanged);
        }

        private void Start()
        {
            _volumeValue = PlayerPrefs.GetFloat(_volumeParameter, Mathf.Log10(_slider.value) * _multiplier);
            _slider.value = Mathf.Pow(10f, _volumeValue / _multiplier);
        }

        public void HandleSliderValueChanged(float value)
        {
            _volumeValue = Mathf.Log10(value) * _multiplier;
            _audioMixer.SetFloat(_volumeParameter, _volumeValue);
        }

        private void OnDisable()
        {
            PlayerPrefs.SetFloat(_volumeParameter, _volumeValue);
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveAllListeners();
        }
    }
}