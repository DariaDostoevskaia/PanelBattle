using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.LoadingPopup
{
    public class LoadingScreenUI : MonoBehaviour
    {
        [SerializeField] private Slider _loadingScreen;

        private void Start()
        {
            LoadData();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void LoadData()
        {
            var progress = 0f;

            while (progress < 1f)
            {
                progress += Time.deltaTime / 2; // Пример увеличения прогресса загрузки
                _loadingScreen.value = progress; // Обновить состояние слайдера на загрузочном экране
            }
        }
    }
}