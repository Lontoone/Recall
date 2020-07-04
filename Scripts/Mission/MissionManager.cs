using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//任務管理
//可以掛載任務元件在此物件
public class MissionManager : MonoBehaviour
{
    [System.Serializable]
    public class MissionData {
        //存讀檔用任務資料型態
        //{ 任務類型, 進度,達成條件,EXP,(額外)總分要求}
        public int[] missionData= { 0,0,0,0,0};
        public MissionData() { }
        public MissionData(int type, int progress, int requirment, int exp)
        {
            missionData[0] = type;
            missionData[1] = progress;
            missionData[2] = requirment;
            missionData[3] = exp;

        }
        public MissionData(int type,int progress,int requirment, int exp, int totalScoreRequire) {
            missionData[0] = type;
            missionData[1] = progress;
            missionData[2] = requirment;
            missionData[3] = exp;

            missionData[4] = totalScoreRequire;
        }
    }
    
    //任務清單
    public static List<MissionData> missionDatas = new List<MissionData>();

    public static MissionManager instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

       
    }

    //讀檔
    private void Start()
    {
        //讀檔 :任務檔案
        missionDatas = Save_LoadSystem.LoadMissinData();
        //清空所有目前任務:
        MissionClass[] _nowMis = gameObject.GetComponents<MissionClass>();
        for (int i = 0; i < _nowMis.Length; i++)
        {
            Destroy(_nowMis[i]);
        }

        //
        if (missionDatas != null)
        {
            Debug.Log("讀檔任務有:" + missionDatas.Count);
            SetLoadedMission();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        SaveMissionData();
    }
    private void OnApplicationQuit()
    {
        SaveMissionData();
    }

    //讀檔後 設定任務
    void SetLoadedMission() {
      
        foreach (MissionData ms in missionDatas) {
            //分類type
            switch (ms.missionData[0]) {
                case 1:
                    //full Combo
                    SetMissionComponent(gameObject.AddComponent<Mission1_FullCombo>(),ms.missionData[1],ms.missionData[2],ms.missionData[3]);
                    break;

                case 2:
                    //總分要求
                    SetMissionComponent(gameObject.AddComponent<Mission2_TotalScore>(), ms.missionData[1], ms.missionData[2], ms.missionData[3],ms.missionData[4]);
                    break;

                case 3:
                    //累積Perfect數量
                    SetMissionComponent(gameObject.AddComponent<Mission3_TotalPerfect>(), ms.missionData[1], ms.missionData[2], ms.missionData[3]);
                    break;

                case 4:
                    //Bad數量小於要求
                    SetMissionComponent(gameObject.AddComponent<Mission4_LessBad>(), ms.missionData[1], ms.missionData[2], ms.missionData[3],ms.missionData[4]);
                    break;
                    
                case 5:
                    //遊玩音樂時間大於一定數
                    SetMissionComponent(gameObject.AddComponent<Mission5_LimitMusicTime>(), ms.missionData[1], ms.missionData[2], ms.missionData[3], ms.missionData[4]);
                    break;
            }

        }

        //刷新
        GameObject.FindObjectOfType<MissionListManager>().CreatMissionList();

    }


    void SetMissionComponent(MissionClass msClss,int _progress, int _requirment, int _exp) {
        msClss.CurrentProgress = _progress;
        msClss.Requirment = _requirment;
        msClss.ExpReward = _exp;


        SetMissionLanguage(msClss);
    }

    void SetMissionComponent(MissionClass msClss, int _progress, int _requirment, int _exp,int totalScore)
    {
        msClss.CurrentProgress = _progress;
        msClss.Requirment = _requirment;
        msClss.ExpReward = _exp;

        msClss.TotalScoreRequirment = totalScore;

        //更新任務資訊
        SetMissionLanguage(msClss);
    }


    //改變任務描述語言
    public static void SetMissionLanguage(MissionClass msClass) {
        switch(msClass.Type){
            case 1:
                //Full Combo
                if (GM.Language=="Che") {
                    msClass.Describe = "Full Combo";
                }
                if (GM.Language=="Eng") {
                    msClass.Describe = "Full Combo";
                }
                if (GM.Language=="JP") {
                    msClass.Describe = "Full Combo";
                }

                break;

            case 2:
                //總分數達標
                if (GM.Language == "Che")
                {
                    msClass.Describe = "總分數大於"+msClass.TotalScoreRequirment.ToString();
                }
                if (GM.Language == "Eng")
                {
                    msClass.Describe = "Total Score more than" + msClass.TotalScoreRequirment.ToString();
                }
                if (GM.Language == "JP")
                {
                    msClass.Describe = "總分數大於" + msClass.TotalScoreRequirment.ToString();
                }

                break;

            case 3:
                //累積Perfect數量
                if (GM.Language == "Che")
                {
                    msClass.Describe = "累積Perfect : " + msClass.Requirment.ToString();
                }
                if (GM.Language == "Eng")
                {
                    msClass.Describe = "Perfect quantity reaches : " + msClass.Requirment.ToString();
                }
                if (GM.Language == "JP")
                {
                    msClass.Describe = "累積Perfect :" + msClass.Requirment.ToString();
                }

                break;

            case 4:
                //Bad數量小於要求
                if (GM.Language == "Che")
                {
                    msClass.Describe = "Bad數量小於 : " + msClass.TotalScoreRequirment.ToString();
                }
                if (GM.Language == "Eng")
                {
                    msClass.Describe = "Bad quantity is less than : " + msClass.TotalScoreRequirment.ToString();
                }
                if (GM.Language == "JP")
                {
                    msClass.Describe = "Bad數量小於 : " + msClass.TotalScoreRequirment.ToString();
                }

                break;

            case 5:
                //音樂長度大於要求

                //換算音樂時間
                string _req = (msClass.TotalScoreRequirment / 60).ToString() +" : " + (msClass.TotalScoreRequirment % 60).ToString();
                if (GM.Language == "Che")
                {
                    msClass.Describe = "遊玩音樂長度大於  " +_req;
                }
                if (GM.Language == "Eng")
                {
                    msClass.Describe = "Music length is greater than " + _req;
                }
                if (GM.Language == "JP")
                {
                    msClass.Describe = "遊玩音樂長度大於  " + _req;
                }

                break;
        }
    }

    //存檔
    void SaveMissionData() {
        //存檔案
        //取得目前所有任務資訊
        MissionClass[] missionClass = GameObject.FindObjectsOfType<MissionClass>();
        Debug.Log("存檔時任務數量:" + missionClass.Length);
        for (int i = 0; i < missionClass.Length; i++)
        {
        }

        if (missionDatas != null)
        {
            missionDatas.Clear();
        }

        foreach (MissionClass ms in missionClass)
        {
            MissionData _missiondata = new MissionData(ms.Type, ms.CurrentProgress, ms.Requirment, ms.ExpReward, ms.TotalScoreRequirment);

            if (missionDatas == null)
            {
                missionDatas = new List<MissionData>();
            }

            missionDatas.Add(_missiondata);
        }

        //SAVE
        Save_LoadSystem.SaveMissionData(missionDatas);
    }
}
