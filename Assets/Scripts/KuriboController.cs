//--------------------------------------------------------------------------------------------------
// 科目：ゲームプログラミング１年
// 概要：なんちゃってクリボースクリプト
// 日付：2023.10.16
//--------------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuriboController : MonoBehaviour
{
    public LayerMask lMask;             // レイヤーマスク
    Rigidbody2D rb2d;                   // リジッドボディ２Ⅾ保存
    BoxCollider2D col;                  // ボックスコライダー２Ⅾ保存
    [SerializeField] float speed = 2;   // 左右移動スピード
    [SerializeField] float upPower = 15;// やられたときに跳ねる強さ
    [SerializeField] int point = 200;   // 倒したときの得点
    float dir;                          // 進行方向
    bool isdeath;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();	// リジッドボディ２Ⅾ取得
        col = GetComponent<BoxCollider2D>();// ボックスコライダー２Ⅾ取得
        dir = -1;                           // 進行方向
        isdeath = false;
    }

    void Update()
    {
        if (isdeath == true) return;

        // 壁チェック
        WallCheck();

        // 移動
        transform.position += new Vector3(dir * speed * Time.deltaTime, 0, 0);
        
        // 落下したら固定位置から出現(デバッグ用)
        if(transform.position.y <= -50f)
        {
            rb2d.velocity = Vector2.zero;
            col.isTrigger = false;
            transform.position = new Vector3(-2, 0, -0.1f);
        }
    }

    void WallCheck()
    {
        Vector2 start = transform.position;                 // 中心座標取得
        Vector2 offset = new Vector2(0.1f, 0);              // コライダーからはみ出す長さ
        float width = col.size.x/2 + offset.x;              // レイの長さ 
        Vector2 end = start + new Vector2(width * dir, 0);  // レイの終点

        // 壁チェック
        RaycastHit2D result;
        result = Physics2D.Linecast(start, end, lMask);
        Debug.DrawLine(start, end, Color.red);   // デバッグ表示
        if (result.collider != null)
        {
            Debug.Log("壁に接触");
            dir *= -1;
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Foot")
        {
            Debug.Log("Foot Hit");
            // プレーヤーが死亡しているときは踏みつけ判定しない
            PlayerController pcon = collision.gameObject.GetComponentInParent<PlayerController>();
            if (pcon.isDead)
            {
                Debug.Log("プレーヤー死亡");
                return;
            }
            GameDirector.score += point;    // 得点を加算
            isdeath = true;

            // コライダーを削除
            Destroy(col);

            // 上方向に力を加えやられた演出
            rb2d.AddForce(Vector2.up * upPower, ForceMode2D.Impulse);
        }
    }

}