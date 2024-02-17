using LegoBattaleRoyal.Core.Levels;
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

        public void Init(LevelModel level)
        {
            _levelOrderText.SetText(level.Order.ToString());
            _button.onClick.AddListener(() => Clicked?.Invoke(level));
        }

        private void OnDestroy()
        {
            Clicked = null;
            _button.onClick.RemoveAllListeners();
        }
    }
}