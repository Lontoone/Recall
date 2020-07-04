using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

//讀取UI文字
public class ReadUI_TextControl : MonoBehaviour {

    public TextMeshProUGUI[] UItext;

    public string fileName;

    public string[] text;

    private void Start()
    {
        //讀檔
        ReadTextFromFile();
        //套用文字
        for (int i=0;i<UItext.Length;i++) {
            UItext[i].text = text[i];
        }
    }

    //讀文字檔
    public void ReadTextFromFile()
    {
        GM.instance.DetectLanguage();

        string path = "UItext/" + GM.Language + "/" + fileName;

        //Debug.Log(path+ File.Exists(path + ".txt"));

        
        TextAsset textAsset = Resources.Load<TextAsset>(path);

        text = textAsset.text.Split('\n');
        
    }
}
