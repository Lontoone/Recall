using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//任務類型:3
//條件: Total Perfect>_
//perfect數量總和
public class Mission3_TotalPerfect : MissionClass {

    public Mission3_TotalPerfect(){
        Type = 3;

        Describe = "Perfect quantity reaches";

        Requirment = 1;

    }


    public override void Awake()
    {
        base.Awake();

        //總分要求
        Requirment = Random.Range(500, 3500);

        //翻譯敘述
        MissionManager.SetMissionLanguage(this);
    }

    public override bool CheckRequirement()
    {
        CurrentProgress += ComboControl.Weighting[0];

        if (CurrentProgress >= Requirment)
        {
            return true;
        }
        else {
            return false;
        }
    }




}
