using Cysharp.Threading.Tasks;
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

        private async void Start()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            await ExerciseForUniTask(cancellationToken);
            await ExerciseForTask(cancellationToken);
            StartCoroutine(ExerciseForCoroutine(cancellationToken));

            _cancelButton.onClick.AddListener(() =>
            {
                CancelToken(cancellationTokenSource);
            });
        }

        private IEnumerator<WaitForSeconds> ExerciseForCoroutine(CancellationToken cancellationToken)
        {
            var timeCount = 10;

            if (!cancellationToken.IsCancellationRequested)
            {
                Debug.Log("Hello, Async!");
            }

            yield return new WaitForSeconds(timeCount);
        }

        private void CancelToken(CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Cancel();
            Debug.Log("Canceled");
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
            _cancelButton.onClick.RemoveAllListeners();
        }
    }
}