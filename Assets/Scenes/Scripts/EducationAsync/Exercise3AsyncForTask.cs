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

        public async Task<string> GetFirstDataAsync()
        {
            await Task.Delay(1000);
            return "Task";
        }

        public async Task<int> GetSecondDataAsync()
        {
            await Task.Delay(1000);
            return 42;
        }

        public async Task CombineAndDisplayResultsAsync()
        {
            Task<string> firstDataTask = GetFirstDataAsync();
            Task<int> secondDataTask = GetSecondDataAsync();

            await Task.WhenAll(firstDataTask, secondDataTask);

            string combinedResult = $"{await firstDataTask} : {await secondDataTask}";

            Debug.Log(combinedResult);
        }
    }
}