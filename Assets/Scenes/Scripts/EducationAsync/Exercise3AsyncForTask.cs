using System.Threading.Tasks;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise3AsyncForTask : MonoBehaviour
    {
        private async void Start()
        {
            await CombineAndDisplayResultsAsync();
        }

        public async Task<string> GetWorldAsync()
        {
            await Task.Delay(1000);
            return "Task";
        }

        public async Task<int> GetNumberAsync()
        {
            await Task.Delay(1000);
            return 42;
        }

        public async Task CombineAndDisplayResultsAsync()
        {
            Task<string> worldTask = GetWorldAsync();
            Task<int> numberTask = GetNumberAsync();

            await Task.WhenAll(worldTask, numberTask);

            string combinedResult = $"{worldTask.Result} : {numberTask.Result}";

            Debug.Log(combinedResult);
        }
    }
}