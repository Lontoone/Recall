using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using myCycusLib;

//Typing cellBobble
public class CellBobbleBehavior : MonoBehaviour {

    //生產該打擊提示的頻率level
    [HideInInspector]
    public int FreLevel;

    //生存時間
    [HideInInspector]
    public float _bobbleLifeTime=3;

    public GameObject TimingText_Prefab;

    //紀錄存在時間
    float timeCounter;

    //由動畫事件呼叫更改 Perfect/Good/Bad
    public string Score="Bad";

    [HideInInspector]
    public float OriginAnimationLength = 2;//打擊點動畫長度

    //(判定結束時間點) 判定時間=0.5*動畫速度比率
    float hintTime;
    float PerfectTime;
    float GoodTime;
    float BadTime;

    private void Start()
    {
        //設定動畫時間和存活時間的比例:
        Animator animator = gameObject.GetComponent<Animator>();
        float SpeedRate = 2 / _bobbleLifeTime;
        animator.speed =SpeedRate ;

        //打擊判定時間點
        hintTime    = _bobbleLifeTime  / 4 ;
        PerfectTime = _bobbleLifeTime  / 2 ;
        GoodTime    = _bobbleLifeTime  / 4 * 3 ;
        BadTime     = _bobbleLifeTime ;
        /*
        Debug.Log(hintTime);
        Debug.Log(PerfectTime);
        Debug.Log(GoodTime);
        Debug.Log(BadTime);*/
    }

    //被摧毀時
    public void OnDestroy()
    {
        try
        {
            TypingCellControl parent = transform.parent.parent.GetComponent<TypingCellControl>();
            if (parent != null)
            {
                parent.StartCoroutine(parent.ProduceGapTime(FreLevel, parent.GapTime));
            }
         
        }
        catch (System.Exception ex) {
            Debug.Log("<color=gray>不重要:</color>"+ex);
        }

        if (GM.playerPreference.IsVibrate)
        {
            //Handheld.Vibrate();
            Vibration.Vibrate(15);
        }


        //產生特效:----------------
        GameObject scoreTxt = Instantiate(TimingText_Prefab, gameObject.transform.position, Quaternion.identity);
        scoreTxt.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = Score;

        //紀錄

        switch (Score) {
            case "Perfect":
                ComboControl.Weighting[0]++;
  
                break;

            case "Good":
                ComboControl.Weighting[1]++;
                break;

            case "Bad":
                ComboControl.Weighting[2]++;
                break;
        }
        ComboControl.TotalScore = ComboControl.Weighting[0] * 5 + ComboControl.Weighting[1] * 2 + ComboControl.Weighting[2] - ComboControl.MissCombo * 3;

        Destroy(scoreTxt, 0.5f);
        //-------------------------

    }

    void Miss() {
        ComboControl.CurrtenCombo = 0;
        ComboControl.MissCombo++;
        //GameObject.Find("Eye").GetComponent<Animator>().Play("BasicEyeSet_ERROR");

        Score = "Miss";

        Destroy(gameObject);

    }

    //碰到玩家的柱子被消除
    public void Eliminate()
    {
       
        //計數
        ComboControl.CurrtenCombo++;
        ComboControl.Combototal++;

        //產生 Combo數量------
        Vector3 _pos = gameObject.transform.position;
        _pos.y += 0.5f;
        GameObject ComboTxt = Instantiate(TimingText_Prefab,_pos, Quaternion.identity);
        ComboTxt.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = ComboControl.CurrtenCombo.ToString();
        Destroy(ComboTxt,0.5f);
        //----------------

        Destroy(gameObject);
       

    }

    private void Update()
    {
        timeCounter += Time.deltaTime;

        //時間到未點擊=>miss
        if (timeCounter>=_bobbleLifeTime) {
            Miss();
        }

        if (timeCounter <= hintTime) {
            Score = "Bad";
        }
        else if (timeCounter <= PerfectTime) {
            Score = "Perfect";
        }
        else if (timeCounter <= GoodTime) {
            Score = "Good";
        }
        else if (timeCounter <= BadTime) {
            Score = "Bad";
        }
        //Debug.Log(timeCounter+" "+ Score);

        //多個打擊點重疊時=>確定自己是不是下一個
        if (transform.parent.GetChild(1) == this.gameObject.transform &&
            transform.parent.GetChild(0).GetComponent<TypingPinControl>().IsPressed)
        {
            CheckTouchBobble();
            transform.parent.GetChild(0).GetComponent<TypingPinControl>().IsPressed = false;
        }
    }


    //是否碰到打擊泡泡=>是就消除
    void CheckTouchBobble()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (myCycusLib.Detect.GetTouchedObj(i) != null)
            {
                for (int j = 0; j < myCycusLib.Detect.GetTouchedObj(i).Length ; j++)
                {
                    /*
                    if (GetTouchedObj(i)[j].transform.gameObject == this.gameObject)
                    {
                        
                        Eliminate();
                    }*/

                    if (myCycusLib.Detect.GetTouchedObj(i)[j].transform.gameObject == this.gameObject)
                    {

                        Eliminate();
                    }
                }
               
            }
        }
    }


    /*
    Vector3 touchPosWorld;
    public RaycastHit2D[] GetTouchedObj(int i)
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
           

        }
        return null;
    }


    */


}
