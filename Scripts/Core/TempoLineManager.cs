using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//依照bpm產生小節的線條 :報廢
public class TempoLineManager : MonoBehaviour {
    /*
     * 60  bpm= 1分鐘60下= 60 個四分音符 
     * 120 bpm= 0.5秒1下 = 1個四分音符 0.5秒 = 2秒(4個4分音符)1小節
     * _goalPos.x = MusicBar.value *speed =0.2 (每0.01秒移動0.2)
     * */

    //小節線物件
    public GameObject tempoLine_Prefab;

    //基準點
    public GameObject benchmark;

    MusicSliderControl sliderControl;

    Vector3 _offsetX=new Vector3(0,0,0);

    //音樂播放後再次重製
    bool _hasReSet=false;

    private void Update()
    {
        if (sliderControl.Share_XoffsetPer_p01Sec !=0) {
            _offsetX.x = sliderControl.Share_XoffsetPer_p01Sec;
        }

        //音樂播放後重製
        if (!_hasReSet && sliderControl.Share_XoffsetPer_p01Sec != 0) {

            ResetTempoLine();
            _hasReSet = true;
            ArrangeLine();
        }


        if (_hasReSet &&canReFrash) {
            canReFrash = false;
            StartCoroutine(reFrash());
        }

    }
    private void Start()
    {
        sliderControl = GameObject.FindObjectOfType<MusicSliderControl>();

        ArrangeLine();
    }

    //產生並排列
    public void ArrangeLine() {

        //目前音樂所對應的位置 //每0.01秒x位移值

        if (_offsetX.x == 0)
        {
            _offsetX.x = 0.2f;
        }
        Debug.Log(sliderControl.Share_XoffsetPer_p01Sec);
        //_offsetX.x *100 => 1秒x位移20

        //每小節位移
        _offsetX.x = sliderControl.Share_XoffsetPer_p01Sec*100 * (GM.playerPreference.Metronome_bpm / 60);

        //要產生的小節數量=歌曲總長度*(bpm/60)
        int _creatAmount =(int)GM.instance.audioSource.clip.length*(GM.playerPreference.Metronome_bpm/60)+1;

        for (int i = 0; i < _creatAmount; i++)
        {
            //生產位置
            Vector3 _goalPos =new Vector3(0,0,0);
            _goalPos.x += _offsetX.x * i;
            
            GameObject tempLine =Instantiate(tempoLine_Prefab, _goalPos, Quaternion.identity,this.transform);
            tempLine.transform.Rotate(0,0,90);
        }
    }

    //刷新小節線
    public void ResetTempoLine() {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);

        }

        ArrangeLine();
    }

    bool canReFrash = true;
    IEnumerator reFrash() {

        ResetTempoLine();
        _hasReSet = true;
        ArrangeLine();
        yield return new WaitForSeconds(0.01f);
        canReFrash = true;

    }
	
	
}
