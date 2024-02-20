using System;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.Presentation.UI.LoadingPopup
{
    public class LoadingScreenUI : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;
        private readonly int _total = 100;

        private void Start()
        {
            StartLoading();
        }

        public void StartLoading()
        {
            IProgress<int> progress = new Progress<int>((progressValue) =>
            {
                float percent = (float)progressValue / _total;
                SetProgress(percent);
            });

            // �������� �������� ������
            LoadData(progress);
        }

        private void LoadData(IProgress<int> progress)
        {
            // ������ �������� ������ � ����������� ���������
            for (int i = 0; i <= 100; i++)
            {
                progress.Report(i); // �������� � ��������� ��������
            }
        }

        private void SetProgress(float percent)
        {
            if (_progressBar != null)
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