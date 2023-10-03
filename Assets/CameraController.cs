using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform player;

    void Start()
    {
        // プレーヤー情報を保存
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // プレーヤーのX座標を取得
        float x = player.position.x;

        // カメラのX座標をプレーヤーのX座標と合わせる
        Vector3 pos = transform.position;
        pos.x = x;
        transform.position = pos;


    }
}
