using LegoBattaleRoyal.Presentation.UI.Base;
using System;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

namespace LegoBattaleRoyal.Presentation.Controllers.Sound
{
    public class SettingsPopup : BaseViewUI
    {
        public event Action OnCloseClicked;

        public event Action OnOkClicked;

        public event Action OnHomeClicked;

        public event Action<float> OnMusicVolumeChanged;

        public event Action<float> OnSoundVolumeChanged;

        [SerializeField] private Slider _soundSlider;
        [SerializeField] private Slider _musicSlider;

        [SerializeField] private Button _okButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _closeButton;

        private void Start()
        {
            _musicSlider.onValueChanged.AddListener((value) => OnMusicVolumeChanged?.Invoke(value));
            _soundSlider.onValueChanged.AddListener((value) => OnSoundVolumeChanged?.Invoke(value));

            _okButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetFloat(SoundController.MusicVolume, _musicSlider.value);
                PlayerPrefs.SetFloat(SoundController.SoundVolume, _soundSlider.value);
                PlayerPrefs.Save();

                gameObject.SetActive(false);

                OnOkClicked?.Invoke();
            });

            _homeButton.onClick.AddListener(() =>
            {
                LoadPrefs();

                gameObject.SetActive(false);

                OnHomeClicked?.Invoke();
            });

            _closeButton.onClick.AddListener(() =>
            {
                LoadPrefs();

                gameObject.SetActive(false);

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
        }

        public void Close()
        {
            gameObject.SetActive(false);
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

            OnMusicVolumeChanged = null;
            OnSoundVolumeChanged = null;

            _musicSlider.onValueChanged.RemoveAllListeners();
            _soundSlider.onValueChanged.RemoveAllListeners();

            _okButton.onClick.RemoveAllListeners();
            _homeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}