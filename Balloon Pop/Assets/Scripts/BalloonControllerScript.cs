using UnityEngine;
using System.Collections;

namespace AuroraEndeavors.SharedComponents
{
    public class BalloonControllerScript : MonoBehaviour
    {


        public float balloonVerticalSpeed;
        public float balloonHorizontalSpeed;
        public float maxHorizontalFloatAmount;
        public float rotationSpeed;
        public float maxRotationAngle;
        /// <summary>
        /// The max randomization percentage. 0-100
        /// </summary>
        public float maxRandomization;

        private bool m_isViewColliding = false;

        // Use this for initialization
        void Start()
        {
            ApplyRandomization();
        }

        void ApplyRandomization()
        {
            balloonVerticalSpeed = PropertyReductionAmount(balloonVerticalSpeed);
            balloonHorizontalSpeed = PropertyReductionAmount(balloonHorizontalSpeed);
            maxHorizontalFloatAmount = PropertyReductionAmount(maxHorizontalFloatAmount);
            rotationSpeed = PropertyReductionAmount(rotationSpeed);
            maxRotationAngle = PropertyReductionAmount(maxRotationAngle);
        }

        float PropertyReductionAmount(float propertyValue)
        {
            //percentage entered as 0-100
            float randomValue = CUtilities.GetRandom(0, maxRandomization / 100);
            float reducedValue = 1.0f - randomValue;
            return propertyValue * reducedValue;

        }

        // Update is called once per frame
        void FixedUpdate()
        {

            //TODO Q - movement very rigid, needs review??
            SwapHorizontalDirection();
            rigidbody2D.velocity = new Vector2(balloonHorizontalSpeed, balloonVerticalSpeed);

            //Apply rotation to balloon
            ReverseRotation();
            rigidbody2D.rotation += rotationSpeed;

        }

        void ReverseRotation()
        {
            if (Mathf.Abs(rigidbody2D.rotation) > maxRotationAngle)
                rotationSpeed = -rotationSpeed;
        }

        void SwapHorizontalDirection()
        { SwapHorizontalDirection(false); }
        void SwapHorizontalDirection(bool ForceSwap)
        {
            if (Mathf.Abs(this.transform.position.x) > maxHorizontalFloatAmount)
                balloonHorizontalSpeed = -balloonHorizontalSpeed;
        }

        void OnPopped()
        {
            Destroy(gameObject);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!m_isViewColliding && collision.gameObject.name == "BalloonComponent")
                maxHorizontalFloatAmount = Mathf.Abs(this.transform.position.x) - .05f;
        }

    }
}