using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class GM : MonoBehaviour {

    [Tooltip("有聲/無聲頻道")]
    public AudioMixerGroup Mute_mixerGroup, Sound_mixerGroup;

    public static GM instance;

    public SongClass TempSongData = new SongClass();

    //無聲(打擊點)生成延遲
    public float MuteSoundPlayDelay;

    //自訂打擊點生產延遲時間
    public float CustumeNodesStartDelay;

    //泡泡原始動畫長度
    int BubbleOriginAnimaTime = 2;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        DetectLanguage();

        Application.runInBackground = true;


        //等級讀檔
        GM.playerData = Save_LoadSystem.LoadPlayerData();
        if (GM.playerData == null) { GM.playerData = new PlayerData(); }

    }

    //***************偏好設定*******************

    #region
    [System.Serializable]
    public class PlayerPreference {
        public bool IsVibrate = true;//開啟震動
        public bool IsMetronome = false;//節拍器
        public int Metronome_bpm = 80; //節拍器速度

        //------------音量---------------
        public float GamePlayMusic_V;
        public float MainBGM_V = -15;
        public float Metronome_V = -10;
        public float SoundEffect_V;

    }

    //難度 0~3 :"Easy","Normal","Hard","Extreme"
    public static int Difficult = 0;

    public static PlayerPreference playerPreference = new PlayerPreference();

    //-----------------------------------------
    #endregion


    //*****************玩家資料*******************
    #region
    //經驗值
    /*
     * 最大經驗值=(等級*100)^1.15+80
     * 每場遊戲經驗: 5*(總分)^0.2*等級
     * 任務經驗值獎勵=x^0.75*20+25
     */
     [System.Serializable]
    public class PlayerData{
        //任務完成總量
        public int MissionCompleteCount=0;
        //體力條
        public int AP=5;
        //等級
        public int Level = 1;
        //目前經驗值
        public int CurrentExp=0;
        //經驗值上限(超過就升級)
        public int MaxExp=80;

        public PlayerData() {
            //設定上限
            MaxExp = (int)Mathf.Pow((Level * 100), 1.15f) + 80;
        }
    }

    public static PlayerData playerData = new PlayerData();
    #endregion
    //*******************************************
    

    //------------------------------------------


    public AudioSource audioSource;

    public static string Language;

    //暫時設false
    public static bool IsFirstIn = false;

    //偵測語言
    public void DetectLanguage() {
        string _textFontPath = "";

        if (Application.systemLanguage == SystemLanguage.Chinese ||
            Application.systemLanguage == SystemLanguage.ChineseSimplified ||
             Application.systemLanguage == SystemLanguage.ChineseTraditional)
        {
            Language = "Che";
            //Language = "Eng"; //測試用
            //_textFontPath = "TextFont/" + "ChineseMain_genkai-mincho";
            _textFontPath = "TextFont/" + "genkai-mincho";

        }
        else if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            Language = "JP";
            //Language = "Che"; //暫時
            //_textFontPath = "TextFont/" + "ChineseMain_genkai-mincho";
            _textFontPath = "TextFont/" + "genkai-mincho";
        }
        //預設英文
        else {
            Language = "Eng";
            //Language = "Che"; //測試用
            //_textFontPath = "TextFont/" + "ENG_ConradVeidt";
            _textFontPath = "TextFont/" + "genkai-mincho";



        }
        ChangeTMPFont(_textFontPath);
    }

    //改字體
    void ChangeTMPFont(string path) {
        TMP_FontAsset Tmp_font = Resources.Load<TMP_FontAsset>(path);

        TextMeshProUGUI[] TMPUI = GameObject.FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI tmp in TMPUI) {
            //被標記為不須改字體的
            if (tmp.tag != "TextDontChangeFont")
            {
                tmp.font = Tmp_font;
            }
        }
    }



    private void Start()
    {

    }

    private void Update()
    {
        //偵測是否按下返回鍵=>按下就返回上一個畫面
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Scene _scene = SceneManager.GetActiveScene();

            if (_scene.name == "MainMenu") {

                SceneLoad(_scene.buildIndex - 1);
                Debug.Log("LodeScene Index: " + _scene.buildIndex);
            }

            if (_scene.name == "GamePlay_CustumeMode" || _scene.name == "GamePlay_PlayCustume"
                || _scene.name == "GamePlay_TypingMode" || _scene.name == "EditCustumeNode"
                || _scene.name == "EndingCounting") {
                Debug.Log("ESC");
                SceneLoad("MainScene");
            }
        }

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //進入場景時
    void OnSceneLoaded(Scene scence, LoadSceneMode mod) {

        //載入偏好設定
        if (GameObject.FindObjectOfType<SubMenuManager>() != null)
        {
            GameObject.FindObjectOfType<SubMenuManager>().SetPerfernce();
           
        }

        if (scence.name == "FirstScene") {
            //如果是第一次進入=>開頭
            if (IsFirstIn)
            {
                //SceneLoad(0);
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }

        //結束頁面=>跳播音樂
        if (scence.name== "StoryEnding") {
            GameObject.Find("BGMPlayer").GetComponent<AudioSource>().time = 20 ;
        }

        if (scence.name == "GamePlay_TypingMode" || scence.name == "GamePlay_PlayCustume") {
            if (audioSource != null)
            {
                GM.instance.audioSource.Stop();
                //設定難度
                if (GameObject.FindObjectOfType<TypingCellControl>() != null) {
                    if (scence.name== "GamePlay_TypingMode") { SetDifficultLevel(); }
                    if (scence.name == "GamePlay_PlayCustume") {
                        SetDifficultLevel_forCustume();
                    }
                }
                //設回時間
                GM.instance.audioSource.time = 0;

                //把GM的mixer設定成無聲頻道
                //GM.instance.Mute_audioSource.outputAudioMixerGroup = Mute_mixerGroup;
                GM.instance.GetComponent<AudioSource>().outputAudioMixerGroup = Mute_mixerGroup;

                //先倒數再開始=>播放無聲音樂(產生打擊點用)
                StartCoroutine(CountDown_coro(4));
                //播放有聲音樂
                try
                {
                    Debug.Log("Start Delay"+MuteSoundPlayDelay);
                    StartCoroutine(PlayRealSound(MuteSoundPlayDelay));
                }
                catch (System.Exception) { }
            }
        }

        if (scence.name == "GamePlay_CustumeMode" || scence.name == "EditCustumeNode") {

            GM.instance.audioSource.Stop();

            GM.instance.audioSource.time = 0;

            //把GM的mixer設定成有聲頻道
            //GM.instance.Mute_audioSource.outputAudioMixerGroup = Sound_mixerGroup;
            GM.instance.GetComponent<AudioSource>().outputAudioMixerGroup = Sound_mixerGroup;

            //先倒數再開始
            StartCoroutine(CountDown_coro(4));
        }

        //選單
        if (scence.name == "MainMenu") {
            //GM.instance.Mute_audioSource.outputAudioMixerGroup = Sound_mixerGroup;
            GM.instance.GetComponent<AudioSource>().outputAudioMixerGroup = Sound_mixerGroup;
        }

        if (scence.name== "MainScene") {
            GM.instance.audioSource.Stop();

            //處理升等:
            if (GM.playerData.CurrentExp>=GM.playerData.MaxExp) {
                LevelUP();
            }

            //存檔
            Save_LoadSystem.SavePlayerData(GM.playerData);

            Debug.Log("目前經驗"+GM.playerData.CurrentExp);
            //更新等級介面:
            GameObject.FindObjectOfType<EXPcontrol>().SetExpUIRing();
            GameObject.FindObjectOfType<EXPcontrol>().SetLevel_Text();
            GameObject.FindObjectOfType<EXPcontrol>().SetPercentage_text();
        }
    }



    //倒數
    IEnumerator CountDown_coro(int time) {
        GameObject cntLable = GameObject.FindGameObjectWithTag("CountDownLab").gameObject;
        if (cntLable == null) { StopCoroutine("CountDown_coro"); }
        cntLable.GetComponent<TextMeshProUGUI>().enabled = true;
        while (--time > 0) {
            cntLable.GetComponent<TextMeshProUGUI>().text = time.ToString();
            yield return new WaitForSeconds(1);
        }
        cntLable.GetComponent<TextMeshProUGUI>().enabled = false;
        audioSource.Play();
    }

    //播放有聲音樂
    IEnumerator PlayRealSound(float time) {
        if (GameObject.FindGameObjectWithTag("RealSoundGamePlayMusic") != null)
        {
            GameObject.FindGameObjectWithTag("RealSoundGamePlayMusic").GetComponent<AudioSource>().clip = GM.instance.audioSource.clip;
            yield return new WaitForSeconds(time);

            GameObject.FindGameObjectWithTag("RealSoundGamePlayMusic").GetComponent<AudioSource>().Play();
        }
    }

    public void SceneLoad(int level) {
        SceneManager.LoadScene(level);

    }
    public void SceneLoad(string levelName)
    {

        SceneManager.LoadScene(levelName);

    }

    //升等
    public void LevelUP() {
        GM.playerData.Level++;
        GM.playerData.CurrentExp = 0;
        //設定上限
        GM.playerData.MaxExp = (int)Mathf.Pow((GM.playerData.Level * 100), 1.15f) + 80;

        //UI更新資訊
        EXPcontrol _levelRing=GameObject.FindObjectOfType<EXPcontrol>();
        _levelRing.SetExpUIRing();
        _levelRing.SetLevel_Text();
        _levelRing.SetPercentage_text();
        
        //動畫?
    }

    

    //設定難度:
    public void SetDifficultLevel() {
        Debug.Log("難度設定OK");
        TypingCellControl _obj = GameObject.FindObjectOfType<TypingCellControl>();
        switch (GM.Difficult)
        {
            //Easy:
            case 0:
                _obj.BobbleLifetime = 2.5f;
                _obj.FreDeviation = 0.05f;
                _obj.GapTime = 1f;
                break;

            //Normal:
            case 1:
                _obj.BobbleLifetime = 2f;
                _obj.FreDeviation = 0.15f;
                _obj.GapTime = 0.25f;
                break;

            //Hard:
            case 2:
                _obj.BobbleLifetime = 1.5f;
                _obj.FreDeviation = 0.35f;
                _obj.GapTime = 0.15f;
                break;

            //Extreme:
            case 3:
                _obj.BobbleLifetime = 1.25f;
                _obj.FreDeviation = 0.85f;
                _obj.GapTime = 0.09f;
                break;
        }

        //4- (提示時間)*加速比例
        MuteSoundPlayDelay =4- 0.5f*( _obj.BobbleLifetime/BubbleOriginAnimaTime);
    }

    //自訂樂譜:
    public void SetDifficultLevel_forCustume() {
        Debug.Log("難度設定OK");
        PlayCustumeMode _obj = GameObject.FindObjectOfType<PlayCustumeMode>();

        switch (GM.Difficult)
        {
            //Easy:
            case 0:
                _obj.BobbleLifetime = 2.5f;
                
                break;

            //Normal:
            case 1:
                _obj.BobbleLifetime = 2f;
               
                break;

            //Hard:
            case 2:
                _obj.BobbleLifetime = 1.5f;
              
                break;

            //Extreme:
            case 3:
                _obj.BobbleLifetime = 1.25f;
             
                break;
        }

        //4+ (提示時間)*加速比例
        //MuteSoundPlayDelay = 4 + 0.5f * (_obj.BobbleLifetime / BubbleOriginAnimaTime);
        MuteSoundPlayDelay = 4 - 0.5f * (_obj.BobbleLifetime / BubbleOriginAnimaTime);

        //CustumeNodesStartDelay = MuteSoundPlayDelay - 4 +;
    }
}
