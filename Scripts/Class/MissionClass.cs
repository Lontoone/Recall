using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionClass : MonoBehaviour {
    [HideInInspector]
    //任務類型
    public int Type;

    public bool IsComplete = false;
    //內容描述
    [TextArea]
    public string Describe = "";

    //經驗值給予?
    public int ExpReward=0;

    //要求達成條件(場數)
    public int Requirment;
    //目前進度
    public int CurrentProgress=0;

    //補充條件:
    //總分要求
    public int TotalScoreRequirment=0;
    //*************

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public virtual void Awake()
    {
        //取得對應exp值
        ExpReward = GetMissionExpReward();

        //隨機次數
        Requirment = Random.Range(1, 5)+1;

    }

    //進入場景時
    void OnSceneLoaded(Scene scence, LoadSceneMode mod)
    {
        //結算畫面時，確認任務達成狀態
        if (scence.name == "EndingCounting")
        {
            if (CheckRequirement())
            {
                Reward();

                //顯示達成任務----------------------------
                GameObject MissionListLable = GameObject.Find("MissionText");
                MissionListLable.GetComponent<UnityEngine.UI.Image>().enabled=true;
                MissionListLable.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().enabled = true;

                TMPro.TextMeshProUGUI missionText = new TMPro.TextMeshProUGUI();
                missionText = GameObject.Find("MissionContent Text").GetComponent<TMPro.TextMeshProUGUI>();
                missionText.text += Describe;
                missionText.text += "\n";
                //-------------------------------------

            }
        }

        //回主畫面 取得獎勵
        if (scence.name == "MainScene")
        {
        }
    }


    ///<summary>判斷是否達成條件</summary>
    public virtual bool CheckRequirement() {
        return true;
    }

    /// <summary>
    /// 達成條件的觸發事件
    /// </summary>
    public virtual void Reward() {

        GM.playerData.MissionCompleteCount++;
        GM.playerData.CurrentExp += ExpReward;

        Debug.Log("<color=blure>GM:任務已解: "+ GM.playerData.MissionCompleteCount+" 個</color>");
        Debug.Log("<color=blure>GM:經驗值: " + GM.playerData.CurrentExp + "</color>");

        //獲得經驗
        GM.playerData.CurrentExp += ExpReward;
        Debug.Log(GM.playerData.CurrentExp);
        //刪除此任務
        Destroy(this);
    }

    public static int GetMissionExpReward() {
        //任務經驗值獎勵=x^0.75*20+25
        int reward = (int)Mathf.Pow(GM.playerData.Level,0.75f)*20+25;
        return reward;
    }
}
