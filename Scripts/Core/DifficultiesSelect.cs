using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//選擇難度
public class DifficultiesSelect : MonoBehaviour {

    /*中文:
     * 清晰的
     * 熟悉的
     * 破散的
     * 遺忘的
     * */

    /*英文:
     * cleared
     * familiar
     * scattered
     * forgotten
     */
    string[] Difficulties = {"Easy","Normal","Hard","Extreme"};
    public TextMeshProUGUI text;
	
	void Start () {
       
        //設語言
        if (GM.Language == "Che")
        {
            Difficulties =new string[]{ "清晰", "熟悉", "破散", "遺忘" };
        }
        else {
            Difficulties = new string[] { "cleared", "familiar", "scattered", "forgotten" };
        }

        SetDifficult();

    }

    //設定難度
    public void SetDifficult() {
        text.text = Difficulties[GM.Difficult];
    }

    public void Pluse() {
        //GM.playerPreference.Difficult=(GM.playerPreference.Difficult>2)? 0:GM.playerPreference.Difficult++;
        //GM.playerPreference.Difficult =Mathf.Abs(GM.playerPreference.Difficult+1)%4;
        GM.Difficult = (GM.Difficult >= 3) ? 3 : GM.Difficult + 1;
        SetDifficult();
    }

    public void Minus() {
        //GM.playerPreference.Difficult = (GM.playerPreference.Difficult < 1 ) ? 3 : GM.playerPreference.Difficult--;
        GM.Difficult = (GM.Difficult <= 0) ? 0 :GM.Difficult-1;
        SetDifficult();
    }
	
}
