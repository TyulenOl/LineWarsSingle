using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public class DebugCameraController : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Camera thisCamera;
        private Vector2 moveDirection;

        private void Start()
        {
            thisCamera = Camera.main;
        }

        private void Update()
        {
            moveDirection = Vector2.zero;
            if(Input.GetKey(KeyCode.W) )
            {
                moveDirection += Vector2.up;
            }
            if(Input.GetKey(KeyCode.S))
            {
                moveDirection += Vector2.down;
            }
            if(Input.GetKey(KeyCode.A))
            {
                moveDirection += Vector2.left;
            }
            if(Input.GetKey(KeyCode.D))
            {
                moveDirection += Vector2.right;
            }
            moveDirection = moveDirection.normalized;
        }

        private void FixedUpdate()
        {
            thisCamera.transform.Translate(moveDirection * speed * Time.fixedDeltaTime);
        }
    }
}
