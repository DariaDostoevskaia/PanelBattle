using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise7Async : MonoBehaviour
    {
        private async void Start()
        {
            await PrintElementAsync();
        }

        private async UniTask PrintElementAsync()
        {
            var worldsList = new List<string>()
            {
                "Bob",
                "Menson",
                "Merline",
                "Anna",
                "Paul",
                "Ron"
            };

            foreach (var world in worldsList)
            {
                await UniTask.WaitForSeconds(2);
                Debug.Log(world);
            }
        }
    }
}