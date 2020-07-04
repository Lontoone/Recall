using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//任務類型:4
//條件: Total Bad>_
//Bad數量小於一定數
public class Mission4_LessBad : MissionClass
{
    public Mission4_LessBad() {
        Type = 4;

        Describe = "Bad quantity is less than";

    }

    public override void Awake()
    {
        base.Awake();

        //總次數要求
        TotalScoreRequirment = Random.Range(15, 50);

        //翻譯敘述
        MissionManager.SetMissionLanguage(this);
    }

    public override bool CheckRequirement()
    {
        Debug.Log(ComboControl.Weighting[2]);
        if (ComboControl.Weighting[2] <= TotalScoreRequirment)
        {
            CurrentProgress++;
        }

        //判斷
        if (CurrentProgress >= Requirment)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

}
