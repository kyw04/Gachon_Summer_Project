using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnParticleSystemStopped()
    {
        Destroy(this.transform.root.gameObject);
    }
}
