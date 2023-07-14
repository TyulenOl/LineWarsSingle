using System;
using UnityEngine;

namespace LineWars.Interface
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LineDrawer : MonoBehaviour
    {
        [SerializeField] [HideInInspector] private SpriteRenderer lineSpriteRenderer;
        [SerializeField] [HideInInspector] private Transform firstTransform;
        [SerializeField] [HideInInspector] private Transform secondTransform;
        public SpriteRenderer LineSpriteRenderer => lineSpriteRenderer;
        
        public void Initialise(Transform first, Transform second)
        {
            lineSpriteRenderer = GetComponent<SpriteRenderer>();
            firstTransform = first;
            secondTransform = second;
            DrawLine();
        }
        
        public void DrawLine()
        {
            var positionFirst = firstTransform.position;
            var positionSecond = secondTransform.position;
            var distance = Vector3.Distance(positionFirst, positionSecond);
            lineSpriteRenderer.size = new Vector2(distance,lineSpriteRenderer.size.y);
            var center = positionFirst;
            var newSecondNodePosition = positionSecond - center;
            var radian = Mathf.Atan2(newSecondNodePosition.y , newSecondNodePosition.x) * 180 / Math.PI;
            lineSpriteRenderer.transform.rotation = Quaternion.Euler(0,0,(float)radian);
            lineSpriteRenderer.transform.position = (positionFirst + positionSecond) / 2;
        }
    }
}
