using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block3Controller : MonoBehaviour
{
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
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * upPower, ForceMode2D.Impulse);
        }
    }
}
