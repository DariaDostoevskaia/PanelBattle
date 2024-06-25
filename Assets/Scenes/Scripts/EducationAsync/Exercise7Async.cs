using Cysharp.Threading.Tasks;
using System.Collections;
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

        private IEnumerator PrintElementForCoroutine()
        {
            var wait = new WaitForSeconds(1);
            for (int i = 0; i < _worldsList.Count; i++)
            {
                yield return wait;
                Debug.Log(_worldsList[i]);
            }
        }

        private async Task PrintElementForTaskAsync()
        {
            for (int i = 0; i < _worldsList.Count; i++)
            {
                await Task.Delay(2000);
                Debug.Log(_worldsList[i]);
            }
        }

        private async UniTask PrintElementForUniTaskAsync()
        {
            for (int i = 0; i < _worldsList.Count; i++)
            {
                await UniTask.WaitForSeconds(2);
                Debug.Log(_worldsList[i]);
            }
        }
    }
}