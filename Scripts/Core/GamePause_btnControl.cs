using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GamePause_btnControl : MonoBehaviour {

    //暫停/繼續按鈕:
    public Sprite Continut_sprite,Pause_sprite;

    public void Onclick() {
        //繼續=>暫停
        if (gameObject.GetComponent<Image>().sprite == Continut_sprite)
        {
            gameObject.GetComponent<Image>().sprite = Pause_sprite;
            GM.instance.audioSource.UnPause();
            if (GameObject.FindGameObjectWithTag("RealSoundGamePlayMusic") != null)
            {
                GameObject.FindGameObjectWithTag("RealSoundGamePlayMusic").GetComponent<AudioSource>().UnPause();
            }
            Time.timeScale = 1;
        }

        //暫停=>繼續
        else if(gameObject.GetComponent<Image>().sprite == Pause_sprite)
        {
            gameObject.GetComponent<Image>().sprite = Continut_sprite;
            GM.instance.audioSource.Pause();
            if (GameObject.FindGameObjectWithTag("RealSoundGamePlayMusic") != null)
            {
                GameObject.FindGameObjectWithTag("RealSoundGamePlayMusic").GetComponent<AudioSource>().Pause();
            }
            Time.timeScale = 0;
        }
    }

}
