using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//經驗值
/*
 * 最大經驗值=(等級*100)^1.15+80
 * 每場遊戲經驗: 5*(總分)^0.2*等級
 * 任務經驗值獎勵=x^0.75*20+25
 */
public class EXPcontrol : MonoBehaviour {

    //EXP圖示
    public GameObject EXP_UI_ring;
    public TextMeshProUGUI percentage_text;
    public TextMeshProUGUI Level_text;

    private void Start()
    {
        SetExpUIRing();
        SetLevel_Text();
        SetPercentage_text();
        Debug.Log(GM.playerData.CurrentExp+"/"+GM.playerData.MaxExp);
    }

    //設定exp的UI
    public void SetExpUIRing() {
        //經驗比率
        float percentage = GM.playerData.CurrentExp / (float)GM.playerData.MaxExp;
        EXP_UI_ring.GetComponent<Image>().fillAmount =percentage;
    }

    public void SetPercentage_text() {
        float percentage =( GM.playerData.CurrentExp / (float)GM.playerData.MaxExp);
        percentage_text.text = percentage.ToString("p1");
    }

    public void SetLevel_Text() {
        Level_text.text = GM.playerData.Level.ToString();
    }
    
}
