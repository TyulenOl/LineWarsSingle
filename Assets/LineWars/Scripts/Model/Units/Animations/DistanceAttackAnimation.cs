using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    public class DistanceAttackAnimation : UnitAnimation
    {
        [SerializeField] private GameObject shootProjectilePrefab;
        [SerializeField] private float projectileSpeed;

        private GameObject shootProjectileInstance;

        private Vector2 destination;
        public override void Execute(AnimationContext context)
        {
            var projectilePosition = CalculateInitialProjectilePosition();  
            destination = context.TargetUnit.transform.position;
            var angle = CalculateAngle(projectilePosition, destination);
            shootProjectileInstance = 
                Instantiate(shootProjectilePrefab, projectilePosition, Quaternion.Euler(0,0, angle));
            
            IsPlaying = true;
            
        }

        private void Update()
        {
            if (!IsPlaying)
                return;

            MoveProjectile();
            CheckDestination();
                
        }

        private void MoveProjectile()
        {
            shootProjectileInstance.transform.position
                = Vector2.MoveTowards(shootProjectileInstance.transform.position, destination, projectileSpeed * Time.deltaTime);
        }

        private void CheckDestination()
        {
            if ((Vector2)shootProjectileInstance.transform.position == destination)
            {
                Destroy(shootProjectileInstance);
                shootProjectileInstance = null;
                IsPlaying = false;
            }
        }

        private float CalculateAngle(Vector2 initialPosition, Vector2 destination)
        {
            var direction = (destination - initialPosition).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return angle;
        }

        private Vector2 CalculateInitialProjectilePosition()
        {
            var animHelper = ownerUnit.GetComponent<UnitAnimationHelper>();
            if (ownerUnit.UnitDirection == UnitDirection.Left)
                return animHelper.LeftCenter.transform.position;
            return animHelper.RightCenter.transform.position;
        }
    }
}
