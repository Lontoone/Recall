using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//自訂節點功能:
public class CustumeEditNode : MonoBehaviour {

    [System.Serializable]
    public class Node {
        public int PinIndex;
        public float time;

        //出現的順序 (可用可不用，並不代表點的出現順序)
        public int Order;
     
    }

    //節點的順序
    int NodeOrder = 0;

    List<Node> nodes = new List<Node>();

    //紀錄的間斷時間
    float recordGapTime = 0.15f;
    bool[] CanRecord = new bool[8];

    string _songName;

    public TypingPinControl[] tpControl;
    private void Start()
    {
        for (int i=0;i<CanRecord.Length;i++) { CanRecord[i] = true; }

        //tpControl = GameObject.FindObjectsOfType<TypingPinControl>();

        StartCoroutine(TimetoEnd(GM.instance.audioSource.clip.length));

        _songName = GM.instance.TempSongData.SongName;
        Debug.Log(_songName);
    }

    private void Update()
    {
        //紀錄時間 和 打擊符號
        for (int i=0;i<tpControl.Length;i++) {

            if (tpControl[i].IsPressed && CanRecord[i]) {
                CanRecord[i] = false;
                StartCoroutine(RecordGapTime(recordGapTime,i));
                //暫存
                SaveNode(nodes,i, GM.instance.audioSource.time,NodeOrder);
                NodeOrder++;
            }
        }

        

    }

    public static void SaveNode(List<Node> _nodes, int _index,float _time, int _Order) {
        Node _node = new Node
        {
            PinIndex = _index,
            time = _time,
            Order = _Order
        };

        _nodes.Add(_node);
    }


    //等待結束時:
    IEnumerator TimetoEnd( float time) {
        yield return new WaitForSeconds(time+5);

        //建檔儲存
        Save_LoadSystem.SaveNodeData(nodes,_songName);

        GM.instance.SceneLoad("MainScene");
    }

    IEnumerator RecordGapTime(float time, int index) {
        yield return new WaitForSeconds(time);
        CanRecord[index] = true;
    }
}
