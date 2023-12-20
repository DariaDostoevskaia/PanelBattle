using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.Ads
{
    public class AdsPanelUI : MonoBehaviour
    {
        public event Action OnPlayAdsClicked;

        [SerializeField] private Button _adsButton;

        private void Start()
        {
            _adsButton.onClick.AddListener(() => OnPlayAdsClicked?.Invoke());
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}