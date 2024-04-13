using LegoBattaleRoyal.Core.Levels;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.LevelSelect
{
    public class LevelItem : MonoBehaviour
    {
        public event Action<LevelModel> Clicked;

        [SerializeField] private TextMeshProUGUI _levelOrderText;
        [SerializeField] private Button _button;

        [SerializeField] private Image _levelIcon;
        [SerializeField] private RectTransform _levelMedal;
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private TextMeshProUGUI _levelPrice;
        [SerializeField] private RectTransform _gem;

        public void Init(LevelModel level, LevelSO levelSO)
        {
            _levelOrderText.SetText(level.Order.ToString());

            _levelIcon.sprite = levelSO.LevelIcon;

            _levelPrice.gameObject.SetActive(!level.IsFinished);
            _levelPrice.SetText(levelSO.Price.ToString());
            _gem.gameObject.SetActive(!level.IsFinished);

            _levelMedal.gameObject.SetActive(level.IsFinished);

            _levelName.SetText(levelSO.LevelName.ToString());

            _button.onClick.AddListener(() => Clicked?.Invoke(level));
        }

        private void OnDestroy()
        {
            Clicked = null;
            _button.onClick.RemoveAllListeners();
        }
    }
}