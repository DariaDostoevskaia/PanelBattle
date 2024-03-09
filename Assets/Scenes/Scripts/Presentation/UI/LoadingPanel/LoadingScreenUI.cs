using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.LoadingPopup
{
    public class LoadingScreenUI : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;

        public void SetProgress(float percent)
        {
            _progressBar.value = percent;
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