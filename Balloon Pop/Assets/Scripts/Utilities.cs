using UnityEngine;
using System.Collections;


namespace AuroraEndeavors.SharedComponents
{
    public class CUtilities
    {
        //public static float ConvertViewportXAxisToWorldXAxis(float viewportPos)
        //{
        //    Vector3 screenPos = Camera.main.ViewportToWorldPoint(new Vector3(viewportPos,0, 0));
        //    return screenPos.x;
        //}

        public static float ConvertViewportYAxisToWorldYAxis(float viewportPos)
        {
            Vector3 screenPos = Camera.main.ViewportToWorldPoint(new Vector3(0, viewportPos, 0));
            return screenPos.y;
        }



        public static int GetRandom(int from, int to)
        {
            //Random.seed = System.DateTime.Now.Millisecond;
            return Random.Range(from, to);
        }

        public static float GetRandom(float from, float to)
        {
            //Random.seed = System.DateTime.Now.Millisecond;
            return Random.Range(from, to);
        }


        public static float GetRandomScreenSingleAxisPos(float padding)
        {
            float viewportPos = Random.Range(0.0f + padding, 1.0f - padding);
            Vector3 screenPos = Camera.main.ViewportToWorldPoint(new Vector3(viewportPos, 0));

            //1. get random viewport position (0-1)
            //2. convert to screen posision
            //3. return
            return screenPos.x;

        }


    }
}