using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//翻下一頁: ex: tutorial頁
public class NextPage : MonoBehaviour {

    //頁面物件
    public GameObject[] Pages;

    int pageCounter = 0;

    public void GoNextPage() {
        if (pageCounter==Pages.Length-1) { return; }
        //關閉上一頁
        Pages[pageCounter].gameObject.SetActive(false);
        pageCounter++;
        Pages[pageCounter].SetActive(true);
    }

    //回上一頁
    public void GoLastPage()
    {
        if (pageCounter == 0) { return; }
        //關閉上一頁
        Pages[pageCounter].gameObject.SetActive(false);
        pageCounter--;
        Pages[pageCounter].SetActive(true);
    }

    public void ClosePage() {
        gameObject.SetActive(false);
    }


    public void OpenPage() {
        gameObject.SetActive(true);
    }







}
