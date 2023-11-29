using System;
using UnityEngine;


public class SimpleBackFordAnimation : MonoBehaviour
{
    [SerializeField, Min(0)] private float backSpeed;
    [SerializeField, Min(0)] private float forewordSpeed;
    [SerializeField, Min(0)] private float maxOffset;

    private float currentSpeed;
    private float currentOffset;

    private void Awake()
    {
        currentSpeed = forewordSpeed;
    }

    private void FixedUpdate()
    {
        var newOffset = Mathf.Clamp(currentOffset + currentSpeed * Time.deltaTime, 0, maxOffset);
        var offsetDelta = currentOffset - newOffset;
        currentOffset = newOffset;
        var direction = transform.rotation * Vector3.right;
        transform.position += direction * offsetDelta;
        if (currentOffset <= 0)
            currentSpeed = forewordSpeed;
        else if (currentOffset >= maxOffset)
            currentSpeed = -backSpeed;
    }
}