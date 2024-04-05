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

        public void Init(LevelModel level, LevelSO levelSO)
        {
            _levelOrderText.SetText(level.Order.ToString());
            _levelIcon.sprite = levelSO.LevelIcon;

            if (level.IsFinished == true)
                _levelMedal.gameObject.SetActive(true);
            else
                _levelMedal.gameObject.SetActive(false);

            _levelName.SetText(levelSO.LevelName.ToString());
            _levelPrice.SetText(levelSO.Price.ToString());

            _button.onClick.AddListener(() => Clicked?.Invoke(level));
        }

        private void OnDestroy()
        {
            Clicked = null;
            _button.onClick.RemoveAllListeners();
        }
    }
}