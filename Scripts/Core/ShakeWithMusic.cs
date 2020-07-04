using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//跟著音樂改變型態
public class ShakeWithMusic : MonoBehaviour {

    //縮放比例
    public float ScaleRate=5;

    [HideInInspector]
    public Vector3 OtherOriginVec3;

    //紀錄原始大小
    private Vector3 _originScale;
    private void Start()
    {
        _originScale = this.transform.localScale;

    }

    //晃動的模式
    public enum ShakeMode { Camera, Scale,ScaleWithOrigin };
    public ShakeMode shakeMode = new ShakeMode();

    private void Update()
    {
       
        try
        {
            if (shakeMode==ShakeMode.Camera) { ShakeCamera();}
            if (shakeMode==ShakeMode.Scale) { ShakeScale(); }
            if (shakeMode==ShakeMode.ScaleWithOrigin) { ShakeScale(OtherOriginVec3); }
        
        }
        catch (System.Exception ex) {
            Debug.LogWarning(ex);
        }

    }

    void ShakeScale() {
        Vector3 Amplitu = new Vector3(AudioPeer._AmplitudeBuffer, AudioPeer._AmplitudeBuffer, _originScale.z);
        //隨著振幅改變大小:
        if (!float.IsNaN(Amplitu.x) && !float.IsNaN(Amplitu.y))
        {
            transform.localScale = _originScale + Amplitu * ScaleRate;
        }
    }

    //跟著別人傳進得初始Scale值縮放
    void ShakeScale(Vector3 originVec3)
    {
        Vector3 Amplitu = new Vector3(AudioPeer._AmplitudeBuffer, AudioPeer._AmplitudeBuffer, _originScale.z);
        //隨著振幅改變大小:
        if (!float.IsNaN(Amplitu.x) && !float.IsNaN(Amplitu.y))
        {
            transform.localScale = originVec3 + Amplitu * ScaleRate;
        }
    }


    void ShakeCamera() {
        if (!float.IsNaN(AudioPeer._AmplitudeBuffer))
        {
            Camera.main.orthographicSize = 5 + AudioPeer._AmplitudeBuffer*ScaleRate;
        }
    }




}
