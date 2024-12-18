using LegoBattaleRoyal.Extensions;
using LegoBattaleRoyal.Presentation.GameView.CapturePath;
using System;
using UnityEngine;

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
            _capturePathView.gameObject.SetActive(false);
            _capturePathView.Clear();
        }

        public void UnBind()
        {
            _capturePathView.UnBind();
        }

        public void Dispose()
        {
            ResetPath();
            _capturePathView.DestroyGameObject();
        }
    }
}