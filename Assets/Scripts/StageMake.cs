using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  //�t�@�C���̃A�N�Z�X

public class StageMake : MonoBehaviour
{
    public GameObject[] obj;   // �I�u�W�F�N�g�f�[�^
    TextAsset csvFile;          // CSV�t�@�C��
    List<string[]> csvDatas = new List<string[]>(); // CSV�̒��g�����郊�X�g;

    void Start()
    {
        CsvRead();
    }

    // CSV�t�@�C������͂��ăX�e�[�W���쐬���鏈��
    void CsvRead()
    {
        csvFile = Resources.Load("stage1") as TextAsset;      // Resouces�t�H���_��CSV�ǂݍ���
        StringReader reader = new StringReader(csvFile.text);

        // �R���}�ŕ������A��s���ǂݍ��݃��X�g�ɒǉ����Ă���
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); // ��s���ǂݍ���
            csvDatas.Add(line.Split(','));   // �R���}��؂�Ń��X�g�ɒǉ�
        }

        for (int y = 0; y < 13; y++)
        {
            for (int x = 0; x < 210; x++)
            {
                int i = int.Parse(csvDatas[y][x]) - 1;
                if (i >= 0)
                {
                    Instantiate(obj[i], new Vector3(-11+x, 6-y, 0.1f), Quaternion.identity);
                }
            }
        }
    }
}