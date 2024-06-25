using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise3AsyncForUniTask : MonoBehaviour
    {
        private async void Start()
        {
            await CombineAndDisplayResultsAsync();
        }

        public async UniTask<string> GetFirstDataAsync()
        {
            await UniTask.WaitForSeconds(1);
            return "UniTask";
        }

        public async UniTask<int> GetSecondDataAsync()
        {
            await UniTask.WaitForSeconds(1);
            return 1111;
        }

        public async UniTask CombineAndDisplayResultsAsync()
        {
            var firstDataTask = GetFirstDataAsync();
            var secondDataTask = GetSecondDataAsync();

            var res = await UniTask.WhenAll(firstDataTask, secondDataTask);

            Debug.Log(res.ToString());
        }
    }
}