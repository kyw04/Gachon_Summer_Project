using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(Hook))]
public class Rope : MonoBehaviour
{
    public AnimationCurve ropeAffectCurve;
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;

    private Hook hook;
    private LineRenderer rope;
    [SerializeField]
    private Spring spring;
    private Vector3 currentHookPosition;

    private void Start()
    {
        hook = GetComponent<Hook>();
        rope = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.target = 0f;
    }
    private void LateUpdate()
    {
        DrawRope();
    }

    private void DrawRope()
    {
        if (!hook.IsUse())
        {
            currentHookPosition = hook.firePosition.position;
            spring.Reset();
            if (rope.positionCount > 0)
                rope.positionCount = 0;

            return;
        }

        if (rope.positionCount == 0)
        {
            spring.velocity = velocity;
            rope.positionCount = quality + 1;
        }

        spring.damper = damper;
        spring.strength = strength;
        spring.UpdateSpring(Time.deltaTime);

        Vector3 hookedPoint = hook.hookHead.position;
        Vector3 up = Vector3.up;
        if (hookedPoint - hook.firePosition.position != Vector3.zero)
        {
            up = Quaternion.LookRotation((hookedPoint - hook.firePosition.position).normalized) * Vector3.up;
        }
        currentHookPosition = Vector3.Lerp(currentHookPosition, hookedPoint, Time.deltaTime * 12f);

        for (int i = 0; i < quality + 1; i++)
        {
            float delta = i / (float)quality;
            Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.value * ropeAffectCurve.Evaluate(delta);

            rope.SetPosition(i, Vector3.Lerp(hook.firePosition.position, currentHookPosition, delta) + offset);
        }
    }
}