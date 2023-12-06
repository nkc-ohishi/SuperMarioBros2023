//-----------------------------------------------------------------------------
// 任意のキーでフェードしながらシーン遷移
//-----------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    // インスペクタで設定する値
    public string sceneName;    // シーン名
    public float fadeTime;      // フェード時間
    public KeyCode keyCode;     // シーン遷移キー

    void Update()
    {
        if(Input.GetKeyDown(keyCode))
        {
            // フェード時間を設定してシーン遷移
            FadeManager.Instance.LoadScene(sceneName, fadeTime);
        }
    }
}
