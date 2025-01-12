using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCollision : MonoBehaviour
{
    public event Action WinState;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
