using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public void SceneLoad(int level)
    {
        SceneManager.LoadScene(level);

    }
    public void SceneLoad(string levelName)
    {
        Debug.Log("Load");
        try
        {
            SceneManager.LoadScene(levelName);
            //GM.instance.SceneLoad(levelName);
        }
        catch (System.Exception ex) {
            Debug.Log(ex);
            SceneManager.LoadScene(1);
        }

    }
}
