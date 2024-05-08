using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise1Async : MonoBehaviour
    {
        private async void Start()
        {
            await ExerciseForUniTask();
            await ExerciseForTask();
            StartCoroutine(ExerciseForCoroutine(2));
        }

        private IEnumerator ExerciseForCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Debug.Log("Hello, Async!");
        }

        private async UniTask ExerciseForUniTask()
        {
            await UniTask.WaitForSeconds(2);
            Debug.Log("Hello, Async!");
        }

        private async Task ExerciseForTask()
        {
            await Task.Delay(2000);
            Debug.Log("Hello, Async!");
        }
    }
}