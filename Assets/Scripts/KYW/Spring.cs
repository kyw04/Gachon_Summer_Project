using System;
using UnityEngine;

[Serializable]
public class Spring
{
    [SerializeField]
    public float strength;
    [SerializeField]
    public float damper;
    [SerializeField]
    public float target;
    [SerializeField]
    public float velocity;
    [SerializeField]
    public float value;

    public Spring()
    {
        Reset();
    }

    public void Reset()
    {
        velocity = 0f;
        value = 0f;
    }

    public void UpdateSpring(float deltaTime)
    {
        float direction = target - value >= 0 ? 1f : -1f;
        float force = Mathf.Abs(target - value) * strength;
        velocity += (force * direction - velocity * damper) * deltaTime;
        value += velocity * deltaTime;
    }
}
