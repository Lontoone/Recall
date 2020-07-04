using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//自訂節點:攝影機看不到時不能觸碰
public class UntuchableOutsideCamera : MonoBehaviour {

    //時間點的文字
    public TMPro.TextMeshProUGUI timeText;

    //紀錄原本的字串
    [HideInInspector]
    public string origineText="";

    //消失的界線
    public float InvisiblePoint_X;
    private void Update()
    {
        //攝影機和物件的距離超過就不能觸碰
        if (Camera.main.transform.position.x - gameObject.transform.position.x > InvisiblePoint_X)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;

            if (origineText=="") { origineText = timeText.text; }
            //文字也消除
            timeText.text = "";
        }
        else {

            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            if (origineText!="") {
                timeText.text = origineText;
            }
        }
    }





}
