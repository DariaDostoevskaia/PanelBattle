using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise7Async : MonoBehaviour
    {
        private List<string> _worldsList = new List<string>()
        {
            "Bob",
            "Menson",
            "Merline",
            "Anna",
            "Paul",
            "Ron"
        };

        private async void Start()
        {
            await PrintElementForUniTaskAsync();
            await PrintElementForTaskAsync();

            StartCoroutine(PrintElementForCoroutine());
        }

        private IEnumerator<WaitForSeconds> PrintElementForCoroutine()
        {
            foreach (var world in _worldsList)
            {
                new WaitForSeconds(2);
                Debug.Log(world);
            }
            yield return new WaitForSeconds(0);
        }

        private async Task PrintElementForTaskAsync()
        {
            foreach (var world in _worldsList)
            {
                await Task.Delay(2000);
                Debug.Log(world);
            }
        }

        private async UniTask PrintElementForUniTaskAsync()
        {
            foreach (var world in _worldsList)
            {
                await UniTask.WaitForSeconds(2);
                Debug.Log(world);
            }
        }
    }
}