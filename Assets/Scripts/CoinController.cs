using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    int point = 200;    // “¾“_

    void Start()
    {
        
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameDirector.coinCount++;
            GameDirector.score += point;
            Destroy(gameObject);
        }
    }

}
