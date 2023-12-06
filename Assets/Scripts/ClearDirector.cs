using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearDirector : MonoBehaviour
{
    float nextSceneTime;

    void Start()
    {
        nextSceneTime = 3;   // 次のシーンの再生時間
    }

    // Update is called once per frame
    void Update()
    {
        nextSceneTime -= Time.deltaTime;
        if(nextSceneTime < 0)
        {
            SceneManager.LoadScene("GameScene");
        }
        
    }
}
