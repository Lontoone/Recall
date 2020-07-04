using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//任務類型:5
//條件: Music Time >_
//遊玩音樂時間大於一定數
public class Mission5_LimitMusicTime : MissionClass {

    public Mission5_LimitMusicTime(){
        Type = 5;
        Describe = "Music length is greater than ";
    }

    public override void Awake()
    {
        base.Awake();

        //音樂長度要求
        TotalScoreRequirment = Random.Range(180, 480);

        //翻譯敘述
        MissionManager.SetMissionLanguage(this);
    }

    public override bool CheckRequirement()
    {
        if (GM.instance.audioSource.time >= TotalScoreRequirment)
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
