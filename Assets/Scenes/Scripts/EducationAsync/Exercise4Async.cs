using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise4Async : MonoBehaviour
    {
        [SerializeField] private Button _cancelButton;

        private CancellationTokenSource _cancellationTokenSource;

        private async void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            Action action = _cancellationTokenSource.Cancel;
            //cancellationToken.Register(action);

            _cancelButton.onClick.AddListener(() =>
            {
                _cancellationTokenSource?.Cancel();
                Debug.Log("Cancel button clicked");
            });

            await ExerciseForUniTask(cancellationToken);
            await ExerciseForTask(cancellationToken);
        }

        private IEnumerator<WaitForSeconds> ExerciseForCoroutine(CancellationToken cancellationToken)
        {
            var timeCount = 10;

            //if (cancellationToken.Register()) ?????

            if (!cancellationToken.IsCancellationRequested) // переделать на .регистр
            {
                Debug.Log("Hello, Async!");
            }
            yield return new WaitForSeconds(timeCount);
        }

        private async UniTask ExerciseForUniTask(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.WaitForSeconds(10);
                Debug.Log("Hello, Async!");
                return;
            }
        }

        private async Task ExerciseForTask(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(10000);
                Debug.Log("Hello, Async!");
                return;
            }
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Dispose();

            _cancelButton.onClick.RemoveAllListeners();
        }
    }
}