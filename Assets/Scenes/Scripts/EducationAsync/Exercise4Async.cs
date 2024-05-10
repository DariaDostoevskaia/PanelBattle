using Cysharp.Threading.Tasks;
using System.Threading;
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

            _cancelButton.onClick.AddListener(() => CancelToken(cancellationTokenSource));
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

        private void OnDestroy()
        {
            _cancelButton.onClick.RemoveAllListeners();
        }
    }
}