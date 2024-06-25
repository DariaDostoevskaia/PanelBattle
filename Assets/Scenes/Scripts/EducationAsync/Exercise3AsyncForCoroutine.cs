using System.Collections;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise3AsyncForCoroutine : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(CombineAndDisplayResults());
        }

        private IEnumerator GetFirstDataCoroutine()
        {
            yield return new WaitForSeconds(1f);
            yield return "Coroutine";
        }

        private IEnumerator GetSecondDataCoroutine()
        {
            yield return new WaitForSeconds(1f);
            yield return 18;
        }

        private IEnumerator CombineAndDisplayResults()
        {
            var firstDataCoroutine = GetFirstDataCoroutine();
            var secondDataCoroutine = GetSecondDataCoroutine();

            while (true)
            {
                if (!firstDataCoroutine.MoveNext()
                    || !secondDataCoroutine.MoveNext())
                {
                    break;
                }

                yield return null;
            }

            string combinedResult = $"{firstDataCoroutine.Current} : {secondDataCoroutine.Current}";
            Debug.Log(combinedResult);
        }
    }
}