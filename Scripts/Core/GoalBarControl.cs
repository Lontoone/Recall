using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBarControl : MonoBehaviour {

    //最大最小角度
    //public float MaxAngle, MinAngle;

    public GameObject Dial;

    private void FixedUpdate()
    {
        Spin();
        
    }

    public void Spin() {
       
        //更新自己的旋轉=Dial旋轉
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Dial.transform.rotation.z*50 );

        /*
        //轉到最大或最小就不再轉(??
        if (transform.rotation.z > MinAngle + 20 && transform.rotation.z < MaxAngle - 20)
        {
           
        }
        else {
         
            //最近的極限角度
            float nearLim = (Mathf.Abs(transform.rotation.z - MaxAngle) > Mathf.Abs(transform.rotation.z - MinAngle)) ?
                        MaxAngle : MinAngle;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, nearLim);
        */
        
    }
}
