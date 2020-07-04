using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myCycusLib;
using TMPro;

//處理選擇自訂節點
public class SelectCustumeNode : MonoBehaviour
{

    //目前選擇的物件
    RaycastHit2D[] CurrentSelectedObj;

    //原本的顏色
    Color originColor;

    //全部以選擇的物件
    List<GameObject> SelectedObj = new List<GameObject>();

    //儲存成功音效
    public AudioSource audioSource;

    private void Start()
    {
        //Nodes = NodeTypeEvent.Nodes;
    }

    private void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Detect.GetTouchedObj(i,TouchPhase.Began) != null)
            {
                for (int j = 0; j < Detect.GetTouchedObj(i, TouchPhase.Began).Length; j++)
                {
                    //檢查tag
                    if (!Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.CompareTag("CustumeNode")) { continue; }

                    //已被選上=>刪除選取
                    if (SelectedObj.Contains(Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.gameObject))
                    {
                        SelectedObj.Find(SelectedObj=>SelectedObj.gameObject== Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.gameObject).GetComponent<SpriteRenderer>().color=originColor;
                        SelectedObj.Remove(Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.gameObject);

                        //取消文字
                        Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

                        //取消子物件的文字紀錄
                        Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.GetComponent<UntuchableOutsideCamera>().origineText="";
                    }

                    //選取:
                    else {
                        //紀錄原本的顏色
                        originColor = Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.gameObject.GetComponent<SpriteRenderer>().color;
                        
                        SelectedObj.Add(Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.gameObject);
                        SelectedObj.Find(SelectedObj => SelectedObj.gameObject == Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.gameObject).GetComponent<SpriteRenderer>().color = Color.blue;


                        ShowNodeTimeText(Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.gameObject ,
                           Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>());
                        

                    }

                }
            }

        }


    }


    void ShowNodeTimeText(GameObject _obj ,TextMeshProUGUI _tmp) {
        float _time = NodeTypeEvent.Nodes.Find(node=> node.Order.ToString()==_obj.name.ToString()).time;
  
        _tmp.text = (_time / 60).ToString("00") + ":" + (_time % 60).ToString(".00");
    }
    //刪除節點 (由外部按鈕呼叫)
    public void DeletNode()
    {
        
        for (int i=0;i<SelectedObj.Count;i++) {
            
            Destroy(SelectedObj[i].gameObject);

            NodeTypeEvent.Nodes.Remove(NodeTypeEvent.Nodes.Find(Nodes => Nodes.Order.ToString()==SelectedObj[i].gameObject.name));
        }
        //刪除
        SelectedObj.Clear();

    }

    //儲存目前進度
    public void Save() {
        Save_LoadSystem.SaveNodeData(NodeTypeEvent.Nodes, GM.instance.TempSongData.SongName);
        //播放音效
        audioSource.Play();
    }

    //前進/後退 NODE的時間點
    public void Plus_time() {
        for (int i=0;i<SelectedObj.Count;i++) {

            //移動位置
            float Offset_X = Mathf.Round(NodeTypeEvent.Nodes.Find(nodes=>nodes.Order.ToString()==SelectedObj[i].gameObject.name).time * 100) / 100 * 10;
            //float Offset_X = 0.1f;
            SelectedObj[i].transform.position = new Vector3(Offset_X, SelectedObj[i].transform.position.y, SelectedObj[i].transform.position.z);

            //改變資料的時間點
            NodeTypeEvent.Nodes.Find(Nodes => Nodes.Order.ToString() == SelectedObj[i].gameObject.name).time += 0.1f;

            //展示時間點
            ShowNodeTimeText(SelectedObj[i].transform.gameObject,
                 SelectedObj[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>());
        }
    }

    public void Minus_time() {
        for (int i = 0; i < SelectedObj.Count; i++)
        {

            //移動位置
            //float Offset_X = 0.1f;
            float Offset_X = Mathf.Round(NodeTypeEvent.Nodes.Find(nodes => nodes.Order.ToString() == SelectedObj[i].gameObject.name).time * 100) / 100 * 10;
            SelectedObj[i].transform.position = new Vector3( Offset_X, SelectedObj[i].transform.position.y, SelectedObj[i].transform.position.z);

            //改變資料的時間點
            NodeTypeEvent.Nodes.Find(Nodes => Nodes.Order.ToString() == SelectedObj[i].gameObject.name).time -= 0.1f;

            //展示時間點
            ShowNodeTimeText(SelectedObj[i].transform.gameObject,
                 SelectedObj[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>());
        }
    }

}

