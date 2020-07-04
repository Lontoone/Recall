using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialControl : MonoBehaviour {

    //可更改
    public float RollBackSpeed = 3.5f;

    //----------------------------------
    bool drag = false;
    public string DialName;

    //開始拉動時=>紀錄拉自己的手指編號
    public void StartDragging()
    {
        
        drag = true;
       
    }

    //update: 每秒偵測自己對應手指的移動偏移，並跟著旋轉
    private void Update()
    {
        try
        {

            Dragging();
            
        }
        catch(System.Exception ex) {
            Debug.Log(ex );
            
        }
        
    }

    float rotateDegree = 0;
   
    //拉轉盤
    public void Dragging()
    {
       
        if (drag && Input.touchCount > 0)
        {

            foreach (Touch touch in Input.touches)
            {
                if ((touch.position.x <= Screen.width / 2 && DialName == "Dial_Left") ||
                    (touch.position.x >= Screen.width / 2 && DialName == "Dial_Right"))
                {

                    rotateDegree += -touch.deltaPosition.x;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotateDegree);
                    //rotateDegree += -Input.touches[fingerId % Input.touchCount].deltaPosition.x;
                }
                else
                {

                }

            }
           
        }
        //停止拉動時
        else
        {
            //角度逐漸趨近0
            rotateDegree = Mathf.Lerp(rotateDegree, 0, Time.deltaTime* RollBackSpeed);
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotateDegree);
        }
    }



   
    public void StopDragging()
    {
        drag = false;
        //Debug.Log("Exit"+gameObject.name+ Mytouches.Length);
    }

    //報廢
    //檢查正在拉的是不是自己的物件
    bool DetectTouchObj() {
        int i = 0;
        while (i < Input.touchCount)
        {

            if (Input.GetTouch(i).phase == TouchPhase.Moved || Input.GetTouch(i).phase == TouchPhase.Stationary )
            {
                //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), -Vector2.up);

                //Debug.Log("TAP !");

                if (hit.collider != null && hit.collider.tag=="Dial")
                {

                    if (hit.transform.name == DialName)
                    {
                        Debug.Log(hit.transform.name);
                        return true;
                    }
                    Debug.Log(hit.transform.name);

                }
            }
            ++i;
           
        }
        return false;
    }

}




