using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome_bpmControl : MonoBehaviour {

    //節拍器音效
    public AudioSource beat_source;

    bool nextBPM = true;

    private void Update()
    {
        //節奏器
        if (GM.playerPreference.IsMetronome && nextBPM && GM.instance.audioSource.isPlaying)
        {
            nextBPM = false;
            StartCoroutine(Metronome_BPM(GM.playerPreference.Metronome_bpm / 60));
        }

    }

    //節奏器打擊
    IEnumerator Metronome_BPM(float t)
    {
        yield return new WaitForSeconds(1/t);
        nextBPM = true;

        //播放音效
        beat_source.Play();

    }

}
