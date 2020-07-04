using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace myCycusLib {
    public class MyCycusLib : MonoBehaviour
    {

    }


    [RequireComponent(typeof(Collider2D))]
    public class Detect {

        static Vector3 touchPosWorld;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i">GetTouch(i)</param>
        /// <returns></returns>
        public static RaycastHit2D[] GetTouchedObj(int i)
        {
            //We check if we have more than one touch happening.
            //We also check if the first touches phase is Ended (that the finger was lifted)
            if (Input.touchCount > 0)//&& (Input.GetTouch(i).phase == TouchPhase.Stationary || Input.GetTouch(i).phase==TouchPhase.Moved))
            {
                //We transform the touch position into word space from screen space and store it.
                touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);

                Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

                //We now raycast with this information. If we have hit something we can process it.
                RaycastHit2D[] hitInformation = Physics2D.RaycastAll(touchPosWorld2D, Camera.main.transform.forward);

                return hitInformation;
                /*
                 if (hitInformation.collider != null)
                 {
                     //We should have hit something with a 2D Physics collider!
                     GameObject touchedObject = hitInformation.transform.gameObject;
                     //touchedObject should be the object someone touched.
                     Debug.Log("Touched " + touchedObject.transform.name);

                     return touchedObject;
                 }*/

            }
            return null;
        }
        public static RaycastHit2D[] GetTouchedObj(int i,TouchPhase touchPhase)
        {
            //We check if we have more than one touch happening.
            //We also check if the first touches phase is Ended (that the finger was lifted)
            if (Input.touchCount > 0 &&  Input.GetTouch(i).phase == touchPhase)
            {
                //We transform the touch position into word space from screen space and store it.
                touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);

                Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

                //We now raycast with this information. If we have hit something we can process it.
                RaycastHit2D[] hitInformation = Physics2D.RaycastAll(touchPosWorld2D, Camera.main.transform.forward);

                return hitInformation;
                /*
                 if (hitInformation.collider != null)
                 {
                     //We should have hit something with a 2D Physics collider!
                     GameObject touchedObject = hitInformation.transform.gameObject;
                     //touchedObject should be the object someone touched.
                     Debug.Log("Touched " + touchedObject.transform.name);

                     return touchedObject;
                 }*/

            }
            return null;
        }
    }
}


