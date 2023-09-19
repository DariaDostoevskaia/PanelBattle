using LegoBattaleRoyal.Presentation.CapturePath;
using System;
using UnityEngine;

namespace LegoBattaleRoyal.Controllers.CapturePath
{
    public class CapturePathController
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

        public void ResetPath(Guid Id)
        {
            _capturePathView.gameObject.SetActive(false);
            _capturePathView.Clear();
        }

        public void UnBind()
        {
            _capturePathView.UnBind();
        }
    }
}