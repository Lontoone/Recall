using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//轉動紙張素材
public class PaperRoller : MonoBehaviour {

    public enum RollMode { Paper,letter}
    public RollMode rollMode = new RollMode();

    Material _paper;

    //檢查有沒有pin被按下
    TypingPinControl[] Pins;
    private void Start()
    {
        Pins = GameObject.FindObjectsOfType<TypingPinControl>();

        if (rollMode == RollMode.Paper)
        {
            //自身的材質
            _paper = gameObject.GetComponent<SpriteRenderer>().material;
        }
    }


    void Update () {

        foreach (TypingPinControl _pin in Pins)
        {
            if (_pin.IsPressed)
            {
                //轉紙張
                if (rollMode == RollMode.Paper)
                {
                    float Y_offset = _paper.mainTextureOffset.y + Time.deltaTime;

                    _paper.SetTextureOffset("_MainTex", new Vector2(1, Y_offset));
                    break;
                }

                //符號跟著紙張轉
                else if (rollMode==RollMode.letter ) {

                    gameObject.transform.position=new Vector2(gameObject.transform.position.x , gameObject.transform.position.y+Time.deltaTime*5) ;
                }
            }
        }
    }


}
