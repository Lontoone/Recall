using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//選擇優先生產頻率
class PriProduce{

    //檢查有無可生產的頻率 (?
    public bool CanProduce=false;

    //最高頻率的level
    public float HighestFrequency=0;

    //public int[] HighestFreLevel;
    public List<int> HighestFreLevel = new List<int>();

    //計數器
    public int j = 0;
    }

class ProduceControl {
    public ProduceControl() { }
    public ProduceControl(bool _isGapTImeOver, float _lastime) {
        IsGapTimeOver = _isGapTImeOver;
        LastingTime = _lastime;
    }

    public bool IsGapTimeOver = true;

    //連續產生時間
    public float LastingTime = 0;
}

public class TypingCellControl : MonoBehaviour {

    public GameObject[] bobblePrefab;

    public GameObject[] Cells ;

    [Header("共用生產冷卻時間")]
    public float GapTime = 0.5f;

    ProduceControl[] produceControls = new ProduceControl[8];
    
    //各個cell生產冷卻時間

    public bool[] CanProduce = new bool[8];

    [Header("氣泡生存時間")]
    public float BobbleLifetime=3;

    [Header("在這(誤差值)範圍內的frequency level可以產生打擊氣泡")]
    public float FreDeviation;

    private void Start()
    {
        
        for (int i=0;i<CanProduce.Length; i++) {
            CanProduce[i] = true;

            produceControls[i] =new ProduceControl(true,0);
        }

        

    }

    private void Update()
    {
        PriProduce pri = new PriProduce();
        //判斷各音階是否符合產生資格
        pri = CheckCanProduce();

        for (int i=0;i<pri.HighestFreLevel.Count;i++) {

            if (CanProduce[i]
               && GM.instance.audioSource.isPlaying)
            {

                //IsProduceInterOver = false;
                ProduceBobble(i);
                //StartCoroutine(ProduceInter(pri.HighestFreLevel[i], GapTime));
                //produceControls[i].IsGapTimeOver = false;
                CanProduce[i] = false;

                //Debug.Log("Level" + pri.HighestFreLevel[i] + " CanProduce[" + i + "] " + CanProduce[i] + " IsGapTimeOver " + produceControls[i].IsGapTimeOver);
            }
            
            //Debug.Log(Cells[i] + " " + Cells[i].transform.childCount);
            #region
            /*
            if (CanProduce[i] && produceControls[i].IsGapTimeOver   
                && pri.CanProduce && Cells[i].transform.childCount<2 && GM.instance.audioSource.isPlaying) {
                Debug.Log(Cells[i]+" " + Cells[i].transform.childCount);
                //IsProduceInterOver = false;

                StartCoroutine(ProduceInter(pri.HighestFreLevel[i], GapTime));
                produceControls[i].IsGapTimeOver = false;
                CanProduce[i] = false;

            }*/
            #endregion
        }


    }



    private void FixedUpdate()
    {
        /*
        for (int i=0;i<8;i++) {
            if (produceControls[i].IsGapTimeOver==false)
            {

                //紀錄bobble持續時間
                //produceControls[i].LastingTime += Time.deltaTime;
                produceControls[i].LastingTime += Time.deltaTime;

            }
            //持續時間過長=>中斷一下
            if (produceControls[i].LastingTime >= BobbleLifetime) {

                StartCoroutine(ProduceInter(i,GapTime));

                
            }

            
        }*/

        
    }

    //生產打擊提示
    public void ProduceBobble(int i) {

        bobblePrefab[0].GetComponent<CellBobbleBehavior>().FreLevel = i;
        bobblePrefab[0].GetComponent<CellBobbleBehavior>()._bobbleLifeTime = BobbleLifetime;
        bobblePrefab[0].transform.localScale = new Vector3(0.88f,0.88f,0.88f);
        Instantiate(bobblePrefab[0], Cells[i].transform.position, Quaternion.identity, Cells[i].transform);

    }

    //檢查freLevel
    bool CheckCanProduce(int i) {

        float FreAvg = 0;
        for (int j = 0; j < 512; j++)
        {
            FreAvg += AudioPeer._samples[j];
        }
        FreAvg /= 512;

        if (AudioPeer._audioBand[i] >= FreAvg ) {
            return true;
        }

        return false;
    }

    PriProduce CheckCanProduce()
    {

        PriProduce prim = new PriProduce();
        //List<PriProduce> prim = new List<PriProduce>();

        //紀錄所有合格 [頻率,level] //從中找到最高頻率音階回傳
        float[,] _qualifiedLevel = new float[8, 2];

        float FreAvg = 0;
        for (int j = 0; j < 512; j++)
        {
            FreAvg += AudioPeer._samples[j];
        }
        FreAvg /= 512;

        for (int i = 0; i < 8; i++)
        {
            if (AudioPeer._audioBand[i] >= FreAvg)
            {
                //return true;
                _qualifiedLevel[i, 0] = AudioPeer._audioBand[i];
                _qualifiedLevel[i, 1] = i;

                if (prim.HighestFrequency< _qualifiedLevel[i,0]) {
                    prim.HighestFrequency = _qualifiedLevel[i, 0];
                    //prim.HighestFreLevel[prim.j] = (int)_qualifiedLevel[i, 1];
                    prim.j++;
                }
            }

        }

        for (int i=0;i<prim.j;i++) {
            //合格的音階如果在誤差值內=> 記錄下來一同輸出
            if (Mathf.Abs(_qualifiedLevel[i,0]-prim.HighestFrequency)<=FreDeviation) {
                prim.HighestFreLevel.Add((int)_qualifiedLevel[i,1]);
            }
        }

        prim.CanProduce = (prim.HighestFrequency == 0) ? false : true;

        return prim;
        
    }

  
    IEnumerator ProduceInter(int level,float time)
    {
       
        ProduceBobble(level);
        produceControls[level].LastingTime = 0;

        yield return new WaitForSeconds(time);
        produceControls[level].IsGapTimeOver = true;
        //IsProduceInterOver = true;


    }



    public IEnumerator ProduceGapTime(int Frelevel,float time) {

        yield return new WaitForSeconds(time);
        CanProduce[Frelevel] = true;
        
    }
}
