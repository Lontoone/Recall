using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using System;
//疲勞值
public class APcontroll : MonoBehaviour {

    //體力值
    public TextMeshProUGUI V_text;

    //恢復時間倒數ring
    public Image Ring;

    //下個AP恢復時間 (秒)
    //float nextAP_TotalDuration = 10;

    //更新疲勞恢復值
    private void Update()
    {
        TimeSpan duration = TimeManager.timeData.NextAPrestoreTime - TimeManager.NowTime;

        Ring.fillAmount = 1- (float)duration.TotalSeconds / (TimeManager.NextAP_Gap*60); //分鐘*60秒

        V_text.text = GM.playerData.AP.ToString() + "/5";

        //時間到就恢復疲勞值
        int isTimePassed = DateTime.Compare(TimeManager.NowTime, TimeManager.timeData.NextAPrestoreTime);

        //檢查時間(時間到)
        if (isTimePassed >= 0)
        {
            //恢復疲勞
            GM.playerData.AP+=(GM.playerData.AP>=5)?0:1;
            //下個倒數
            TimeManager.timeData.NextAPrestoreTime=TimeManager.NowTime.AddMinutes(TimeManager.NextAP_Gap);
            TimeManager.timeData.IsCountingNextAPTime = false;
        }
    }

    //開啟時，一次加回超時的疲勞值
    private void OnEnable()
    {
        //讀檔
        TimeManager.timeData= Save_LoadSystem.LoadTimeData();
        if (TimeManager.timeData==null) { TimeManager.timeData = new TimeManager.TimeData(); }

        //總經過時間.秒數/恢復所需時間.秒數 =疲勞值恢復數量
        TimeSpan duration = TimeManager.timeData.NextAPrestoreTime - TimeManager.NowTime;
        Debug.Log("中途時間 "+duration);
        Debug.Log("下個AP時間 "+ TimeManager.timeData.NextAPrestoreTime.ToString());
        int _ap = Mathf.FloorToInt((float)duration.TotalSeconds / (TimeManager.NextAP_Gap*60));
        Debug.Log("<color=red>這期間恢復了 " + _ap +" 疲勞</color>");

        //AP上限
        if (_ap >= 0)
        {
            GM.playerData.AP += _ap;
            if (GM.playerData.AP >= 5) { GM.playerData.AP = 5; }
        }
    }


}
