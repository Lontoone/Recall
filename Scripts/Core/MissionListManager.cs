using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using TMPro;

//任務列表管理
public class MissionListManager : MonoBehaviour {


    public GameObject MissionListPrefab;

    [Header("Scroll view的content物件")]
    public GameObject Content;

    //所有種類任務的數量總和
    int _missionTypeTotalCount = 5;

    private void Start()
    {
        CreatMissionList();
    }

    private void Update()
    {
        CheckNextMission();
    }

    //產生任務列表
    public void CreatMissionList() {
        //任務數量
        MissionClass[] _missions = MissionManager.instance.GetComponents<MissionClass>();

        //生產物件------------

        //先清空ScrowView
        for (int i = 0; i < Content.transform.childCount; i++)
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < _missions.Length; i++)
        {
            //ScrolView的排列
            Vector3 nextBtnPos = new Vector3(MissionListPrefab.GetComponent<RectTransform>().rect.width / 2,
                -MissionListPrefab.GetComponent<RectTransform>().rect.height * i - MissionListPrefab.GetComponent<RectTransform>().rect.height / 2
                , 0);

            //產生按鈕
            GameObject mission_item = Instantiate(MissionListPrefab, Content.transform);
            mission_item.transform.position = nextBtnPos;
            //設定位置
            mission_item.transform.localPosition = nextBtnPos;

            //文字
            mission_item.GetComponentInChildren<TextMeshProUGUI>().text = _missions[i].Describe;
            
            mission_item.transform.Find("MissionProgress_text").GetComponent<TextMeshProUGUI>().text = _missions[i].CurrentProgress.ToString() + "/" + _missions[i].Requirment.ToString();
        }

        Content.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Content.transform.GetComponent<RectTransform>().sizeDelta.x
                                                                                , MissionListPrefab.GetComponent<RectTransform>().rect.height * _missions.Length);

    }


    //檢查生成下一個任務
    public void CheckNextMission() {
        //任務數量小於最大可接任務量 (5個)
        if (Content.transform.childCount<5) {

            //若還沒時間到，則<0
            int isTimePassed = DateTime.Compare(TimeManager.NowTime,TimeManager.timeData.NextMissionTime);
            
            //檢查時間(時間到)
            if (isTimePassed>=0) {
                //生產任務
                int _type = UnityEngine.Random.Range(0,30);
               
                RamdomAddMission((_type% _missionTypeTotalCount) +1);

                CreatMissionList();

                //後延時間
                TimeManager.timeData.NextMissionTime = TimeManager.NowTime.AddMinutes(TimeManager.NextMissionGap);
                Debug.Log("生產任務,下一次將在:"+TimeManager.timeData.NextMissionTime);
            }
        }

    }

    //開啟時，一次加回超時的任務
    private void OnEnable()
    {
        //讀檔
        TimeManager.timeData = Save_LoadSystem.LoadTimeData();
        if (TimeManager.timeData == null) { TimeManager.timeData = new TimeManager.TimeData(); }

        //總經過時間.秒數/任務產生所需時間.秒數 =任務增加數量
        TimeSpan duration = TimeManager.timeData.NextMissionTime - TimeManager.NowTime;
        Debug.Log("任務中途時間 " + duration);
        Debug.Log("下個任務時間 " + TimeManager.timeData.NextMissionTime.ToString());

        int _mis = Mathf.FloorToInt((float)duration.TotalSeconds / (TimeManager.NextMissionGap * 60));
        Debug.Log("<color=red>這期間恢復了 " + _mis + " 個任務</color>");

        //產生任務
        if (_mis >= 0)
        {
            //目前任務數量
            int _missinonCount = MissionManager.instance.GetComponents<MissionClass>().Length;
            //任務上限
            _mis = ((_missinonCount + _mis) > 5) ? (5 - _missinonCount) : _mis;

            //增加任務
            for (int i = 0; i < _mis; i++) {
                //生產任務
                int _type = UnityEngine.Random.Range(0, 30);

                RamdomAddMission((_type % _missionTypeTotalCount) + 1);

            }
            //刷新頁面
            CreatMissionList();

        }
    }


    //隨機加入任務
    void RamdomAddMission(int _missionType) {
        if (_missionType == 1) { GameObject.FindObjectOfType<MissionManager>().gameObject.AddComponent<Mission1_FullCombo>(); }
        if (_missionType == 2) { GameObject.FindObjectOfType<MissionManager>().gameObject.AddComponent<Mission2_TotalScore>(); }
        if (_missionType == 3) { GameObject.FindObjectOfType<MissionManager>().gameObject.AddComponent<Mission3_TotalPerfect>(); }
        if (_missionType == 4) { GameObject.FindObjectOfType<MissionManager>().gameObject.AddComponent<Mission4_LessBad>(); }
        if (_missionType == 5) { GameObject.FindObjectOfType<MissionManager>().gameObject.AddComponent<Mission5_LimitMusicTime>(); }
    }
}







