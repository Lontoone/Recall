using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ComboControl : MonoBehaviour {

    public static int CurrtenCombo, Combototal,HighestCombo,MissCombo ,TotalScore;

    //Perfect/ good/ bad
    public static int[] Weighting = { 0, 0, 0 };

    public GameObject Particle_Prefab;

    public TextMeshProUGUI tmp;

    //初始的Scale值
    private Vector3 originScale;

    private void Awake()
    {
        originScale = tmp.rectTransform.localScale;
    }

    //前一個數值
    int preComboCount=0;

    private void FixedUpdate()
    {
        //數值改變時=>產生特效
        if (CurrtenCombo!=preComboCount) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(tmp.transform.position);
            GameObject comboEffect=Instantiate(Particle_Prefab,pos,Quaternion.identity);
            Destroy(comboEffect, 1f);
            preComboCount = CurrtenCombo;
        }
        //展示次數
        tmp.text = CurrtenCombo.ToString();

        //紀錄最高
        HighestCombo = (HighestCombo > CurrtenCombo) ? HighestCombo : CurrtenCombo;

        /*
        int tempCombo = (CurrtenCombo <= 0) ? 1 : CurrtenCombo;

        Vector3 tmpScaleTo = new Vector3(tempCombo/10,tempCombo/10,0);

        tmp.rectTransform.localScale = originScale +tmpScaleTo/5;

        if (tmp.GetComponent<ShakeWithMusic>()!=null) {
            tmp.GetComponent<ShakeWithMusic>().OtherOriginVec3 = tmp.rectTransform.localScale;
        }*/
        
    }







}
