using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise9Async : MonoBehaviour
    {
        private string _text = "The timer went off";
        private float _time = 10f;
        private int _timer = 10;

        private void Start()
        {
            //await StartTimerUniTask();
            //await StartTimerTask();

            //StartCoroutine(StartTimerCoroutine());

            //StartTimer();

            Timer();
        }

        private void Timer()
        {
            TimerCallback timerCallback = new TimerCallback(Count);
            var timer = new Timer(timerCallback, 0, 10, 1000);

            void Count(object obj)
            {
                int x = (int)obj;

                for (int i = x; i > 0; i--)
                {
                    Debug.Log(i);
                }
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