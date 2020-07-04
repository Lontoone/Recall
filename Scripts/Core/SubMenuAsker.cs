using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//分支選項
public class SubMenuAsker : MonoBehaviour {

    //進入/退出的動畫名稱
    public string InAnimaName,OutAnimaName;

    Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    //打開選單
    public void OpenSubMenu(string FuncName) {
        //動畫
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(InAnimaName))
        {
            animator.Play(OutAnimaName);
        }

        else {
            animator.Play(InAnimaName);
        }

        //執行
        if (FuncName != "")
        {
            Invoke(FuncName, 5f);
        }
    }
	
	
	
	
	
	
}
