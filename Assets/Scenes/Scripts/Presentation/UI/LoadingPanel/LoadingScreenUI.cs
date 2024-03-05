using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.LoadingPopup
{
    public class LoadingScreenUI : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private float _seconds = 0.2f;

        public Slider ProgressBar => _progressBar;

        public float Seconds => _seconds;

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