using LegoBattaleRoyal.Characters.Interfaces;
using System;
using UnityEngine;

public class InputService : IInputService
{
    public event Action<Vector3> OnClicked;

    private readonly Camera _camera;

    public InputService()
    {
        _camera = Camera.main;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out var hit))
                OnClicked?.Invoke(hit.point);
        }
    }
}