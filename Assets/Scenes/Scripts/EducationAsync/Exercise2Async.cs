using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise2Async : MonoBehaviour
    {
        [SerializeField] private string _imageUrl = "https://dummyimage.com/500";
        [SerializeField] private Image _texture;

        private async void Start()
        {
            StartCoroutine(LoadTextureWithCoroutine());

            LoadTextureWithUniTask().Forget();
            await LoadTextureWithTask();
        }

        private void SetTextureOnObject(Texture2D texture)
        {
            _texture.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
            _texture.SetNativeSize();
        }

        private UnityWebRequest GetTextureUrl()
        {
            var www = UnityWebRequestTexture.GetTexture(_imageUrl);

            if (www == null)
                Exercise6ReportExeptions();

            return www;
        }

        private Texture2D GetContent(UnityWebRequest request)
        {
            return DownloadHandlerTexture.GetContent(request);
        }

        private void Exercise6ReportExeptions()
        {
            throw new ArgumentOutOfRangeException(nameof(GetTextureUrl));
        }

        public async UniTask LoadTextureWithUniTask()
        {
            var request = GetTextureUrl();
            await request.SendWebRequest();

            var texture = GetContent(request);
            SetTextureOnObject(texture);
        }

        public async Task LoadTextureWithTask()
        {
            var request = GetTextureUrl();
            await request.SendWebRequest();

            var texture = GetContent(request);
            SetTextureOnObject(texture);
        }

        public IEnumerator LoadTextureWithCoroutine()
        {
            var request = GetTextureUrl();
            yield return request.SendWebRequest();

            var texture = GetContent(request);
            SetTextureOnObject(texture);
        }
    }
}