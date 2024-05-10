using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise3Async : MonoBehaviour
    {
        [SerializeField] private int[] _numbers = new int[10] { 1, 7, 15, 28, 56, 46, 3333, 95837, 5534, 232 };
        [SerializeField] private string[] _words = { "John went to school", "He's an excellent student." };

        private async void Start()
        {
            await PrintMessage();
        }

        private async UniTask PrintMessage()
        {
            var numberTask = PrintMaxNumberAsync();
            var letterTask = PrintLetter();

            await UniTask.WhenAll(numberTask, letterTask);

            int number = await numberTask;
            string letter = await letterTask;

            string combinedResult = $"Your code password: {letter}-{number}";
            Debug.Log(combinedResult);
        }

        private async UniTask<int> PrintMaxNumberAsync()
        {
            await Task.Delay(1000);
            return GetMaxNumber();
        }

        private async UniTask<string> PrintLetter()
        {
            await UniTask.Delay(2000);
            return FindMostFrequentLetterAsync();
        }

        private int GetMaxNumber()
        {
            int max = int.MinValue;
            for (int i = 0; i < _numbers.Length; i++)
            {
                if (_numbers[i] > max)
                    max = _numbers[i];
            }
            return max;
        }

        private string FindMostFrequentLetterAsync()
        {
            Dictionary<char, int> letterCount = new();

            foreach (string word in _words)
            {
                foreach (char letter in word)
                {
                    if (char.IsLetter(letter))
                    {
                        char lowercaseLetter = char.ToLower(letter);

                        if (letterCount.ContainsKey(lowercaseLetter))
                            letterCount[lowercaseLetter]++;
                        else
                            letterCount[lowercaseLetter] = 1;
                    }
                }
            }

            int maxCount = letterCount.Values.Max();

            var mostFrequentLetters = letterCount
                .Where(pair => pair.Value == maxCount)
                .Select(pair => pair.Key);

            return string.Join(" ", mostFrequentLetters);
        }
    }
}