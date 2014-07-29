using UnityEngine;
using System.Collections;
using System;

namespace AuroraEndeavors.SharedComponents
{
    public class BalloonComponentController : MonoBehaviour
    {
        public int balloonCount;
        public float balloonSpawnWaitTime;
        //Viewport Space - 0.0f-1.0f
        public float balloonSpawnYPos = 0.0f;
        public GameObject balloon;
        public float screenPadding = 10;
        public float horizontalPadding;
        public Color[] colors;


        private int m_spawnedBalloonCount = 0;
        private float m_balloonXPos;
        private float m_balloonYWorldPos;
        private int m_balloonPoppedCount = 0;
        private bool m_allBalloonsArePopped = false;

        private bool CelebrationComplete = false;

        public event CEvents.CompletedEventHandler Completed;

        void OnCompleted(EventArgs e)
        {
            if (!CelebrationComplete)
            {
                CelebrationComplete = true;
                if (Completed != null)
                    Completed(e);
            }
        }

        // Use this for initialization
        void Start()
        {

            Reset();
            //test 
            TriggerBallons();
        }

        public void Reset()
        {
            //convert y StartPos to World Space;
            m_balloonYWorldPos = CUtilities.ConvertViewportYAxisToWorldYAxis(balloonSpawnYPos);
            m_spawnedBalloonCount = 0;
            m_balloonPoppedCount = 0;
            m_allBalloonsArePopped = false;
            CelebrationComplete = false;
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
                BalloonControllerScript newBalloon = goBalloon.GetComponent<BalloonControllerScript>();

                // Calculate a random x offset with enough padding to ensure it can't float of screen horizontally
                m_balloonXPos = CUtilities.GetRandomScreenSingleAxisPos(horizontalPadding);
                // m_balloonXPos =  CUtilities.GetRandomScreenSingleAxisPos(newBalloon.maxHorizontalFloatAmount);
                newBalloon.gameObject.transform.position = new Vector3(m_balloonXPos, m_balloonYWorldPos, 0);
                    
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

            //Check balloons out of bounds
            GameObject[] balloons = GameObject.FindGameObjectsWithTag("Balloon");

            ProcessInput(balloons);

            foreach (GameObject balloon in balloons)
            {
                VerifyOutOfBounds(balloon);
            }

            //if all are popped and all destroyed, this means they have been popped, 
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

            //test outof bounds
            if (balloonScreenPos.y > Screen.height - screenPadding)
            {
                PopBalloon(balloon);
                //anim.SetTrigger("Touched");
                //IncrementBalloonPoppedCount();
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
#if UNITY_EDITOR
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