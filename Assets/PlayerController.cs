using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;        // リジッドボディ２Ⅾコンポーネント保存用
    public float speed;         // 左右の移動速度
    public float jumpPower;     // ジャンプの強さ
    Vector3 dir = Vector3.zero; // 左右の移動方向
    // public AudioSource jumpSe;

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 左右移動
        dir.x = Input.GetAxisRaw("Horizontal");
        transform.position += dir.normalized * speed * Time.deltaTime;
        rigid2D.velocity = new Vector2(0, rigid2D.velocity.y);

        // ジャンプ
        if (Input.GetButtonDown("Jump"))
        {
            // jumpSe.Play();
            rigid2D.AddForce(Vector2.up * jumpPower);
        }
    }
}
