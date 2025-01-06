using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Health
{
    public void TakeHealth(int health);
    public void TakeDamage(int damage);
}
