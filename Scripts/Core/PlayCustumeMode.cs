using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//遊玩自製譜面
public class PlayCustumeMode : MonoBehaviour {

    //讀取自製譜面
    List<CustumeEditNode.Node> nodes = new List<CustumeEditNode.Node>();

    
    public GameObject[] tpControl;

    //打擊點
    public GameObject HitBubble_Perfab;

    [HideInInspector]
    public float BobbleLifetime;//氣泡存活時間

    private void Start()
    {
        nodes= Save_LoadSystem.LoadNodeData(GM.instance.TempSongData.SongName);

    }

    //產生打擊點
    private void Update()
    {
        if (nodes != null) {
        
            for (int i=0;i<nodes.Count;i++) {
                if (nodes.Count > 0 && GM.instance.audioSource.time >= (nodes[i].time))
                {

                    Debug.Log(GM.instance.audioSource.time + "  :  " + nodes[i].time + " " + nodes[i].Order);
                    //Debug.Log("<color=red> GM:"+ GM.instance.audioSource.time+"  now: " + (nodes[i].time - GM.instance.MuteSoundPlayDelay + 4)+"</color>");
                    //生產打擊點:
                    GameObject hitBubble = Instantiate(HitBubble_Perfab, tpControl[nodes[i].PinIndex].transform.position, Quaternion.identity, tpControl[nodes[i].PinIndex].transform);
                    hitBubble.GetComponent<CellBobbleBehavior>()._bobbleLifeTime = BobbleLifetime;
                    nodes.RemoveAt(i);
                }
            }
        }
    }


    //等待結束時:
    IEnumerator TimetoEnd(float time)
    {
        yield return new WaitForSeconds(time + 5);

        GM.instance.SceneLoad("EndingCounting");
    }
}
