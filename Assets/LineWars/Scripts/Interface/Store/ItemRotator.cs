using System;
using UnityEngine;

namespace LineWars.Interface
{
    public class ItemRotator : MonoBehaviour
    {
        [SerializeField] private RectTransform rotatable;
        [SerializeField] private float rotateSpeedDegrees;
        private void FixedUpdate()
        {
            rotatable.transform.Rotate(Vector3.back, rotateSpeedDegrees);
            if (rotatable.transform.rotation.z <= -360)
            {
                rotatable.transform.Rotate(Vector3.forward, 360);
            }
        }
    }
}