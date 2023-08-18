using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Graph = LineWars.Model.Graph;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speedDampening = 15;
    [SerializeField] private float additionalPaddingFromBorders = 2f;
    [SerializeField] private float zoomDampening = 6f;
    [SerializeField] private float zoomStepSize = 2f;
    [SerializeField] private float maxHeight = 10f;
    [SerializeField] private float minHeight = 3f;

    private Camera mainCamera;
    private Transform cameraTransform;

    private Vector2 horizontalVelocity;
    private Vector2 pivotPoint;
    private Vector3 lastPosition;
    private bool isDragging;
    private float zoomValue;
    
    private Vector2 maxLimit;
    private Vector2 minLimit;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            cameraTransform = mainCamera.GetComponent<Transform>();
            zoomValue = mainCamera.orthographicSize;
        }
    }

    private void Start()
    {
        if (Graph.AllNodes.Count != 0)
            (minLimit, maxLimit) = GetMinAndMaxGraphPoints();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touches.Any(touch => touch.phase == TouchPhase.Began))
        {
            pivotPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }
        
        if (Input.GetMouseButton(0) && isDragging)
        {
            if (Input.touches.Any(touch => touch.phase == TouchPhase.Ended))
                pivotPoint = mainCamera.ScreenToWorldPoint(GetMidpointBetweenTouches());
            else
            {
                var currentPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                DragCamera(currentPosition);
            }
        }
        else
            isDragging = false;

        MouseZoom();
        TouchZoom();

        UpdateVelocity();
        UpdateOrthographicSize();
    }

    private Vector2 GetMidpointBetweenTouches()
    {
        var activeTouches =
            Input.touches.Where(touch => touch.phase != TouchPhase.Canceled && touch.phase != TouchPhase.Ended)
                .Select(touch => touch.position).ToArray();
        return activeTouches.Aggregate((touch1, touch2) => touch1 + touch2) / activeTouches.Count();
    }

    private void DragCamera(Vector2 position)
    {
        var resultPosition = cameraTransform.position + (Vector3)(position - pivotPoint) * -1;
        cameraTransform.position = VectorClamp(resultPosition, minLimit, maxLimit);
    }

    private void UpdateVelocity()
    {
        if (isDragging)
        {
            var cameraPosition = cameraTransform.position;
            horizontalVelocity = (cameraPosition - lastPosition) / Time.deltaTime;
            lastPosition = cameraPosition;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, speedDampening * Time.deltaTime);
            var resultPosition = cameraTransform.position + (Vector3)horizontalVelocity * Time.deltaTime;
            cameraTransform.position = VectorClamp(resultPosition, minLimit, maxLimit);
        }
    }

    private void UpdateOrthographicSize()
    {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomValue,
            zoomDampening * Time.deltaTime);
    }

    private Vector3 VectorClamp(Vector3 value, Vector3 min, Vector3 max)
    {
        return new Vector3(
            Mathf.Clamp(value.x, min.x, max.x),
            Mathf.Clamp(value.y, min.y, max.y),
            value.z);
    }

    private void MouseZoom()
    {
        var inputValue = Input.mouseScrollDelta.y;
        if (Mathf.Abs(inputValue) > 0f)
            zoomValue = Mathf.Clamp(mainCamera.orthographicSize - (inputValue * zoomStepSize), minHeight,
                maxHeight);
    }
    
    private void TouchZoom()
    {
        if (Input.touchCount > 1)
        {
            var touch0 = Input.GetTouch(0);
            var touch1 = Input.GetTouch(1);
            var previousMagnitude =
                ((touch0.position - touch0.deltaPosition) - (touch1.position - touch1.deltaPosition)).magnitude;
            var currentMagnitude = (touch0.position - touch1.position).magnitude;

            var difference = currentMagnitude - previousMagnitude;

            zoomValue = Mathf.Clamp(mainCamera.orthographicSize - (difference * 0.1f), minHeight, maxHeight);
        }
    }

    private (Vector2, Vector2) GetMinAndMaxGraphPoints()
    {
        var maxPoint = Vector2.negativeInfinity;
        var minPoint = Vector2.positiveInfinity;
        
        var allNodes = Graph.AllNodes;
        foreach (var node in allNodes)
        {
            var position = node.Position;
            maxPoint.x = position.x > maxPoint.x ? position.x : maxPoint.x;
            maxPoint.y = position.y > maxPoint.y ? position.y : maxPoint.y;
            minPoint.x = position.x < minPoint.x ? position.x : minPoint.x;
            minPoint.y = position.y < minPoint.y ? position.y : minPoint.y;
        }

        maxPoint += additionalPaddingFromBorders * Vector2.one;
        minPoint -= additionalPaddingFromBorders * Vector2.one;
        
        
        Debug.Log($"Min: {minPoint} || Max: {maxPoint}");
        return (minPoint, maxPoint);
    }
}