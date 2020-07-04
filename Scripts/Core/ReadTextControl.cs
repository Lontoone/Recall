using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using TMPro;
[System.Serializable]
public struct DialogSet {
    public string Dialog;
    public string Name;
}
public class ReadTextControl : MonoBehaviour {

    //內容台詞/名字
    public TextMeshProUGUI TMptext,nameText;

    public string fileName;

    
    public List<DialogSet> Dialog=new List<DialogSet>();

    public bool _canGoNextLine=false;

    //句數
    public int LineCount = 0;

    Animator animator;

    //讀文字檔
    public void ReadTextFromFile() {
        string path = "Dialog/"+GM.Language + "/" + fileName;
        Debug.Log(path);
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        string[] _Dialog = textAsset.text.Split('\n');

        for (int i =0 ; i <_Dialog.Length;i++) {
            //Dialog[i].Dialog = _Dialog[i];
            DialogSet _set = new DialogSet
            {
                Dialog = _Dialog[i],
                Name = nameText.text
            };
            Dialog.Add(_set);
        }

    }

    //每_秒讀文字 (逐字讀)
    public IEnumerator _readText(int i) {
        TMptext.text = "";

        char[] textChar = Dialog[i].Dialog.ToCharArray();

        //逐字
        int j =0;

        while (j<textChar.Length) {
            TMptext.text += textChar[j];
            j++;
            yield return new WaitForSeconds(0.05f);
         }

        //等待按下
        StartCoroutine(WaitForTap());
    
    }
    private void Start()
    {
        ReadTextFromFile();

        _canGoNextLine = true;

        //進入動畫
        animator = gameObject.GetComponent<Animator>();
        animator.Play("DialogBoxOpen");
    }

    private void OnDisable()
    {
        animator.Play("DialogBoxClose");
    }
    private void Update()
    {
        if (_canGoNextLine && LineCount<Dialog.Count) {

            //讀字
            _canGoNextLine = false;
            //名字text
            nameText.text = Dialog[LineCount].Dialog;
            //台詞=名字的下一句
            LineCount++;

            //直接跳離
            if (Dialog[LineCount].Dialog == "") { LineCount = Dialog.Count; }
            else
            {
                StartCoroutine(_readText(LineCount));
            }
        }
        //Debug.Log(LineCount);
        //講完了
        if (LineCount == Dialog.Count ) {
            LineCount = 0;

            this.enabled = false;
        }
       
    }

    IEnumerator WaitForTap() {
        while(!(Input.touchCount >= 1 || Input.anyKeyDown)){
            yield return null;
        }

        LineCount++;
        _canGoNextLine = true;
    }

    //檢查是否按下
    bool CheckForTap()
    {

        if (Input.touchCount >= 1 || Input.anyKeyDown)
        {
            return true;
        }
        else {
            return false;
        }
    }
}
