using UnityEngine;

namespace LineWars.Model
{
    public class SteppingAnimation : UnitStoppingAnimation
    {
        [Header("Sprite Settings")]
        [SerializeField] private SpriteRenderer leftSprite;
        [SerializeField] private SpriteRenderer rightSprite;
        [Header("Rotation Settings")]
        [SerializeField] private float rotationAngle;
        [SerializeField] private float rotationSpeed;

        private SpriteRenderer mainSprite;

        private bool isMoving;

        private bool rotatingLeft;

        public override void Execute(AnimationContext context)
        {
            isMoving = true;
            StartAnimation();
            SetMainSprite();
        }

        public override void Stop()
        {
            var x = mainSprite.transform.rotation.x;
            var y = mainSprite.transform.rotation.y;
            mainSprite.transform.rotation = Quaternion.Euler(x, y, 0);
            isMoving = false;
            EndAnimation();
        }

        private void Update()
        {
            if (!isMoving) return;
            Rotate();
        }

        private void SetMainSprite()
        {
            if (ownerUnit.UnitDirection == UnitDirection.Left)
                mainSprite = leftSprite;
            else
                mainSprite = rightSprite;
        }

        private void Rotate()
        {
            if (rotatingLeft)
                CheckLeft();
            else
                CheckRight();

            if (rotatingLeft)
                RotateLeft();
            else
                RotateRight();
        }

        private void CheckLeft()
        {
            var rotation = ConvertRotation(mainSprite.transform.rotation.eulerAngles.z);
            if (rotation > rotationAngle)
            {
                rotatingLeft = false;
            }
        }

        private void CheckRight()
        {
            var rotation = ConvertRotation(mainSprite.transform.rotation.eulerAngles.z);
            if (rotation < -rotationAngle)
            {
                rotatingLeft = true;
            }
        }

        private void RotateLeft()
        {
            mainSprite.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        private void RotateRight()
        {
            mainSprite.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }

        private float ConvertRotation(float value)
        {
            if (value <= 180f)
                return value;
            return value - 360;
        }
    }
}
