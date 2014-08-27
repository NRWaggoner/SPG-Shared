using UnityEngine;
using System.Collections;

namespace AuroraEndeavors.SharedComponents
{
    public class CBalloonController : MonoBehaviour
    {
		/// <summary>
		/// Gets or sets the balloon vertical speed.
		/// </summary>
		/// <value>The balloon vertical speed.</value>
		public float BalloonVerticalSpeed {get;set;}
		/// <summary>
		/// Gets or sets the balloon horizontal speed.
		/// </summary>
		/// <value>The balloon horizontal speed.</value>
		public float BalloonHorizontalSpeed {get;set;}
		/// <summary>
		/// Gets or sets the max horizontal float amount.
		/// </summary>
		/// <value>The max horizontal float amount.</value>
		public float MaxHorizontalFloatAmount {get;set;}
		/// <summary>
		/// Gets or sets the rotation speed.
		/// </summary>
		/// <value>The rotation speed.</value>
		public float RotationSpeed {get;set;}
		/// <summary>
		/// Gets or sets the max rotation angle.
		/// </summary>
		/// <value>The max rotation angle.</value>
		public float MaxRotationAngle {get;set;}
        /// <summary>
        /// The max randomization percentage. 0-100
        /// </summary>
		public float MaxRandomization {get;set;}


        private bool m_isViewColliding = false;

		void Awake()
		{

		}

        // Use this for initialization
        void Start()
        {
			this.gameObject.tag = "Balloon";
            //ApplyRandomization();
			SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
			if(renderer == null)
				renderer = this.gameObject.AddComponent<SpriteRenderer>();

			Animator animator = this.gameObject.AddComponent<Animator>();
			Rigidbody2D body = this.gameObject.AddComponent<Rigidbody2D>();
			CircleCollider2D collider = this.gameObject.AddComponent<CircleCollider2D>();
			
			
			body.mass = 1f;
			body.drag = 1f;
			body.angularDrag = .05f;
			body.gravityScale = 0f;
			body.fixedAngle = true;
			body.isKinematic = false;
			body.interpolation = RigidbodyInterpolation2D.None;
			body.sleepMode = RigidbodySleepMode2D.StartAwake;
			
			collider.radius = .7f;
			collider.center = new Vector2(0f, 0.5f);	
			
			animator.runtimeAnimatorController = Resources.Load("BalloonAnimationController") as RuntimeAnimatorController;

        }

		/// <summary>
		/// Applies the Max Randomization property to all other properties to reduce their value accordingly.
		/// </summary>
        public void ApplyRandomization()
        {
            BalloonVerticalSpeed = PropertyReductionAmount(BalloonVerticalSpeed);
            BalloonHorizontalSpeed = PropertyReductionAmount(BalloonHorizontalSpeed);
            MaxHorizontalFloatAmount = PropertyReductionAmount(MaxHorizontalFloatAmount);
            RotationSpeed = PropertyReductionAmount(RotationSpeed);
            MaxRotationAngle = PropertyReductionAmount(MaxRotationAngle);
        }

        float PropertyReductionAmount(float propertyValue)
        {
            //percentage entered as 0-100
            float randomValue = CUtilities.GetRandom(0, MaxRandomization / 100);
            float reducedValue = 1.0f - randomValue;
            return propertyValue * reducedValue;

        }

        // Update is called once per frame
        void FixedUpdate()
        {            
            SwapHorizontalDirection();
            rigidbody2D.velocity = new Vector2(BalloonHorizontalSpeed, BalloonVerticalSpeed);

            //Apply rotation to balloon
            ReverseRotation();
            rigidbody2D.rotation += RotationSpeed;
        }

        void ReverseRotation()
        {
            if (Mathf.Abs(rigidbody2D.rotation) > MaxRotationAngle)
                RotationSpeed = -RotationSpeed;
        }

        void SwapHorizontalDirection()
        { SwapHorizontalDirection(false); }
        void SwapHorizontalDirection(bool ForceSwap)
        {
            if (Mathf.Abs(this.transform.position.x) > MaxHorizontalFloatAmount)
                BalloonHorizontalSpeed = -BalloonHorizontalSpeed;
        }

        void OnPopped()
        {
            Destroy(gameObject);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!m_isViewColliding && collision.gameObject.name == "BalloonComponent")
                MaxHorizontalFloatAmount = Mathf.Abs(this.transform.position.x) - .05f;
        }

    }
}