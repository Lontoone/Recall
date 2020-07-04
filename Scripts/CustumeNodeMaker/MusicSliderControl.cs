using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;



//音樂進度條控制
public class MusicSliderControl : MonoBehaviour {

    public Slider MusicBar;

    public TextMeshProUGUI timerText;

    public float CameraScrollSpeed;

    [HideInInspector]
    //每0.01秒x位移值
    public float Share_XoffsetPer_p01Sec;
  
    /*
     每0.01秒 移動 x移動 1
         
         */

    private void Start()
    {
        MusicBar.maxValue = GM.instance.audioSource.clip.length;

    }

    //主動調整音樂進度
    public void OnValueChange(float _time) {
        if (EventSystem.current.currentSelectedGameObject== gameObject) {

            GM.instance.audioSource.time = _time;
            
            EventSystem.current.SetSelectedGameObject(null);
        }

        //移動攝影機:
        MoveCamera(CameraScrollSpeed);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            MusicBar.value = GM.instance.audioSource.time;
        }
        
        //計時器 文字:
        string _timer = (GM.instance.audioSource.time / 60).ToString("00") +":"+  (GM.instance.audioSource.time%60).ToString(".00");
        timerText.text = _timer;
    }


    void MoveCamera(float speed) {
        Vector3 _goalPos = Camera.main.transform.position;

        //_goalPos.x = Mathf.Round(MusicBar.value*100)/100 ; //取小數第2位的時間點

        _goalPos.x = MusicBar.value*speed;

        Share_XoffsetPer_p01Sec = Mathf.Abs(Camera.main.transform.position.x-_goalPos.x);
        //Debug.Log(Share_XoffsetPer_p01Sec); //=0.2多
        Camera.main.transform.position =_goalPos;
    }


}
