using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise9Async : MonoBehaviour
    {
        private Timer _timer;

        private string _text = "The timer went off";
        private float _time = 10f;
        private int _timeForTimer = 10;

        private async void Start()
        {
            await StartTimerUniTask();
            await StartTimerTask();

            StartCoroutine(StartTimerCoroutine());

            StartTimer();

            _timer = new Timer(TimerCallback, null, 0, 1000);
        }

        private void TimerCallback(object state)
        {
            Debug.Log(_timeForTimer);
            _timeForTimer--;

            if (_timeForTimer < 0)
                _timer.Dispose();
        }

        private void StartTimer()
        {
            Debug.Log("Timer is : " + _timeForTimer);
            _timeForTimer--;
            Invoke(nameof(StartTimer), 1);

            if (_timeForTimer < 0)
                StopTimer();
        }

        private void StopTimer()
        {
            CancelInvoke(nameof(StartTimer));
        }

        private IEnumerator StartTimerCoroutine()
        {
            var seconds = 6;
            var wait = new WaitForSeconds(1);

            for (int i = 1; i <= seconds; i++)
            {
                yield return wait;
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