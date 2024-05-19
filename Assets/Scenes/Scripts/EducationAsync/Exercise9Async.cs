using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise9Async : MonoBehaviour
    {
        private readonly string _text = "The timer went off";

        private async void Start()
        {
            await StartTimerUniTask();
            await StartTimerTask();

            StartCoroutine(StartTimerCoroutine(8));

            //Timer ++
            //Update ++
        }

        private IEnumerator<WaitForSeconds> StartTimerCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Debug.Log(_text);
        }

        private async UniTask StartTimerUniTask()
        {
            await UniTask.Delay(3000);

            Debug.Log(_text);
        }

        private async Task StartTimerTask()
        {
            await UniTask.Delay(7000);

            Debug.Log(_text);
        }
    }
}