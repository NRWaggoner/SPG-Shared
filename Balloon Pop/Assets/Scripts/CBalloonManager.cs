using UnityEngine;
using System.Collections;
using System;

namespace AuroraEndeavors.SharedComponents
{
    public class CBalloonManager: MonoBehaviour
    {
        public int balloonCount;
        public float balloonSpawnWaitTime;
        //Viewport Space - 0.0f-1.0f
        public float balloonSpawnYPos = 0.0f;
        public GameObject balloon;
        public float screenPadding = 10;
        public float horizontalPadding;
        public Color[] colors;

		public float balloonVerticalSpeed;
		public float balloonHorizontalSpeed;
		public float maxHorizontalFloatAmount;
		public float rotationSpeed;
		public float maxRotationAngle;
		public float maxRandomization;

        private int m_spawnedBalloonCount = 0;
        private float m_balloonXPos;
        private float m_balloonYWorldPos;
        private int m_balloonPoppedCount = 0;
        private bool m_allBalloonsArePopped = false;

        private bool m_celebrationComplete = false;

        public event CEvents.CompletedEventHandler Completed;

        void OnCompleted(EventArgs e)
        {
            if (!m_celebrationComplete)
            {
                m_celebrationComplete = true;
                if (Completed != null)
                    Completed(e);
            }
        }

        // Use this for initialization
        void Start()
        {

            Reset();
            //TODO Testing: TriggerBalloons() called in Start() for testing puurposes (remove when no longer needed)
            TriggerBallons();
        }

        public void Reset()
        {            
            m_balloonYWorldPos = CUtilities.ConvertViewportYAxisToWorldYAxis(balloonSpawnYPos);
            m_spawnedBalloonCount = 0;
            m_balloonPoppedCount = 0;
            m_allBalloonsArePopped = false;
            m_celebrationComplete = false;
        }


        public void TriggerBallons()
        {
            StartCoroutine(SpawnBalloons());
        }

        IEnumerator SpawnBalloons()
        {
            while (m_spawnedBalloonCount < balloonCount)
            {
                yield return new WaitForSeconds(balloonSpawnWaitTime);

                GameObject goBalloon = (GameObject)Instantiate(balloon, new Vector3(m_balloonXPos, m_balloonYWorldPos, 0), Quaternion.identity);
                CBalloonController newBalloon = goBalloon.GetComponent<CBalloonController>();

                // Calculate a random x offset with enough padding to ensure it can't float of screen horizontally
                m_balloonXPos = CUtilities.GetRandomScreenSingleAxisPos(horizontalPadding);               
                newBalloon.gameObject.transform.position = new Vector3(m_balloonXPos, m_balloonYWorldPos, this.transform.position.z);
				newBalloon.BalloonVerticalSpeed = balloonVerticalSpeed;
				newBalloon.BalloonHorizontalSpeed = balloonHorizontalSpeed;
				newBalloon.MaxHorizontalFloatAmount = maxHorizontalFloatAmount;
				newBalloon.RotationSpeed = rotationSpeed;
				newBalloon.MaxRotationAngle = maxRotationAngle;
				newBalloon.MaxRandomization = maxRandomization;
				newBalloon.ApplyRandomization();
                    
                SpriteRenderer spriteRenderer = newBalloon.GetComponent<SpriteRenderer>();  
                spriteRenderer.color = ChooseRandomColor();
                m_spawnedBalloonCount++;
            }
        }

        Color ChooseRandomColor()
        {
            int i = CUtilities.GetRandom(0, colors.Length);
            return colors[i];
        }

        void IncrementBalloonPoppedCount()
        {
            m_balloonPoppedCount++;

            if (m_balloonPoppedCount == balloonCount)
            {
                m_allBalloonsArePopped = true;

            }

        }

        // Update is called once per frame
        void Update()
        {           
            GameObject[] balloons = GameObject.FindGameObjectsWithTag("Balloon");

			//Check for balloon touches/clicks
            ProcessInput(balloons);

            foreach (GameObject balloon in balloons)
            {
                VerifyOutOfBounds(balloon);
            }

            //If all are popped and all destroyed, this means they have been popped, 
            //or gone out of bounds and their animations have completed
            if (m_allBalloonsArePopped && balloons.Length == 0)
            {
                OnCompleted(new EventArgs());
            }
        }

        void VerifyOutOfBounds(GameObject balloon)
        {
            Vector3 balloonScreenPos = Camera.main.WorldToScreenPoint(balloon.rigidbody2D.position);
            //Animator anim = balloon.GetComponent<Animator>() as Animator;

            if (balloonScreenPos.y > Screen.height - screenPadding)
            {
                PopBalloon(balloon);               
            }
        }
	
        void PopBalloon(GameObject balloon)
        {
            Animator anim = balloon.GetComponent<Animator>() as Animator;
            anim.SetTrigger("Touched");
            IncrementBalloonPoppedCount();
        }

        void ProcessInput(GameObject[] balloons)
        {
            bool success = false;
            Vector3 touchPos = new Vector3();

#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_METRO
            if (Input.GetMouseButtonDown(0))
            {
                success = true;
                touchPos = Input.mousePosition;

            }
#endif

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    success = true;
                    touchPos = touch.position;
                }
            }

            if (success)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);

                foreach (GameObject balloon in balloons)
                {
                    worldPos.z = balloon.transform.position.z;
                    if (balloon.collider2D.bounds.Contains(worldPos))
                    {
                        PopBalloon(balloon);

                    }
                }
            }

        }
    }
}