using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace LegoBattaleRoyal.EducationAsync
{
    public class Exercise2Async : MonoBehaviour
    {
        [SerializeField] private string _imageUrl = "https://dummyimage.com/500";
        [SerializeField] private GameObject _texture;

        private Renderer _renderer;

        private async void Start()
        {
            _renderer = _texture.GetComponent<Renderer>();

            StartCoroutine(LoadTextureWithCoroutine());

            LoadTextureWithUniTask().Forget();
            await LoadTextureWithTask();
        }

        private void SetTextureOnObject(Texture2D texture)
        {
            _renderer.material.mainTexture = texture;
        }

        private UnityWebRequest GetTexture()
        {
            return UnityWebRequestTexture.GetTexture(_imageUrl);
        }

        private Texture2D GetContent(UnityWebRequest request)
        {
            return DownloadHandlerTexture.GetContent(request);
        }

        public async UniTask LoadTextureWithUniTask()
        {
            var request = GetTexture();
            await request.SendWebRequest();

            var texture = GetContent(request);
            SetTextureOnObject(texture);
        }

        public async Task LoadTextureWithTask()
        {
            var request = GetTexture();
            await request.SendWebRequest();

            var texture = GetContent(request);
            SetTextureOnObject(texture);
        }

        public IEnumerator LoadTextureWithCoroutine()
        {
            var request = GetTexture();
            yield return request.SendWebRequest();

            var texture = GetContent(request);
            SetTextureOnObject(texture);
        }
    }
}