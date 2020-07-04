using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//進入畫面管理

public class OpenSceneManager : MonoBehaviour {

    //開門聲
    public AudioSource DoorOpenAudio;

	void Update () {
        //點擊任一點進入下個畫面(主畫面)
        if (Input.touchCount>0 && Input.GetTouch(0).phase==TouchPhase.Began) {

            DoorOpenAudio.Play();
            Invoke("NextScene",0.5f);
        }
	}


    void NextScene() {

        SceneManager.LoadScene("MainScene");
    }

    //檢查OBB檔是否完整:
    void CheckObb() {
        //string paht=System.o
    }
}
