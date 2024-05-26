using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise9Async : MonoBehaviour
    {
        private readonly string _text = "The timer went off";

        private float _timerInterval = 5f;
        private bool _useTimer = true;

        private async void Start()
        {
            await StartTimerUniTask();
            await StartTimerTask();

            StartCoroutine(StartTimerCoroutine());

            //Timer ++
            //Update ++

            //TimerExample();
        }

        private void TimerExample()
        {
            if (_useTimer)
            {
                var timer = new Timer(_timerInterval * 1000);

                timer.Elapsed += OnTimerElapsed;
                timer.AutoReset = true;
                timer.Enabled = true;
            }
        }

        private static void OnTimerElapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            Debug.Log("Timer elapsed!");
        }

        //private void Update()
        //{
        //    if (!_useTimer)
        //    {
        //        _timerInterval -= Time.deltaTime;

        //        if (_timerInterval <= 0f)
        //        {
        //            OnTimerElapsed(null, null);
        //            _timerInterval = 5f;
        //            // —брос таймера
        //        }
        //    }
        //}

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