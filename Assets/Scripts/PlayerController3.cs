using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3 : MonoBehaviour
{
    Rigidbody2D rb2d;           // リジッドボディ２Ⅾコンポーネント保存用

    float inputLR;              // 左右入力
    bool inputJump;             // ジャンプ入力

    [SerializeField] float moveSpdX = 300;  // 移動速度x
    [SerializeField] float jumpPower = 30; // ジャンプ力

    // 着地判定用変数
    BoxCollider2D col;          // ボックスコライダー
    bool isGround;              // 地面にいるかのフラグ
    public LayerMask lMask;     // レイヤーマスク

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    // 物理挙動（Rigidbody）の変更はFixedUpdateで
    void FixedUpdate()
    {
        // 左右移動
        {
            // 左右入力に合わせて速度ベクトルをセット
            Vector2 vel = rb2d.velocity;
            vel.x = inputLR * moveSpdX * Time.deltaTime;
            rb2d.velocity = vel;
        }

        // ジャンプ
        if (inputJump == true)
        {
            Vector2 vel = rb2d.velocity;
            vel.y = 0;              // まずYの速度を初期化
            rb2d.velocity = vel;    // Yの速度反映
            // ジャンプ力を加える
            rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            inputJump = false;       // ジャンプフラグをオフに
        }
    }

    // キー入力、アニメーション、コライダー判定はUpdateメソッドで
    void Update()
    {
        // 地面チェック
        CheckGround();

        // 左右移動
        inputLR = Input.GetAxisRaw("Horizontal");

        // 地上にいる場合
        if (isGround == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                inputJump = true;               // ジャンプ入力オン
            }
        }
    }

    // 中心座標を取得
    Vector3 GetCenterPos()
    {
        Vector3 pos = transform.position;
        return pos;
    }

    // 足元の座標を取得
    Vector3 GetFootPos()
    {
        Vector3 pos = GetCenterPos();
        pos.y += -(col.size.y / 2);
        return pos;
    }

    // 地面に接触しているかチェック
    void CheckGround()
    {
        isGround = false;                           // 空中判定に
        Vector3 foot = GetFootPos();
        float width = col.size.x / 2;               // 幅を取得
        foot.x -= width;                            // 左端からチェック       
        for (int no = 0; no < 3; ++no)
        {   // ３点チェック                    
            Vector3 end = foot + Vector3.down;      // レイの長さ    
            // ラインキャストを用いた地面チェック
            RaycastHit2D result;
            result = Physics2D.Linecast(foot, end, lMask);
            Debug.DrawLine(foot, end, Color.red);   // デバッグ表示
            if (result.collider != null)
            {   // 何かに接触した
                isGround = true;
                Debug.Log("地面に接触");
                return;
            }
            foot.x += width;                        // ｘ座標をずらす
        }
    }

}
