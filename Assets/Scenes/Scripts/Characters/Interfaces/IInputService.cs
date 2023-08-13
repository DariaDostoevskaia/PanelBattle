using System;
using UnityEngine;

namespace LegoBattaleRoyal.Characters.Interfaces
{
    public interface IInputService : IUpdate
    {
        event Action<Vector3> OnClicked;
    }
}