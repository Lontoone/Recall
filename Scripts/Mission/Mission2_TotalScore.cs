using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//任務種類2: 總分限定
public class Mission2_TotalScore : MissionClass {
    public Mission2_TotalScore() {
        Describe = "TotolScore >"+ TotalScoreRequirment.ToString();
        Type = 2;
    }
    public override void Awake()
    {
        base.Awake();
        if (TotalScoreRequirment == 0)
        {
            TotalScoreRequirment = Random.Range(2500, 5500);
        }

        //Describe = "TotolScore >" + TotalScoreRequirment.ToString();
        MissionManager.SetMissionLanguage(this);
    }
    public override bool CheckRequirement()
    {
        Debug.Log(ComboControl.TotalScore);
        if (ComboControl.TotalScore >= TotalScoreRequirment)
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
