using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//切換scene
public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string name){
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
}
