using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatenaController : MonoBehaviour
{
    public GameObject block;  //破壊不能ブロック

    public float upPower = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Head")
        {
            block.SetActive(true);  //破壊不能ブロック表示
            block.GetComponent<Rigidbody2D>().AddForce(Vector3.up * upPower, ForceMode2D.Impulse);
            Destroy(gameObject);    //ハテナブロック削除
        }
    }
}
