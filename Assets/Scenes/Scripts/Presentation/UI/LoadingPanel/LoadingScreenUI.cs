using DG.Tweening;
using LegoBattaleRoyal.Presentation.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.LoadingPopup
{
    public class LoadingScreenUI : BaseViewUI
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private float _duration;
        private Tween _tween;

        public bool IsAnimation => _tween?.IsActive() ?? false;

        public void SetProgress(float percent)
        {
            _tween?.Kill();
            var initValue = _progressBar.value;
            _tween = DOVirtual.Float(initValue, percent, _duration, (value) => _progressBar.value = value)
                .SetEase(Ease.Linear);
        }

        public void ResetProgress()
        {
            _tween?.Kill();
            _progressBar.value = 0;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _tween?.Kill();
        }
    }
}