using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform player;

    void Start()
    {
        // �v���[���[����ۑ�
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // �v���[���[��X���W���擾
        float x = player.position.x;

        // �J������X���W���v���[���[��X���W�ƍ��킹��
        Vector3 pos = transform.position;
        pos.x = x;
        transform.position = pos;


    }
}
