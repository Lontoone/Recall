using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//任務類型:1
//條件: Full Combo
public class Mission1_FullCombo : MissionClass {

    public Mission1_FullCombo() {
        Describe = "Full Combo";

        Requirment = 1;

        Type = 1;
    }
    public Mission1_FullCombo(string _descirbe, int _requirment, int _rewardExp) {
        Describe = _descirbe;
        Requirment = _requirment;
        ExpReward = _rewardExp;
        Type = 1;
    }
    
    public override bool CheckRequirement()
    {
        if (ComboControl.MissCombo == 0)
        { 
            //進度+1
            CurrentProgress++;
        }
        
        //判斷是否full combo:
        if (CurrentProgress >= Requirment) {
            return true;
        }
        
        else {
            return false;
        }
        
    }

    public override void Reward()
    {
        
        base.Reward();
        Debug.Log("Complete!");
    }
}
