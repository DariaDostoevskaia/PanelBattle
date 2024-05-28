using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise9Async : MonoBehaviour
    {
        [SerializeField] private float _time = 5;
        [SerializeField] private TextMeshProUGUI _timerText;

        private readonly string _text = "The timer went off";
        private int _timer = 10;

        private async void Start()
        {
            await StartTimerUniTask();
            await StartTimerTask();

            StartCoroutine(StartTimerCoroutine());

            StartTimer();
        }

        private void Update()
        {
            if (_time > 0)
            {
                _time -= Time.deltaTime;
                var result = Mathf.Round(_time * 100) / 100.0;
                _timerText.SetText($"{result}");
            }
        }

        private void StartTimer()
        {
            Debug.Log("Timer is : " + _timer);
            _timer--;
            Invoke(nameof(StartTimer), 1);

            if (_timer < 0)
                StopTimer();
        }

        private void StopTimer()
        {
            CancelInvoke(nameof(StartTimer));
        }

        private IEnumerator StartTimerCoroutine()
        {
            var seconds = 6;
            for (int i = 1; i <= seconds; i++)
            {
                yield return new WaitForSeconds(1);
                Debug.Log(i);
            }

            Debug.Log(_text);
        }

        private async UniTask StartTimerUniTask()
        {
            var seconds = 6;

            for (int i = 1; i <= seconds; i++)
            {
                await UniTask.WaitForSeconds(1);
                Debug.Log(i);
            }

            Debug.Log(_text);
        }

        private async Task StartTimerTask()
        {
            var seconds = 6;

            for (int i = 1; i <= seconds; i++)
            {
                await Task.Delay(1000);
                Debug.Log(i);
            }

            Debug.Log(_text);
        }
    }
}