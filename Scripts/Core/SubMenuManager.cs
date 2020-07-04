using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;


//小選單
public class SubMenuManager : MonoBehaviour {

    public TextMeshProUGUI bpm_text;

    [Header("OpenMetronome指定")]
    public GameObject bpmSlider;

    //
    [Tooltip("打字機_進入窗口")]
    public GameObject BoxSet;

    [Header("音效")]
    public AudioMixer audioMixer;

    [Header("左右選單開關")]
    public GameObject LeftWindow;
    public GameObject RightWindo;

    [Space(height: 10f)]

    public Image MetronomeImg, VibrateImg;
    [Space(height: 10f)]

    [Tooltip("節奏器開關圖片")]
    public Sprite Metronome_active, Metronome_deactive;

    [Space(height: 20f)]

    [Tooltip("震動開關圖片")]
    public Sprite Vibrate_active, Vibrate_deactive;

    [Header("音效Group")]
    public Slider Music, BGM, Effect, Metronome;

    //資訊頁面
    [Tooltip("音效/資訊/credit")]
    public GameObject[] SubMenuInfo;


    //開啟/關閉震動
    public void OpenVibration() {
        
        GM.playerPreference.IsVibrate = !GM.playerPreference.IsVibrate;
        ChangeImage(GM.playerPreference.IsVibrate, VibrateImg, Vibrate_active, Vibrate_deactive);
    }

    //開啟/關閉節拍器
    public void OpenMetronome() {
        if (bpmSlider==null) { return; }

        GM.playerPreference.IsMetronome = !GM.playerPreference.IsMetronome;
        ChangeImage(GM.playerPreference.IsMetronome, MetronomeImg, Metronome_active, Metronome_deactive);

        //如果打開節拍器=>show Slider(調數值)
        if (GM.playerPreference.IsMetronome)
        {
            bpmSlider.SetActive(true);
        }
        else {
            //關閉Metronome
            bpmSlider.SetActive(false);
        }
    }

    //改變節拍器bpm
    public void Slide_bpm(float bpm) {

        if (bpm_text.text==null) { return; }
        GM.playerPreference.Metronome_bpm = (int)bpm;

        bpm_text.text = GM.playerPreference.Metronome_bpm.ToString();
    }


    //換圖片
    void ChangeImage(bool _IsTrue,Image _img , Sprite _act,Sprite _deAct)
    {
        if (_IsTrue)
        {
            //gameObject.transform.GetComponentInChildren<Image>().sprite = Active_img;
            _img.sprite = _act;
        }
        else
        {
            //gameObject.transform.GetComponentInChildren<Image>().sprite = UnActive_img;
            //gameObject.GetComponent<Image>().sprite = UnActive_img;
            _img.sprite = _deAct;
        }
    }

    //左邊頁面
    public void OpenLeftWindow() {
        Animator animator = LeftWindow.GetComponent<Animator>();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Menu_LeftWindow_Close"))
        {
            animator.Play("Menu_LeftWindow_Open");
        }

        else
        {
            animator.Play("Menu_LeftWindow_Close");
        }
    }

    //右邊頁面->存讀玩家偏好
    public void OpenRightWindow()
    {
        Animator animator = RightWindo.GetComponent<Animator>();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Menu_RightWindow_Close"))
        {
            animator.Play("Menu_RightWindow_Open");
            if (BoxSet != null)
            {
                BoxSet.GetComponent<Image>().raycastTarget = false;
            }
            //設偏好
            SetPerfernce();
        }

        else
        {
            animator.Play("Menu_RightWindow_Close");
            if (BoxSet != null)
            {
                BoxSet.GetComponent<Image>().raycastTarget = true;
            }

            //存偏好
            Save_LoadSystem.SavePlayerPreferenceData(GM.playerPreference);

        }
    }

    //增加BPM
    public void PluseBPM() {
        GM.playerPreference.Metronome_bpm++;
        bpmSlider.GetComponent<Slider>().value = GM.playerPreference.Metronome_bpm;
        bpm_text.text = GM.playerPreference.Metronome_bpm.ToString();
        
    }

    //減少bpm
    public void MinusBPM() {
        bpm_text.text = GM.playerPreference.Metronome_bpm.ToString();
        GM.playerPreference.Metronome_bpm--;
        bpmSlider.GetComponent<Slider>().value = GM.playerPreference.Metronome_bpm;
    }

    //回主頁面
    public void Icon_Exit() {
        Time.timeScale = 1;
        GM.instance.audioSource.Stop();
        GM.instance.SceneLoad("MainScene");
    }

    [Header("More選項")]
    public GameObject Icon_more_paper;

    //更多 按鈕
    public void Icon_More() {
        if (Icon_more_paper.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Icon_MorePaper_In"))
        {
            Icon_more_paper.GetComponent<Animator>().Play("Icon_MorePaper_Out");
        }
        else {
            Icon_more_paper.GetComponent<Animator>().Play("Icon_MorePaper_In");
        }
       

    }

    //**************音效*****************
    #region
    public void SetVolume_Music(float v) {
        GM.playerPreference.GamePlayMusic_V = v;
        audioMixer.SetFloat("Music",v);
    }

    public void SetVolume_Effect(float v)
    {
        GM.playerPreference.SoundEffect_V = v;
        audioMixer.SetFloat("Effect", v);
    }
    public void SetVolume_BGM(float v)
    {
        GM.playerPreference.MainBGM_V = v;
        audioMixer.SetFloat("BGM", v);
    }
    public void SetVolume_Metronome(float v)
    {
        GM.playerPreference.Metronome_V = v;
        audioMixer.SetFloat("Metronome", v);
    }
    #endregion
    //**********************************


    //設定偏好
    public void SetPerfernce() {
        //讀偏好:
        GM.PlayerPreference _temp = Save_LoadSystem.LoadPlayerPreferenceData();
        if (_temp != null)
        {

            GM.playerPreference = _temp;

            //難度
            //GM.playerPreference.Difficult = _temp.Difficult;

            //設定值
            ChangeImage(GM.playerPreference.IsMetronome, MetronomeImg, Metronome_active, Metronome_deactive);
            ChangeImage(GM.playerPreference.IsVibrate, VibrateImg, Vibrate_active, Vibrate_deactive);


            //音效:
            SetVolume_BGM(GM.playerPreference.MainBGM_V);
            SetVolume_Effect(GM.playerPreference.SoundEffect_V);
            SetVolume_Music(GM.playerPreference.GamePlayMusic_V);
            SetVolume_Metronome(GM.playerPreference.Metronome_V);

            Music.value = GM.playerPreference.GamePlayMusic_V;
            BGM.value = GM.playerPreference.MainBGM_V;
            Effect.value = GM.playerPreference.SoundEffect_V;
            Metronome.value = GM.playerPreference.Metronome_V;

            bpmSlider.GetComponent<Slider>().value = GM.playerPreference.Metronome_bpm;
            bpm_text.text = GM.playerPreference.Metronome_bpm.ToString();
            if (GM.playerPreference.IsMetronome)
            {
                bpmSlider.SetActive(true);
            }
            else
            {
                //關閉Metronome
                bpmSlider.SetActive(false);
            }

        }
    }

    public void OpenSoundInfo() {
        SubMenuInfo[0].SetActive(true);
        SubMenuInfo[1].SetActive(false);
        SubMenuInfo[2].SetActive(false);
        //SubMenuInfo[3].SetActive(false);
    }

    public void OpenDevInfo()
    {
        SubMenuInfo[0].SetActive(false);
        SubMenuInfo[1].SetActive(true);
        SubMenuInfo[2].SetActive(false);
        //SubMenuInfo[3].SetActive(false);
    }

    public void OpenCreditInfo()
    {
        SubMenuInfo[0].SetActive(false);
        SubMenuInfo[1].SetActive(false);
        SubMenuInfo[2].SetActive(true);
        //SubMenuInfo[3].SetActive(false);
    }
    public void OpenDonationInfo() {
        SubMenuInfo[0].SetActive(false);
        SubMenuInfo[1].SetActive(false);
        SubMenuInfo[2].SetActive(false);
        //SubMenuInfo[3].SetActive(true);
    }

    public void OpenURL(string url) {
        Application.OpenURL(url);
    }
}
