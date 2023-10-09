using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    Rigidbody2D rb2d;           // リジッドボディ２Ⅾコンポーネント保存用

    float inputLR;              // 左右入力
    bool inputJump;             // ジャンプ入力

    [SerializeField] float moveSpdX = 300;  // 移動速度x
    [SerializeField] float jumpPower = 30; // ジャンプ力

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
        // 左右移動
        inputLR = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            inputJump = true;               // ジャンプ入力オン
        }
    }
}
