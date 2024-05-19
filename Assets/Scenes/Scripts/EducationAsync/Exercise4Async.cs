using Cysharp.Threading.Tasks;
using System.Collections;
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

            await ExerciseForUniTask(cancellationToken);
            await ExerciseForTask(cancellationToken);

            var coroutine = StartCoroutine(ExerciseForCoroutine());

            _cancelButton.onClick.AddListener(() =>
            {
                _cancellationTokenSource?.Cancel();
                StopCoroutine(coroutine);
                Debug.Log("Cancel button clicked");
            });
        }

        private IEnumerator ExerciseForCoroutine()
        {
            var timeCount = 10;

            Debug.Log("Hello, Async!");

            yield return new WaitForSeconds(timeCount);
            Debug.Log("Bye, bye");
        }

        private async UniTask ExerciseForUniTask(CancellationToken cancellationToken)
        {
            Debug.Log("Hello, Async!");
            await UniTask.WaitForSeconds(10, cancellationToken: cancellationToken);
            Debug.Log("Bye, bye");
        }

        private async Task ExerciseForTask(CancellationToken cancellationToken)
        {
            Debug.Log("Hello, Async!");
            await Task.Delay(10000, cancellationToken: cancellationToken);
            Debug.Log("Bye, bye");
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Dispose();

            _cancelButton.onClick.RemoveAllListeners();
        }
    }
}