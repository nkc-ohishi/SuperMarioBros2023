//--------------------------------------------------------------------------------------------------
// 科目：ゲームプログラミング１年
// 概要：２Ⅾ横スクロールアクション
// 日付：2023.10.16
//--------------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public LayerMask lMask;     // レイヤーマスク
    Rigidbody2D rb2d;           // リジッドボディ２Ⅾコンポーネント保存用
    BoxCollider2D col;          // ボックスコライダー

    float inputLR;              // 左右入力

    bool inputJump;             // ジャンプ入力
    bool isGround;              // 地面にいるかのフラグ

    [SerializeField] Vector3 StartPos;       // プレーヤー初期座標
    [SerializeField] float fallTime = -50f;  // 落ちてからリスタートまでの時間

    [SerializeField] float MaxSpeed = 10;    // 移動最高速度x
    [SerializeField] float moveSpdX = 1000;  // 移動速度x
    [SerializeField] float jumpPower = 300;  // ジャンプ力

    // 死亡フラグをプロパティを使って宣言
    private bool _isDead;
    public bool isDead
    {
        get => _isDead;
        set => _isDead = value;
    }

    // Paul接触判定
    public static bool isPaul;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();  // リジッドボディ２Ⅾ取得
        col = GetComponent<BoxCollider2D>(); // ボックスコライダー２Ⅾ取得
        isDead = false;                      // 死亡フラグＯＦＦ
        isPaul = false;                      // Paul接触フラグ
    }

    // 物理挙動（Rigidbody）の変更はFixedUpdateで
    void FixedUpdate()
    {
        // 死亡フラグがONの時は処理をしない
        if (isDead) return;

        // Paulに接触したときの判定
        if(isPaul)
        {
            // 移動量を０にする
            rb2d.velocity = Vector2.zero;
            rb2d.gravityScale = 50;

            // 下についたら右に移動
            if(transform.position.y < -2.9f)
            {
                transform.position += Vector3.right * 5 * Time.deltaTime; 
            }

            return; // Paulに接触していれば以下の処理を行わない
        }


        // 左右移動
        {
            // 左右入力に合わせて速度ベクトルをセット
            Vector2 vel = rb2d.velocity;
            float speed = inputLR * moveSpdX * Time.deltaTime;
            rb2d.AddForce(new Vector2(speed, 0));

            if (Mathf.Abs(rb2d.velocity.x) > MaxSpeed)
            {
                vel.x = MaxSpeed * inputLR;
                rb2d.velocity = vel;
            }
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
        // 穴に落ちたらリスタート
        if (transform.position.y <= fallTime)
        {
            SceneManager.LoadScene("GameScene");

            //rb2d.velocity = Vector2.zero;   // 移動量０
            //col.isTrigger = false;          // トリガーを解除
            //isDead = false;                 // 死亡フラグ解除
            //transform.position = StartPos;  // 最初の位置から出現
        }


        if (isDead == true) return;

        // 制限時間が０になったら死亡
        if(GameDirector.lastTime == 0)
        {
            Dead();
        }

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
        pos.y += col.offset.y;
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
            {
                isGround = true;
                Debug.Log("地面に接触");
                return;
            }
            foot.x += width;                        // ｘ座標をずらす
        }
    }

    // やられたときの処理
    void Dead()
    {
        // ポールに触った後は死なないようにする
        if (isPaul) return;
            
        isDead = true;            // 死亡フラグON
        col.isTrigger = true;     // トリガーに変更

        // X方向の移動量を０にする
        Vector2 vel = rb2d.velocity;
        vel.x = 0;
        rb2d.velocity = vel;

        // 上方向に力を加えやられた演出
        rb2d.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 当たってきたオブジェクトが敵だったら
        if (collision.gameObject.tag == "Enemy")
        {
            Dead();

            //isDead = true;            // 死亡フラグON
            //col.isTrigger = true;     // トリガーに変更

            //// X方向の移動量を０にする
            //Vector2 vel = rb2d.velocity;
            //vel.x = 0;
            //rb2d.velocity = vel;

            //// 上方向に力を加えやられた演出
            //rb2d.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Paul")
        {
            Debug.Log("Paulに当たった");
            isPaul = true;
        }
    }
}
