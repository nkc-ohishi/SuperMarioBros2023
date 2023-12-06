using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  //ファイルのアクセス

public class StageMake : MonoBehaviour
{
    public GameObject[] obj;   // オブジェクトデータ
    TextAsset csvFile;          // CSVファイル
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    void Start()
    {
        CsvRead();
    }

    // CSVファイルを入力してステージを作成する処理
    void CsvRead()
    {
        csvFile = Resources.Load("stage1") as TextAsset;      // ResoucesフォルダのCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // コンマで分割し、一行ずつ読み込みリストに追加していく
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(','));   // コンマ区切りでリストに追加
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