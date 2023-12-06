using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    Vector3[] pos = { 
        new Vector3(-0.5f, -0.275f,0),
        new Vector3(-0.5f, -0.43f,0)
    };

    int curNo;

    // Start is called before the first frame update
    void Start()
    {
        curNo = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            curNo = (curNo == 0) ? 1 : 0;
            transform.position = pos[curNo];
        }

    }
}
