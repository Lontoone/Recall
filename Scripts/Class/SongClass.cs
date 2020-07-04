using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongClass  {

    //歌名
    public string SongName;

    //加權分數(perfect*500  Good*200  Bad*100  Miss-300)
    public int WeightingScore;

    //最佳成績
    public float BestScore;
    //最佳打擊率
    public float BestPercentage;

    //音樂的檔案位置
    [HideInInspector]
    public string SongFilePath;

    public SongClass(){
     
    }

    public SongClass(SongClass _song)
    {
        SongName = _song.SongName;
        BestScore = _song.BestScore;
        BestPercentage = _song.BestPercentage;
        SongFilePath = _song.SongFilePath;
    }
}
