using Cysharp.Threading.Tasks;
using System;
using System.Collections;
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
            Func<Task<int[]>> funcTask = () => GetNumbersAsync();
            Func<UniTask<string[]>> funcUnitask = () => GetWorldsAsync();

            var array = Array.Empty<int>();
            var coroutine = PrintWorldsForCoroutineAsync((numbers) =>
            {
                array = numbers;
                Debug.Log(string.Join(",", array));
            });

            var worlds = await funcUnitask.Invoke();
            var numbers = await funcTask.Invoke();
            StartCoroutine(coroutine);

            Debug.Log(string.Join(",", worlds));
            Debug.Log(string.Join(",", numbers));
        }

        private async Task<int[]> GetNumbersAsync()
        {
            foreach (var number in _numbers)
            {
                Debug.Log(number + " ");
            }
            await Task.Delay(2000);
            return _numbers;
        }

        private async UniTask<string[]> GetWorldsAsync()
        {
            foreach (var world in _worlds)
            {
                Debug.Log(world + " ");
            }
            await UniTask.WaitForSeconds(1);
            return _worlds;
        }

        private IEnumerator PrintWorldsForCoroutineAsync(Action<int[]> callback)
        {
            yield return new WaitForSeconds(2);

            foreach (var number in _numbers)
            {
                Debug.Log(number + " ");
            }
            callback?.Invoke(_numbers);
        }

        //private IEnumerator PrintWorldsForCoroutineAsync(Action<string[]> callback)
        //{
        //    yield return new WaitForSeconds(2);

        //    foreach (var number in _numbers)
        //    {
        //        Debug.Log(number + " ");
        //    }
        //    callback?.Invoke(_numbers);
        //}
    }
}