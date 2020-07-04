using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//讀取之前自定義的打擊點檔案
public class LoadPreviousNodeData : MonoBehaviour {


    public Button[] Pins;

    public Sprite[] Letters;

    public GameObject[] Lines;

    public Color NodeColor;

    List<CustumeEditNode.Node> nodes = new List<CustumeEditNode.Node>();

    public GameObject NodePrefab;

    //基準點
    public GameObject benchmark;

    private void Awake()
    {
        //讀取檔案
        nodes = Save_LoadSystem.LoadNodeData(GM.instance.TempSongData.SongName);

        //放在畫面上
        if (nodes != null)
        {
            LoadInNodes();
            NodeTypeEvent.Nodes = nodes;
            Debug.Log(NodeTypeEvent.Nodes.Count);
        }

    }
    void Start () {

       
	}


    void LoadInNodes() {
        //從基準點開始，依照時間位置擺放
        for (int i=0;i<nodes.Count;i++){
            //產生節點
            //GameObject letter = new GameObject(nodes[i].Order.ToString());
            for (int j = 0; j < 8; j++)
            {
                if (Pins[nodes[i].PinIndex] == Pins[j])
                {

                    //float Offset_X = 10 + nodes[i].time*Time.unscaledDeltaTime * 100;
                    //float Offset_X =Mathf.Round(nodes[i].time*100)/100*10;// 10 = CameraScrollSpeed
                    float Offset_X = nodes[i].time * 10;
                    Debug.Log(Offset_X);

                    Vector3 pos = new Vector3(benchmark.transform.position.x + Offset_X, Pins[j].transform.position.y, Pins[j].transform.position.z);
                    GameObject letter = Instantiate(NodePrefab, pos, Quaternion.identity, Lines[j].transform);
                    letter.transform.position = new Vector3(benchmark.transform.position.x + Offset_X, Pins[j].transform.position.y, Pins[j].transform.position.z);


                    letter.name = nodes[i].Order.ToString();
                    letter.GetComponent<SpriteRenderer>().sprite = Letters[j];


                    break;
                }
            }
        }
    }
	
	
	
	
	
}
