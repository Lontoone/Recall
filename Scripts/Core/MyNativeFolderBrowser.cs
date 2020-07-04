using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;
//“~”表示Web 應用程序根目錄，“/”也是表示根目錄，“../”表示當前目錄的上一級目錄，“./”表示當前目錄
//資料夾瀏覽器
public class MyNativeFolderBrowser : MonoBehaviour {

    public Button Btn_Prefab;

    public string FolderPath= @"C:\";

    // 所有資料夾
    List<string> FolderList;

    private void Start() {

        if (Application.platform == RuntimePlatform.Android) {

            //FolderPath = @"/sdcard/Music";
            FolderPath = (Directory.Exists(@"/sdcard/Music")) ? @"/sdcard/Music" : @"/sdcard/";
            ShowFolder_Window();
        }
        if (Application.platform==RuntimePlatform.WindowsPlayer) {
            FolderPath = @"C:\";
            ShowFolder_Window();
        }
        if (Application.platform==RuntimePlatform.WindowsEditor) {
            FolderPath = @"C:\";
            ShowFolder_Window();
        }

    }

    //Window作業平台:
    public void ShowFolder_Window() {
       
        // 所有資料夾
        //List<string> FolderList = Directory.GetFileSystemEntries(FolderPath).ToList();
        FolderList = Directory.GetDirectories(FolderPath).ToList();
        // Put all mp3 files in root directory into array.
        // ... This is case-insensitive.(不分大小寫)
        List<string> MP3List = Directory.GetFiles(FolderPath, "*.mp3").ToList();
        List<string> AifList = Directory.GetFiles(FolderPath, "*.aif").ToList();
        List<string> WAVList = Directory.GetFiles(FolderPath, "*.wav").ToList();
        List<string> OGGList = Directory.GetFiles(FolderPath, "*.ogg").ToList();
        //合併
        FolderList.AddRange(MP3List);
        FolderList.AddRange(AifList);
        FolderList.AddRange(WAVList);
        FolderList.AddRange(OGGList);


        //先清空ScrowView
        for (int i=0;i<transform.Find("Content").childCount;i++) {
            Destroy(transform.Find("Content").GetChild(i).gameObject);
        }

        // 展示資料夾
        for (int i=0;i<FolderList.Count;i++) {
            //ScrolView的排列
            Vector3 nextBtnPos = new Vector3(0,- Btn_Prefab.GetComponent<RectTransform>().rect.height * i- Btn_Prefab.GetComponent<RectTransform>().rect.height/2, 0);

            //產生按鈕
            Button path_btn = Instantiate(Btn_Prefab, transform.Find("Content"));
            path_btn.transform.position = nextBtnPos;
            //設定位置
            path_btn.transform.localPosition = nextBtnPos;

            path_btn.name= FolderList[i].ToString();
            //文字
            path_btn.GetComponentInChildren<TextMeshProUGUI>().text = Path.GetFileName(FolderList[i].ToString());
        }

        transform.Find("Content").GetComponent<RectTransform>().sizeDelta = new Vector2(transform.Find("Content").GetComponent<RectTransform>().sizeDelta.x
                                                                                , Btn_Prefab.GetComponent<RectTransform>().rect.height*FolderList.Count);

    }

    //搜尋歌單
    public void SearchFile(string _searchText) {
        //搜尋結果
        List<string> searchResult=new List<string>();

        searchResult = FolderList.FindAll(s=>s.Contains(_searchText));


        //先清空ScrowView
        for (int i = 0; i < transform.Find("Content").childCount; i++)
        {
            Destroy(transform.Find("Content").GetChild(i).gameObject);
        }

        // 展示資料夾
        for (int i = 0; i < searchResult.Count; i++)
        {
            //ScrolView的排列
            Vector3 nextBtnPos = new Vector3(0, -Btn_Prefab.GetComponent<RectTransform>().rect.height * i - Btn_Prefab.GetComponent<RectTransform>().rect.height / 2, 0);

            //產生按鈕
            Button path_btn = Instantiate(Btn_Prefab, transform.Find("Content"));
            path_btn.transform.position = nextBtnPos;
            //設定位置
            path_btn.transform.localPosition = nextBtnPos;

            path_btn.name = searchResult[i].ToString();
            //文字
            path_btn.GetComponentInChildren<TextMeshProUGUI>().text = Path.GetFileName(searchResult[i].ToString());
        }

        transform.Find("Content").GetComponent<RectTransform>().sizeDelta = new Vector2(transform.Find("Content").GetComponent<RectTransform>().sizeDelta.x
                                                                                , Btn_Prefab.GetComponent<RectTransform>().rect.height * searchResult.Count);

    }


}
