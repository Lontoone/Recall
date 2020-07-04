using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//按下按鈕時=> 產生該時間點的node
public class NodeTypeEvent : MonoBehaviour {

    public Button[] Pins;

    //基準線
    public GameObject benchmark;

    public Sprite[] Letters;

    public GameObject[] Lines;

    public Color NodeColor;

    public GameObject NodePrefab;

    public static List<CustumeEditNode.Node> Nodes = new List<CustumeEditNode.Node>();

    [HideInInspector]
    //按下按鈕的順序
    public static int NodeOrder = 0;

    private void Awake()
    {
        //Nodes.Clear();
        NodeOrder = 0;
        Debug.Log("NodeOreder:"+NodeOrder);
    }

    private void Start()
    {
        if (Nodes.Count > 0)
        {
            //NodeOrder=先前紀錄的最大order
            Nodes.Sort((a, b) => a.Order.CompareTo(b.Order));

            //取得最大Order
            NodeOrder = Nodes[Nodes.Count-1].Order+1;
            Debug.Log("最大:"+NodeOrder);
        }
    
    }

    private void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (myCycusLib.Detect.GetTouchedObj(i,TouchPhase.Began) != null)
            {
                for (int j = 0; j < myCycusLib.Detect.GetTouchedObj(i, TouchPhase.Began).Length; j++)
                {
                   
                    if (myCycusLib.Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.gameObject == this.gameObject)
                    {
                        string _name =myCycusLib.Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.name;
                        CreatNode(_name);
                        Debug.Log(NodeOrder);
                        NodeOrder++;
                        
                    }
                }

            }
        }
    }

    //報廢  原本由button呼叫
    public void OnPinType() {

        try
        {
            string name = EventSystem.current.currentSelectedGameObject.name;

            CreatNode(name);
            NodeOrder++;
        }
        catch (System.Exception ex) {
            Debug.Log("<color=blue>"+ex+"</color>");
        }
        
    }


    void CreatNode(string _name) {
        //if (GM.IsVibrate) { Handheld.Vibrate(); }

        for (int i=0;i<Pins.Length;i++) {
            if (Pins[i].name==_name) {

                Vector3 pos= new Vector3(benchmark.transform.position.x, Pins[i].transform.position.y, Pins[i].transform.position.z);
                GameObject letter = Instantiate(NodePrefab, pos , Quaternion.identity, Lines[i].transform);
                letter.name = NodeOrder.ToString();
                letter.GetComponent<SpriteRenderer>().sprite = Letters[i];

                //暫時紀錄:
                CustumeEditNode.SaveNode(Nodes, i, GM.instance.audioSource.time, NodeOrder);

            }
        }
    }

   
}

