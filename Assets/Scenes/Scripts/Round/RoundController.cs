using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController
{
    public event Action OnRoundChanged;

    public void ChangeRound()
    {
        OnRoundChanged?.Invoke();
        //dispose
    }
}