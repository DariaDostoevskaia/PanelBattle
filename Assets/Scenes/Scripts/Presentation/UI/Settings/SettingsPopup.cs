using System;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

namespace LegoBattaleRoyal.Presentation.Controllers.Sound
{
    public class SettingsPopup : MonoBehaviour
    {
        public event Action OnCloseClicked;

        public event Action OnOkClicked;

        public event Action OnHomeClicked;

        public event Action Closed;

        public event Action<float> OnMusicVolumeChanged;

        public event Action<float> OnSoundVolumeChanged;

        [SerializeField] private Slider _soundSlider;
        [SerializeField] private Slider _musicSlider;

        [SerializeField] private Button _okButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _closeButton;

        [SerializeField] private RectTransform[] _parts;

        private void Start()
        {
            _musicSlider.onValueChanged.AddListener((value) => OnMusicVolumeChanged?.Invoke(value));
            _soundSlider.onValueChanged.AddListener((value) => OnSoundVolumeChanged?.Invoke(value));

            _okButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetFloat(SoundController.MusicVolume, _musicSlider.value);
                PlayerPrefs.SetFloat(SoundController.SoundVolume, _soundSlider.value);
                PlayerPrefs.Save();

                Close();

                OnOkClicked?.Invoke();
            });

            _homeButton.onClick.AddListener(() =>
            {
                LoadPrefs();

                Close();

                OnHomeClicked?.Invoke();
            });

            if (_closeButton != null)
                _closeButton.onClick.AddListener(() =>
            {
                LoadPrefs();

                Close();

                OnCloseClicked?.Invoke();
            });

            _musicSlider.SetValueWithoutNotify(PlayerPrefs
                .GetFloat(SoundController.MusicVolume, 1f));

            _soundSlider.SetValueWithoutNotify(PlayerPrefs
                .GetFloat(SoundController.SoundVolume, 1f));
        }

        public void Show()
        {
            gameObject.SetActive(true);
            foreach (var part in _parts)
            {
                part.gameObject.SetActive(true);
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
            foreach (var part in _parts)
            {
                part.gameObject.SetActive(false);
            }
            Closed?.Invoke();
        }

        private void LoadPrefs()
        {
            var musicVolume = PlayerPrefs.GetFloat(SoundController.MusicVolume, 1f);
            var soundVolume = PlayerPrefs.GetFloat(SoundController.SoundVolume, 1f);

            _musicSlider.SetValueWithoutNotify(musicVolume);
            _soundSlider.SetValueWithoutNotify(soundVolume);

            OnMusicVolumeChanged?.Invoke(musicVolume);
            OnSoundVolumeChanged?.Invoke(soundVolume);
        }

        private void OnDestroy()
        {
            OnOkClicked = null;
            OnHomeClicked = null;
            OnCloseClicked = null;
            Closed = null;

            OnMusicVolumeChanged = null;
            OnSoundVolumeChanged = null;

            _musicSlider.onValueChanged.RemoveAllListeners();
            _soundSlider.onValueChanged.RemoveAllListeners();

            _okButton.onClick.RemoveAllListeners();
            _homeButton.onClick.RemoveAllListeners();

            if (_closeButton != null)
                _closeButton.onClick.RemoveAllListeners();
        }
    }
}