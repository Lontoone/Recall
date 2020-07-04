using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//拉動鏡頭
public class ViewScrollWithCamera : MonoBehaviour {

    public float dragSpeed = 2;
    private Vector2 dragOrigin;


    void Update()
    {

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                dragOrigin = Input.GetTouch(0).position;
                return;
            }

            else if (transform.position.x >= 0)
            {
                Vector2 pos = Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position - dragOrigin);
                Vector2 move = new Vector2(pos.x * dragSpeed, 0);

                Vector3 goalPos = Vector3.Lerp(pos, move, Time.unscaledDeltaTime * 5);

                transform.Translate(goalPos, Space.World);
                transform.position = new Vector3(transform.position.x, 1, transform.position.z);

            }
            
          
        }

        else if(transform.position.x<0)
        {
            transform.position = new Vector3(0, 1, transform.position.z);
        }

    }
}
