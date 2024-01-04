using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LineWars.Controllers
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        [Header("Move to a point options")] 
        [SerializeField] private float timeToMove = 1;
        [SerializeField] private float minHeightOnMove = 5;

        [Header("Drag options")] 
        [SerializeField] private float speedDampening = 15;

        [Header("Zoom options")] 
        [SerializeField] private float zoomDampening = 6f;
        [SerializeField] private float zoomStepSize = 2f;
        [SerializeField] private float minHeight = 3f;

        [Header("Paddings")] 
        [SerializeField] private float paddingsTop;
        [SerializeField] private float paddingsRight;
        [SerializeField] private float paddingsBottom;
        [SerializeField] private float paddingsLeft;

        private Canvas canvas;
        private Camera mainCamera;
        private Transform cameraTransform;

        private float maxHeight;
        private Vector2 horizontalVelocity;
        private Vector2 pivotPoint;
        private Vector3 lastPosition;
        private bool isDragging;

        private Vector2 mapPointMin;
        private Vector2 mapPointMax;

        private float zoomValue;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError($"Больше чем два {nameof(CameraController)} на сцене");
                Destroy(gameObject);
            }

            mainCamera = Camera.main;
            if (mainCamera != null)
                cameraTransform = mainCamera.GetComponent<Transform>();

            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }

        private void Start()
        {
            var basePosition = Player.LocalPlayer.Base.transform.position;
            cameraTransform.position = new Vector3(basePosition.x, basePosition.y, cameraTransform.position.z);
            var bounds = Map.MapSpriteRenderer.bounds;
            mapPointMax = bounds.max;
            mapPointMin = bounds.min;

            maxHeight = Mathf.Min((mapPointMax - mapPointMin).x / (2 * mainCamera.aspect),
                (mapPointMax - mapPointMin).y / 2);

            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minHeight, maxHeight);
            zoomValue = mainCamera.orthographicSize;
        }

        private void Update()
        {
            if ((Input.GetMouseButtonDown(0) || Input.touches.Any(touch => touch.phase == TouchPhase.Began)) &&
                !PointerIsOverUI())
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

            UpdateOrthographicSize();
            UpdateVelocity();
        }

        public void MoveTo(Vector2 point)
        {
            StartCoroutine(MoveToPoint(new Vector3(point.x, point.y, cameraTransform.position.z)));
        }

        private IEnumerator MoveToPoint(Vector3 targetPoint)
        {
            var startPoint = cameraTransform.position;
            float completionPercentage = 0;

            var halfCameraSize = GetCameraSize() / 2;
            var aspect = mainCamera.aspect;

            var leftBottomCorner = (Vector2)targetPoint - mapPointMin;
            var rightTopCorner = mapPointMax - leftBottomCorner;

            var minDistanceToBorder = Mathf.Min(leftBottomCorner.x / aspect, rightTopCorner.x / aspect,
                leftBottomCorner.y, rightTopCorner.y);

            var coefficient = 1 / (halfCameraSize.y / minDistanceToBorder);
            var newOrthographicSize = halfCameraSize.y * coefficient;

            if (newOrthographicSize < zoomValue)
                zoomValue = Mathf.Clamp(newOrthographicSize, minHeightOnMove, newOrthographicSize);

            while (completionPercentage < 1 && !isDragging)
            {
                completionPercentage += Time.deltaTime / timeToMove;
                var easeOutQuart = 1 - Mathf.Pow(1 - completionPercentage, 4);
                cameraTransform.position = ClampCameraPosition(Vector3.Lerp(startPoint, targetPoint, easeOutQuart));
                yield return null;
            }
        }

        private bool PointerIsOverUI()
        {
            var results = new List<RaycastResult>();
            var pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            EventSystem.current.RaycastAll(pointerData, results);

            var hitObject = results.Count < 1 ? null : results[0].gameObject;

            return hitObject != null && hitObject.layer == LayerMask.NameToLayer("UI");
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
            cameraTransform.position = ClampCameraPosition(resultPosition);
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
                cameraTransform.position = ClampCameraPosition(resultPosition);
            }
        }

        private void MouseZoom()
        {
            var inputValue = Input.mouseScrollDelta.y;
            if (Mathf.Abs(inputValue) > 0f)
                zoomValue = mainCamera.orthographicSize - inputValue * zoomStepSize;
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

                zoomValue = mainCamera.orthographicSize - difference * 0.1f;
            }
        }

        private void UpdateOrthographicSize()
        {
            zoomValue = Mathf.Clamp(zoomValue, minHeight, maxHeight);

            if (Mathf.Abs(zoomValue - mainCamera.orthographicSize) < 0.01)
                return;

            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomValue,
                zoomDampening * Time.deltaTime);
            cameraTransform.position = ClampCameraPosition(cameraTransform.position);
        }

        private Vector3 ClampCameraPosition(Vector3 position)
        {
            var maxPaddingCorner = new Vector2(FromScreenToWorld(paddingsRight), FromScreenToWorld(paddingsTop)) *
                                   canvas.scaleFactor;
            var minPaddingCorner = new Vector2(FromScreenToWorld(paddingsLeft), FromScreenToWorld(paddingsBottom)) *
                                   canvas.scaleFactor;

            var halfCameraSize = GetCameraSize() / 2;
            var maxLimitPoint = mapPointMax - halfCameraSize + maxPaddingCorner;
            var minLimitPoint = mapPointMin + halfCameraSize - minPaddingCorner;

            return new Vector3(Mathf.Clamp(position.x, minLimitPoint.x, maxLimitPoint.x),
                Mathf.Clamp(position.y, minLimitPoint.y, maxLimitPoint.y),
                cameraTransform.position.z);
        }

        private Vector2 GetCameraSize()
        {
            var height = mainCamera.orthographicSize * 2;
            var width = height * mainCamera.aspect;
            return new Vector2(width, height);
        }

        private float FromScreenToWorld(float value)
        {
            var valueInWorld = mainCamera.ScreenToWorldPoint(new Vector3(0, value)).y;
            return -(cameraTransform.position.y - mainCamera.orthographicSize - valueInWorld);
        }
    }
}