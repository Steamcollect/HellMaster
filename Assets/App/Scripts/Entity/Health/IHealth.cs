using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public void TakeHealth(int health);
    public void TakeDamage(float damage, Action onDeath);
}
