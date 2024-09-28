using LegoBattaleRoyal.Presentation.GameView.CapturePath;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LegoBattaleRoyal.Presentation.Controllers.CapturePath
{
    public class CapturePathController : IDisposable
    {
        private readonly CapturePathView _capturePathView;

        public CapturePathController(CapturePathView capturePathView)
        {
            _capturePathView = capturePathView;
        }

        public void BindTo(Transform transform)
        {
            _capturePathView.gameObject.SetActive(true);
            _capturePathView.Bind(transform);
        }

        public void ResetPath()
        {
            if (_capturePathView != null)
            {
                _capturePathView.gameObject.SetActive(false);
                _capturePathView.Clear();
            }
        }

        public void UnBind()
        {
            _capturePathView.UnBind();
        }

        public void Dispose()
        {
            ResetPath();

            if (_capturePathView != null)
                Object.Destroy(_capturePathView.gameObject);
        }
    }
}