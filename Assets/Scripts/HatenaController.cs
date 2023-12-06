using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatenaController : MonoBehaviour
{
    public GameObject block;  //�j��s�\�u���b�N

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
            block.SetActive(true);  //�j��s�\�u���b�N�\��
            block.GetComponent<Rigidbody2D>().AddForce(Vector3.up * upPower, ForceMode2D.Impulse);
            Destroy(gameObject);    //�n�e�i�u���b�N�폜
        }
    }
}
