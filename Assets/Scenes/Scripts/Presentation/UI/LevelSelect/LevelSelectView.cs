using LegoBattaleRoyal.Core.Levels;
using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.UI.LevelSelect
{
    public class LevelSelectView : MonoBehaviour
    {
        public event Action<LevelModel> Selected;

        [SerializeField] private LevelItem _levelItem;
        [SerializeField] private RectTransform _levelContent;
        [SerializeField] private RectTransform _levelSplit;

        private List<LevelItem> _items;

        private void Awake()
        {
            _levelItem.gameObject.SetActive(false);
            _levelSplit.gameObject.SetActive(false);
        }

        public void SetLevels(LevelModel[] levels, LevelSO[] levelsSO)
        {
            _levelItem.gameObject.SetActive(true);
            _levelSplit.gameObject.SetActive(true);

            for (int i = 0; i < _levelContent.childCount; i++)
            {
                var levelContentChild = _levelContent.GetChild(i);
                if (levelContentChild == _levelItem.transform
                    || levelContentChild == _levelSplit)
                    continue;
                levelContentChild.DestroyGameObject();
            }
            _items = new List<LevelItem>(levels.Length);

            for (int i = 0; i < levels.Length; i++)
            {
                var level = levelsSO[i];

                var levelItem = Instantiate(_levelItem, _levelContent);
                levelItem.Init(levels[i], level);

                levelItem.Clicked += OnLevelItemClicked;
                _items.Add(levelItem);

                if (i == levels.Length - 1)
                    continue;

                Instantiate(_levelSplit, _levelContent);
            }
            _levelItem.gameObject.SetActive(false);
            _levelSplit.gameObject.SetActive(false);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnLevelItemClicked(LevelModel level)
        {
            Selected?.Invoke(level);
        }

        private void OnDestroy()
        {
            Selected = null;

            foreach (var item in _items)
            {
                item.Clicked -= OnLevelItemClicked;
            }
        }
    }
}