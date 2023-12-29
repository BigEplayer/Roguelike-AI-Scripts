using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public void HandleTrapHit();
    public void TakeHit(float damage);
    public void Die();
}
