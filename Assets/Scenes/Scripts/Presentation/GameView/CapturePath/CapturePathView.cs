using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CapturePathView : MonoBehaviour
{
    [SerializeField] private MeshRenderer _dotPrefab;
    private LineRenderer _lineRenderer;
    private Transform _bindedTransform;
    private readonly List<MeshRenderer> _dots = new();

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
    }

    private void Update()
    {
        if (_bindedTransform == null)
            return;

        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _bindedTransform.position);
    }

    public void ShowCapturePath(params Vector3[] points)
    {
        if (points.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(points), points.Length, "Exepted > 0");

        var startCount = _lineRenderer.positionCount;

        _lineRenderer.positionCount += points.Length;

        for (int i = 0; i < points.Length; i++, startCount++)
        {
            CreateDot(points[i]);
            _lineRenderer.SetPosition(startCount, points[i]);
        }
    }

    public void Bind(Transform transform)
    {
        var index = _lineRenderer.positionCount;

        if (index == 0)
            ShowCapturePath(transform.position);

        index = _lineRenderer.positionCount;
        _lineRenderer.positionCount++;

        _lineRenderer.SetPosition(index, transform.position);
        _bindedTransform = transform;
    }

    public void UnBind()
    {
        CreateDot(_bindedTransform.position);
        _bindedTransform = null;
    }

    public void SetColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
        //использвать в создании игрока .SetColor();
    }

    public void Clear()
    {
        _lineRenderer.positionCount = 0;

        foreach (var dot in _dots)
        {
            Destroy(dot.gameObject);
        }
        _dots.Clear();
    }

    private void CreateDot(Vector3 vector3)
    {
        var dot = Instantiate(_dotPrefab, vector3, Quaternion.identity, transform);
        dot.gameObject.SetActive(true);
        _dots.Add(dot);
    }
}