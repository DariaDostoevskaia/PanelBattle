using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise5Async : MonoBehaviour
    {
        private readonly int[] _numbers = new int[3] { 2, 45, 76 };
        private readonly string[] _worlds = new string[3] { "Maximum", "Slider", "Brows" };

        private async void Start()
        {
            var task = PrintNumbersAsync();
            var unitask = PrintWorldsAsync();
            var coroutine = PrintWorldsForCoroutineAsync();

            await task;
            await unitask;
            StartCoroutine(coroutine);
        }

        private async Task PrintNumbersAsync()
        {
            foreach (var number in _numbers)
            {
                Debug.Log(number + " ");
            }
            await Task.Delay(2000);
        }

        private async UniTask PrintWorldsAsync()
        {
            foreach (var world in _worlds)
            {
                Debug.Log(world + " ");
            }
            await UniTask.WaitForSeconds(2);
        }

        private IEnumerator<WaitForSeconds> PrintWorldsForCoroutineAsync()
        {
            foreach (var number in _numbers)
            {
                Debug.Log(number + " ");
            }

            yield return new WaitForSeconds(2);
        }
    }
}