using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using TMPro;

//遊戲結束控制
public class GameOverHandle : MonoBehaviour {

    GameObject gm;

    //距離結束時間
    public float TimeToEnd;

    private void Start()
    {
       
    }
    private void Update()
    {
       
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scence, LoadSceneMode mod)
    {

        //重置
        if (scence.name == "GamePlay_TypingMode"  || scence.name == "GamePlay_PlayCustume")
        {
            gm = GameObject.FindObjectOfType<GM>().gameObject;
           
            TimeToEnd = gm.GetComponent<GM>().audioSource.clip.length + 5;

            StartCoroutine(_countdown());


            ComboControl.Combototal   = 0;
            ComboControl.CurrtenCombo = 0;
            ComboControl.HighestCombo = 0;
            ComboControl.MissCombo    = 0;
            ComboControl.Weighting[0] = 0;
            ComboControl.Weighting[1] = 0;
            ComboControl.Weighting[2] = 0;
            ComboControl.TotalScore   = 0;
        }


        if (scence.name == "EndingCounting")
        {
            GM.IsFirstIn = false;

            //處理疲勞值
            SetAP();

            //故事控制=>玩完一局才能觸發下個對話-------------
            StoryManager.storyProcess.IsNextGame = true;
            Save_LoadSystem.SaveStoryProcess(StoryManager.storyProcess);
            //---------------------------------

            float _bestPercentage = (float)ComboControl.Combototal / (float)(ComboControl.Combototal + ComboControl.MissCombo);

            total_text.text                 = ComboControl.Combototal.ToString();
            complete_text.text              = _bestPercentage.ToString("p1");
            hightestCombo_text.text         = ComboControl.HighestCombo.ToString();
            Miss_text.text                  = ComboControl.MissCombo.ToString();
            Perfect_text.text               = ComboControl.Weighting[0].ToString();
            Good_text.text                  = ComboControl.Weighting[1].ToString();
            Bad_text.text                   = ComboControl.Weighting[2].ToString();

            //算加權分數
            //ComboControl.TotalScore         = ComboControl.Weighting[0] * 5 + ComboControl.Weighting[1] * 2 + ComboControl.Weighting[2] - ComboControl.MissCombo * 3;
            WeightingScore_text.text        = ComboControl.TotalScore.ToString();


            //如果紀錄有超越前面的=>寫入更新
            GM.instance.TempSongData.BestPercentage = (GM.instance.TempSongData.BestPercentage > _bestPercentage) ? GM.instance.TempSongData.BestPercentage : _bestPercentage;
            GM.instance.TempSongData.BestScore      = (GM.instance.TempSongData.BestScore > ComboControl.TotalScore) ? GM.instance.TempSongData.BestScore : ComboControl.TotalScore;

            //計算經驗值
            GM.playerData.CurrentExp +=5*(int)Mathf.Pow(ComboControl.TotalScore, 0.2f)*GM.playerData.Level;
            if (GM.playerData.CurrentExp<=0) { GM.playerData.CurrentExp = 0; }

            Save_LoadSystem.SaveSongData(GM.instance.TempSongData);
        }
    }

   

    IEnumerator _countdown() {
        yield return new WaitForSeconds(TimeToEnd+3);

        SongOver();
    }

    //當歌曲播完過x秒後執行
    public void SongOver() {
        Debug.Log("Over!");
        //黑幕一下後換場景:(在別的場景結算)

        SceneManager.LoadScene("EndingCounting");

    }

    public TextMeshProUGUI total_text;
    public TextMeshProUGUI complete_text;
    public TextMeshProUGUI Miss_text;
    public TextMeshProUGUI Perfect_text;
    public TextMeshProUGUI Good_text;
    public TextMeshProUGUI Bad_text;
    public TextMeshProUGUI hightestCombo_text;
    public TextMeshProUGUI WeightingScore_text;


    public void LoadScene(string SceneName) {
        SceneManager.LoadScene(SceneName);
    }

    void SetAP() {

        //疲勞值-1
        //GM.playerData.AP -= 1; 不限制AP

        //下個疲勞恢復時間
        if (TimeManager.timeData.IsCountingNextAPTime==false)
        {
            TimeManager.timeData.IsCountingNextAPTime = true;
            TimeManager.timeData.NextAPrestoreTime = TimeManager.NowTime.AddMinutes(TimeManager.NextAP_Gap);
        }
        Save_LoadSystem.SaveTimeData(TimeManager.timeData);
        Debug.Log("下一個AP恢復時間:" + TimeManager.timeData.NextAPrestoreTime);

    }
}
