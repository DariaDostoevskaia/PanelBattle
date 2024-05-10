using System.Threading.Tasks;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise5Async : MonoBehaviour
    {
        private readonly int[] _numbers = new int[5] { 2, 45, 76, 23, 44 };
        private readonly string[] _worlds = new string[3] { "Maximum", "Slider", "Brows" };

        private async void Start()
        {
            await Task.WhenAll(PrintNumbersAsync(), PrintWorldsAsync());
        }

        private async Task PrintNumbersAsync()
        {
            await Task.Delay(1000);

            foreach (var number in _numbers)
            {
                Debug.Log(number + " ");
            }
        }

        private async Task PrintWorldsAsync()
        {
            await Task.Delay(2000);

            foreach (var world in _worlds)
            {
                Debug.Log(world + " ");
            }
        }
    }
}