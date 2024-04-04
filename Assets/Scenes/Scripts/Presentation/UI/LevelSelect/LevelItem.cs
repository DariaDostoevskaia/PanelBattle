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

        [SerializeField] private Image _levelOrder;

        public void Init(LevelModel level, LevelSO levelSO)
        {
            _levelOrderText.SetText(level.Order.ToString());
            _button.onClick.AddListener(() => Clicked?.Invoke(level));

            _levelOrder.sprite = levelSO.LevelIcon;
        }

        private void OnDestroy()
        {
            Clicked = null;
            _button.onClick.RemoveAllListeners();
        }
    }
}