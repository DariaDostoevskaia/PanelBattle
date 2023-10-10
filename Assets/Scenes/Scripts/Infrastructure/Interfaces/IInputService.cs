using System;
using UnityEngine;

namespace LegoBattaleRoyal.Infrastructure.Interfaces
{
    public interface IInputService : IUpdate
    {
        event Action<Vector3> OnClicked;
    }
}