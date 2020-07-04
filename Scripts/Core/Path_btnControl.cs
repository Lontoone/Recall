using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

using UnityEngine.SceneManagement;

public class Path_btnControl : MonoBehaviour {

    MyNativeFolderBrowser myNativeFolderBrowser;

    public GameObject LoadBarPrefab;

    private void Start()
    {
        myNativeFolderBrowser = GameObject.FindGameObjectWithTag("PathControl").GetComponentInParent<MyNativeFolderBrowser>();

        animator = GameObject.Find("Menu").GetComponent<Animator>();
    }

    //點擊時呼叫
    public void OnPahtclick() {

        //傳回路徑 並打開資料夾
        myNativeFolderBrowser.FolderPath = transform.name;

        //如果不是音樂檔案=> 進入資料夾
        if (!Path.HasExtension(myNativeFolderBrowser.FolderPath))
        {
            myNativeFolderBrowser.ShowFolder_Window();
        }
        else if (Path.GetExtension(myNativeFolderBrowser.FolderPath) == ".mp3" ||
            Path.GetExtension(myNativeFolderBrowser.FolderPath) == ".aif" ||
            Path.GetExtension(myNativeFolderBrowser.FolderPath) == ".wav"||
            Path.GetExtension(myNativeFolderBrowser.FolderPath) == ".ogg"
            )
        {
            //"file://" +
            string link = "file://" + myNativeFolderBrowser.FolderPath;
            WWW www = new WWW(link);
            Debug.Log("www:"+www.url);

            if (GM.instance.GetComponent<AudioSource>()==null) {
                GM.instance.gameObject.AddComponent<AudioSource>();
            }

            GM.instance.audioSource = GM.instance.GetComponent<AudioSource>();


            //讀取=>產生進度條
            Instantiate(LoadBarPrefab, new Vector3(0, 0, 0),Quaternion.identity);
            StartCoroutine(LoadingProcess(www));
            while (!www.isDone) { }
            GM.instance.audioSource.clip = www.GetAudioClip();

            //開啟info頁面
            ShowclipInfo(www);

        }
        Debug.Log(myNativeFolderBrowser.FolderPath);
    }

    //回上一層
    public void GoBack() {
        
        myNativeFolderBrowser = GameObject.FindGameObjectWithTag("PathControl").GetComponentInParent<MyNativeFolderBrowser>();
        if (Path.GetDirectoryName(myNativeFolderBrowser.FolderPath) != null) {
            myNativeFolderBrowser.FolderPath = Path.GetDirectoryName(myNativeFolderBrowser.FolderPath);
        }
        myNativeFolderBrowser.ShowFolder_Window();
    }

    //歌曲長度與最佳成績
    TextMeshProUGUI ClipDuration, BestScore, BestPercentage, Title;
    Animator animator;
    public void ShowclipInfo(WWW www) {

        GameObject InfoPanel = GameObject.Find("InfoPanel");

        BestScore = InfoPanel.transform.Find("BestScore Text").GetComponent<TextMeshProUGUI>();
        BestPercentage=InfoPanel.transform.Find("BestPercentage Text").GetComponent<TextMeshProUGUI>();

        //讀取資料------------------------------------------------------
        GM.instance.TempSongData = Save_LoadSystem.LoadSongData(Path.GetFileNameWithoutExtension(www.url));
        Debug.Log("Load Item:"+ Path.GetFileName(www.url));
        Debug.Log(GM.instance.TempSongData);

        //先前已有紀錄=> 讀進資料
        if (GM.instance.TempSongData != null)
        {
            
            BestScore.text = GM.instance.TempSongData.BestScore.ToString();
            BestPercentage.text = GM.instance.TempSongData.BestPercentage.ToString("p1");
        }
        else {
            GM.instance.TempSongData = new SongClass();
            BestScore.text = "0";
            BestPercentage.text = "0%";
        }

        GM.instance.TempSongData.SongFilePath   = www.url;
        GM.instance.TempSongData.SongName       = Path.GetFileNameWithoutExtension(www.url);
        //--------------------------------------------------------------


        //Title:
        Title       = InfoPanel.transform.Find("Title Text").GetComponent<TextMeshProUGUI>();
        Title.text  = Path.GetFileNameWithoutExtension(www.url.ToString());

        //音樂長度
        ClipDuration        = InfoPanel.transform.Find("Duration Text").GetComponent<TextMeshProUGUI>();
        ClipDuration.text   = ((int)GM.instance.GetComponent<AudioSource>().clip.length/60).ToString() +" : "+ ((int)GM.instance.GetComponent<AudioSource>().clip.length % 60).ToString();

        //預覽音樂=>從音樂一半開始播放
        if (GM.instance.audioSource.clip==null) {
            Debug.LogError("GM.instance.audioSource.clip =NULL");
            return;
        }
        try
        {
            GM.instance.audioSource.Play();
            GM.instance.audioSource.time = GM.instance.audioSource.clip.length / 2;
        }
        catch (System.Exception ex) { }
        
        //動畫=> 打開顯示資料的面板
        animator.Play("Menu_ShowInfo");

        //若有自訂譜: 出現遊玩選項
        if (CheckCustumeFile(Path.GetFileNameWithoutExtension(www.url)))
        {
            GameObject btn=GameObject.FindGameObjectWithTag("HideBtn");
            btn.GetComponent<Image>().enabled = true;
            btn.GetComponent<Button>().enabled = true;
            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
        }
        else {
            GameObject btn = GameObject.FindGameObjectWithTag("HideBtn");
            btn.GetComponent<Image>().enabled = false;
            btn.GetComponent<Button>().enabled = false;
            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
        }
    }

    //檢查是否有自訂譜過
    public bool CheckCustumeFile(string _songName) {
        string path = Application.persistentDataPath + "/CustumeEditNode/" + _songName + ".node";
        return File.Exists(path);
    }


    public void PlayAnima(string animName) {
        animator.Play(animName);
    }

    IEnumerator LoadingProcess(WWW www) {
        Image bar           = GameObject.FindGameObjectWithTag("LoadProcessBar").GetComponent<Image>();
        TextMeshProUGUI text= GameObject.FindGameObjectWithTag("LoadProcessText").GetComponent<TextMeshProUGUI>();
        
        while (!www.isDone)
        {
            bar.fillAmount = www.progress ;
            Debug.Log(bar.fillAmount);
            text.text = www.progress.ToString("P1") ;

            Debug.Log(string.Format("Downloaded {0:P1}", www.progress));
            yield return new WaitForSeconds(.01f);
        }

        //所有存在的讀取條 (避免點太快產生2個而留下一個)
        GameObject[] _bar=GameObject.FindGameObjectsWithTag("LoadPage");
        for (int i=0;i<_bar.Length;i++) {
           Destroy( _bar[i]);
        }
        
        Debug.Log("Done");
    }
}
