using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Cache;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

using UnityEngine.SceneManagement;

//網路時間管理
public class TimeManager : MonoBehaviour {

    //下個任務間隔時間
    public static int NextMissionGap = 40;

    //下個疲勞值恢復間隔時間
    public static int NextAP_Gap = 30;

    //*****************時間資料*******************
    [System.Serializable]
    public class TimeData {
        //下一個任務時間
        public DateTime NextMissionTime=System.DateTime.Now;

        //下一個體力回復時間
        public DateTime NextAPrestoreTime= System.DateTime.Now;
        //是否在計算下個AP時間
        public bool IsCountingNextAPTime=false;
    }
    public static TimeData timeData = new TimeData();
    //********************************************

    //目前時間
    public static DateTime NowTime=System.DateTime.Now;

    bool CanCallnetwork = true;
    
    private void Update()
    {

      //每30秒取得時間 (只在主畫面有用)
        if (CanCallnetwork && SceneManager.GetActiveScene().name== "MainScene")
        {
            CanCallnetwork = false;
            StartCoroutine(t(30));
        }
      
    }

    

    IEnumerator t(int _sec) {

        NowTime = GetNistTime();

        yield return new WaitForSeconds(_sec);
        CanCallnetwork = true;
    }

    public static DateTime GetNistTime()
    {
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
        //var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.google.com"); break!!
        var response = myHttpWebRequest.GetResponse();
        string todaysDates = response.Headers["date"];

        return DateTime.ParseExact(todaysDates,
                                   "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                   CultureInfo.InvariantCulture.DateTimeFormat,
                                   DateTimeStyles.AssumeUniversal);
    }

    private void Awake()
    {
        Debug.Log("Load TimeData!");
        //讀檔
        timeData = Save_LoadSystem.LoadTimeData();
        if (timeData==null){ timeData = new TimeData(); }
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("Time");
        //存檔
        Save_LoadSystem.SaveTimeData(timeData);
    }

    private void OnApplicationQuit()
    {
        //存檔
        Save_LoadSystem.SaveTimeData(timeData);

    }

}
