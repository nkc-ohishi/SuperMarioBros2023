//------------------------------------------------------------------------------
// 科目：ゲームプログラミング
// 内容：ゲームシーン管理及びUI操作
// 日付：2023.10.31 Ken.D.Ohishi
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    // コンポーネント取得用
    public Text scoreLabel;         // スコアラベル
    public Text coinCountLabel;     // コイン枚数ラベル
    public Text timeLabel;          // 残り時間ラベル

    // 外部参照変数
    public static int score;        // スコア
    public static int coinCount;    // コイン取得枚数
    public static int gameState;    // ゲームの状態
    public static int lastTime;     // 残り時間

    // 制限時間
    const int MAX_TIME = 400;
    const float TIME_INTERBAL = 0.4f;
    float lastTimeInterbal;

    // ゴール得点処理後の待機時間
    float stateTime;

    void Start()
    {
        lastTime = MAX_TIME;    // 制限時間
        lastTimeInterbal = 0;   // 時間間隔計算用
        gameState = 1;          // ゲームの状態
        score     = 0;          // スコア
        stateTime = 2f;         // ゴール得点処理後の待機時間
    }

    void Update()
    {
        // ゴール後の得点処理が終わって２秒たったらシーン切り替え
        if(gameState == 2)
        {
            stateTime -= Time.deltaTime;
            if(stateTime <= 0)
            {
                SceneManager.LoadScene("ClearScene");
            }
            return;
        }

        // ゴールしたら得点処理
        if (PlayerController.isPaul)
        {
            if (lastTime > 0)
            {
                lastTime--;     // 残り時間を得点に加算
                score += 100;   // 残り時間１＝100ポイント加算
            }
            else
            {
                // ゴール後の得点処理終了したらゲームの状態を２にする
                gameState = 2;
            }

            // 残り時間表示
            timeLabel.text = "TIME\n " + lastTime.ToString("D3");

            // スコア表示
            scoreLabel.text = "MARIO\n" + score.ToString("D6");

            return;
        }






        // 制限時間処理 約0.4秒毎に時間を１減らす
        lastTimeInterbal += Time.deltaTime;
        if(lastTimeInterbal > TIME_INTERBAL)
        {
            lastTime--;                         // 残り時間を減らす
            lastTime = Mathf.Max(lastTime, 0);  // ０未満になら無いように制限
            lastTimeInterbal = 0;               // 時間間隔計算用変数クリア
        }

        // 残り時間を表示
        timeLabel.text = "TIME\n " + lastTime.ToString("D3");

        // スコア表示
        scoreLabel.text = "MARIO\n" + score.ToString("D6");

        // コイン枚数表示
        coinCountLabel.text = "\n  x" + coinCount.ToString("D2");
    }
}
