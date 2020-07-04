using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//任務選單 事件
public class MissionPic_btn : MonoBehaviour
{


    //下個任務倒數時間文字
    public TextMeshProUGUI NextMissionCountDownText;

    Coroutine nextAsk;

    public GameObject MissionList;

    bool IsNeedShowingTime = false;

    private void FixedUpdate()
    {
        if (nextAsk == null && IsNeedShowingTime)
        {
            //nextAsk = false;
            nextAsk= StartCoroutine(timer());
        }
    }
    public void OnpeMissionList()
    {
        //每秒要求時間
        IsNeedShowingTime = true;
        MissionList.GetComponent<Animator>().Play("MissionList_In");

    }

    public void CloseMissionList()
    {
        IsNeedShowingTime = false;
        MissionList.GetComponent<Animator>().Play("MissionList_Out");
        if (nextAsk != null) { StopCoroutine(nextAsk); }
    }

    IEnumerator timer()
    {
        //刷新時間
        //TimeManager.NowTime = TimeManager.GetNistTime();
        TimeManager.NowTime = System.DateTime.Now;//線上取時間改成使用本地時間



        //兩者相差時間
        System.TimeSpan gapTime = TimeManager.timeData.NextMissionTime - TimeManager.NowTime;

        int _compare = System.DateTime.Compare(TimeManager.timeData.NextMissionTime, TimeManager.NowTime);
        if (_compare >= 0)
        {
            NextMissionCountDownText.text = string.Format("{0:D2}:{1:D2}", gapTime.Minutes, gapTime.Seconds);
        }
        else
        {
            NextMissionCountDownText.text = "";
        }


        yield return new WaitForSeconds(1);
        nextAsk = null;
    }
}
