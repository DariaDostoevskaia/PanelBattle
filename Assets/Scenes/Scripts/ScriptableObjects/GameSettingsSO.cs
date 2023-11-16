using LegoBattaleRoyal.Presentation.GameView.CapturePath;
using UnityEngine;

namespace LegoBattaleRoyal.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(GameSettingsSO), menuName = "Config/GameSettingsSO")]
    public class GameSettingsSO : ScriptableObject
    {
        [SerializeField] private CharacterSO _characterSO;
        [SerializeField] private CapturePathView _capturePathViewPrefab;
        [SerializeField] private LevelSO[] _levels;

        [SerializeField] private AudioClip _mainMusic;
        [SerializeField] private AudioClip _loseMusic;
        [SerializeField] private AudioClip _winMusic;
        [SerializeField] private AudioClip _losePanels;
        [SerializeField] private AudioClip _capturePanels;

        public CharacterSO CharacterSO => _characterSO;

        public LevelSO[] Levels => _levels;

        public CapturePathView CapturePathViewPrefab => _capturePathViewPrefab;

        public AudioClip MainMusic => _mainMusic;

        public AudioClip LoseGameMusic => _loseMusic;

        public AudioClip WinGameMusic => _winMusic;

        public AudioClip LosePanels => _losePanels;

        public AudioClip CapturePanels => _capturePanels;
    }
}