using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicParticleController : MonoBehaviour
{
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
