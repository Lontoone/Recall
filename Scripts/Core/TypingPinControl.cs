using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingPinControl : MonoBehaviour {

    //對應的撞針
    public GameObject LetterPin;
    //自身按鍵符號
    Sprite letter;

    //轉動的紙
    GameObject TypingPaper;


    bool CanType = true;


    //紀錄被按下的按鈕
    public bool IsPressed = false;

    Animator animator;

    //正被按下的按鈕
    List<GameObject> PressedbtnList = new List<GameObject>();

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        letter = transform.Find("PinText_def1").GetComponent<SpriteRenderer>().sprite;

        TypingPaper = GameObject.Find("TypingPaper");
    }

    //偵測是否被按下
    private void Update()
    {
        
        CheckTouchBobble();

        if (IsPressed && !animator.GetCurrentAnimatorStateInfo(0).IsName("PinPressed_anima")) {
            animator.Play("PinPressed_anima");
            
        }

        //檢查手放開?
        if (IsPressed == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PinRelease_anima"))
        {
            //放手

            animator.Play("PinRelease_anima");

            LetterPin.GetComponent<Animator>().Play("LetterRelease");
        }

        if (IsPressed && CanType) {

            //連打誤差時間
            CanType = false;
            StartCoroutine(TypingGapTime());

            TypeLetter();
        }

    }


    //是否碰到打擊泡泡=>是就消除
     void CheckTouchBobble()
    {
        //IsPressed = false;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (myCycusLib.Detect.GetTouchedObj(i,TouchPhase.Began) != null)
            {
              
                for (int j = 0; j < myCycusLib.Detect.GetTouchedObj(i, TouchPhase.Began).Length; j++)
                {
                    //IsPressed = false;
                    if (myCycusLib.Detect.GetTouchedObj(i, TouchPhase.Began)[j].transform.gameObject == this.gameObject)
                    {
                        PressedbtnList.Add(this.gameObject);
                        IsPressed = true;
                        break;
                    }
                    
                    
                }
                

            }
            
            //檢查放手
            /*
            if (myCycusLib.Detect.GetTouchedObj(i,TouchPhase.Stationary) != null)
            {
                for (int j = 0; j < myCycusLib.Detect.GetTouchedObj(i,TouchPhase.Stationary).Length; j++)
                {

                    if (myCycusLib.Detect.GetTouchedObj(i,TouchPhase.Stationary)[j].transform.gameObject == this.gameObject)
                    {

                        IsPressed = false;
                        break;
                    }

                }


            }
            */
        }
    }

    private void LateUpdate()
    {
        //手指移開按鈕後自動回彈
        for (int i=0;i<Input.touchCount;i++) {
            //有手指離開
            if (Input.GetTouch(i).phase==TouchPhase.Ended || Input.GetTouch(i).phase == TouchPhase.Moved || Input.GetTouch(i).phase == TouchPhase.Stationary) {
                for (int j = 0; j < PressedbtnList.Count; j++) {

                    GameObject[] btns = PressedbtnList.ToArray();

                    if (btns[j]==this.gameObject) {

                        
                        PressedbtnList.Remove(this.gameObject);
                        IsPressed = false;
                    }
                }
            }
        }
    }

    //產生字符
    public void TypeLetter() {
        //動畫
        LetterPin.GetComponent<Animator>().Play("LetterType");
        //音效
        LetterPin.GetComponent<AudioSource>().PlayOneShot(LetterPin.GetComponent<AudioSource>().clip);
        //產生字符
        GameObject _letter = new GameObject("letter");
        _letter.transform.position = new Vector2(LetterPin.transform.position.x+0.2f, LetterPin.transform.position.y+2f);
        _letter.AddComponent<SpriteRenderer>();
        _letter.GetComponent<SpriteRenderer>().sprite = letter;
        _letter.GetComponent<SpriteRenderer>().color = Color.black;
        _letter.GetComponent<SpriteRenderer>().sortingOrder = 2;

        _letter.AddComponent<PaperRoller>();
        _letter.GetComponent<PaperRoller>().rollMode = PaperRoller.RollMode.letter;

        _letter.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
        _letter.transform.parent = TypingPaper.transform;
    }



    //連打誤差
    IEnumerator TypingGapTime()
    {
        yield return new WaitForSeconds(0.15f);
        CanType = true;
    }
}
