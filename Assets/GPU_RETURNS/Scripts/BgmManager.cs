//---------------------------------------------------------------------------------------------
// BgmManager.cs
// 概要：BGMを管理するクラス。シングルトンオブジェクトにし、ゲームを終了するまで破棄しない
// 元ネタ：https://naichilab.blogspot.com/
//---------------------------------------------------------------------------------------------
using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// BGMの制御
public class BgmManager : MonoBehaviour
{
	// リージョンディレクティブ　折りたたみ可能範囲指定
	#region Singleton

	// BgmManagerインスタンス保存用変数
	private static BgmManager instance;

	// Instanceプロパティの定義(シングルトン）
	public static BgmManager Instance 
	{
		// getアクセサー（ゲッター）
		get
		{
			// インスタンスが空なら
			if (instance == null)
			{
				// ヒエラルキーから BgmManagerオブジェクトを探して取得
				instance = (BgmManager)FindObjectOfType (typeof(BgmManager));

				// ヒエラルキーに BgmManagerオブジェクトがいなければエラーメッセージを出す
				if (instance == null) 
				{
					Debug.LogError (typeof(BgmManager) + "is nothing");
				}
			}

			// 格納した BgmManagerオブジェクトを返す
			return instance;
		}
	}

	#endregion Singleton

	public float TargetVolume = 1.0f;	// BGM再生音量(0.0～1.0)
	public float TimeToFade = 2.0f;		// フェードイン、フェードアウトにかかる時間です。
	public float CrossFadeRatio = 1.0f;	// 音を重ねる割合(0.0重ねない～1.0完全クロスフェード)

	[NonSerialized]
	public AudioSource CurrentAudioSource = null;				// 現在再生中のAudioSource
	private List<AudioSource> AudioSources = null;				// クロスフェードさせるために２つ用意
	private Dictionary<string, AudioClip> AudioClipDict = null; // 再生可能なBGM(AudioClip)のリスト
	private IEnumerator fadeOutCoroutine;						// フェードアウト中断に使用
	private IEnumerator fadeInCoroutine;						// フェードイン中断に使用


	// FadeOut中、もしくは再生待機中のAudioSource
	public AudioSource SubAudioSource 
	{
		get
		{
			//bgmSourcesのうち、CurrentAudioSourceでない方を返す
			if (AudioSources == null)
			{
				return null;
			}
			foreach (AudioSource s in AudioSources) 
			{
				if (s != CurrentAudioSource) 
				{
					return s;
				}
			}
			return null;
		}
	}

	public void Awake ()
	{
		// 生成されたオブジェクトが同一のものでなければ破棄
		if (this != Instance) 
		{
			Destroy (gameObject);
			return;
		}
		// シーンが遷移しても、Destroyされない設定
		DontDestroyOnLoad (gameObject);

		// AudioSourceを２つ用意。クロスフェード時に同時再生するために２つ用意する。
		AudioSources = new List<AudioSource> ();
		AudioSources.Add (gameObject.AddComponent<AudioSource> ());
		AudioSources.Add (gameObject.AddComponent<AudioSource> ());

		// ２つの AudioSource コンポーネントの共通設定
		foreach (AudioSource s in this.AudioSources) 
		{
			s.playOnAwake = false;
			s.volume = 0f;
			s.loop = true;
		}

		// オーディオクリップを名前と一緒に保存する変数を用意
		AudioClipDict = new Dictionary<string, AudioClip> ();

		// [Resources/Audio/BGM]フォルダ内の音源をオーディオクリップとして読み込み、
		// Dictionary(連想配列)に登録
		foreach (AudioClip bgm in Resources.LoadAll<AudioClip>("Audio/BGM")) 
		{
			Debug.Log(bgm.name);
			AudioClipDict.Add (bgm.name, bgm);
		}

		// 有効なスピーカー(AudioListener)が一つも無い場合は、
		// BgmManagerオブジェクトにスピーカー(AudioListenerコンポーネント)を追加する。
		// （通常はAudioListenerはデフォでMainCameraにアタッチされている）
		if (FindObjectsOfType (typeof(AudioListener)).All (o => !((AudioListener)o).enabled)) 
		{
			// BmgManagerオブジェクトにAudioListenerコンポーネントを追加する
			gameObject.AddComponent<AudioListener> ();
		}

	}

    // BGMを再生します。
    public void Play (string bgmName)
	{
		// 再生するBGMが見つからないときはエラーメッセージを出して終了
		if (!AudioClipDict.ContainsKey (bgmName)) 
		{
			Debug.LogError (string.Format ("BGM名[{0}]が見つかりません。", bgmName));  
			return;
		}

		// 引数で指定した音源がすでに再生中なら終了
		if ((CurrentAudioSource != null) && (CurrentAudioSource.clip == AudioClipDict [bgmName]))
		{
			return;
		}

		//
		stopFadeOut (); // fadeOutCoroutineを止める
		stopFadeIn ();  // fadeInCoroutineを止める

		// 再生中のBGMをフェードアウト開始
		Stop();        

		// フェードインの開始時間を保存
		float fadeInStartDelay = TimeToFade * (1.0f - CrossFadeRatio);

		// 使用されていないほうのオーディオソースをカレントオーディオソースに設定
		CurrentAudioSource = SubAudioSource;

		// カレントオーディオソースに引数で指定された音源を登録
		CurrentAudioSource.clip = AudioClipDict [bgmName];

		// フェードインコルーチンを開始
		fadeInCoroutine = fadeIn(CurrentAudioSource, TimeToFade, CurrentAudioSource.volume, TargetVolume, fadeInStartDelay);
		StartCoroutine (fadeInCoroutine);
	}

	// BGMを停止
	public void Stop ()
	{
		// 再生中の音源があるなら
		if (CurrentAudioSource != null) 
		{
			// 再生中の音源をフェードアウトスタート
			fadeOutCoroutine = fadeOut (CurrentAudioSource, TimeToFade, CurrentAudioSource.volume, 0f);
			StartCoroutine (fadeOutCoroutine);
		}
	}

	// BGMをすべてすぐ停止
	public void StopImmediately ()
	{
		// 各コルーチンをクリア
		fadeInCoroutine = null;
		fadeOutCoroutine = null;

		// すべてのオーディオソースを停止
		foreach (AudioSource s in AudioSources) 
		{
			s.Stop ();
		}
		// カレントオーディオソースもクリア
		CurrentAudioSource = null;
	}

	// BGMをフェードインさせながら再生を開始
	// 引数１．AudioSource
	// 引数２．フェードインにかかる時間
	// 引数３．初期音量
	// 引数４．フェードイン完了時の音量
	// 引数５．フェードイン開始までの待ち時間
	private IEnumerator fadeIn (AudioSource bgm, float timeToFade, float fromVolume, float toVolume, float delay)
	{
		// delay秒処理を止める
		if (delay > 0) 
		{
			yield return new WaitForSeconds (delay);
		}
		
		// 開始時間を保存し、再生開始
		float startTime = Time.time;
		bgm.Play ();

		while (true) 
		{
			// スタートしてからの経過時間を計測
			float spentTime = Time.time - startTime;

			// 経過時間がフェード時間より大きくなったら
			if (spentTime > timeToFade) 
			{
				bgm.volume = toVolume;      // 初期音量をセット
				fadeInCoroutine = null;     // フェードインコルーチンをクリア
				break;						// ループを抜ける
			}

			// 音量を徐々に増やす
			float rate = spentTime / timeToFade;
			float vol = Mathf.Lerp (fromVolume, toVolume, rate);
			bgm.volume = vol;

			yield return null;
		}
	}

	// BGMをフェードアウトし、その後停止
	// 引数１．フェードアウトさせるAudioSource
	// 引数２．フェードアウトにかかる時間
	// 引数３．フェードアウト開始前の音量
	// 引数４．フェードアウト完了時の音量
	private IEnumerator fadeOut (AudioSource bgm, float timeToFade, float fromVolume, float toVolume)
	{
		// 開始時間を保存し、停止開始
		float startTime = Time.time;

		while (true) 
		{
			float spentTime = Time.time - startTime;

			// 指定したフェードアウト時間を超えたら
			if (spentTime > timeToFade) 
			{
				bgm.volume = toVolume;      // 音量を0する
				bgm.Stop ();				// オーディオソース停止
				fadeOutCoroutine = null;	// フェードアウトコルーチンクリア
				break;						// ループを抜ける
			}

			// ボリュームを徐々に減らす
			float rate = spentTime / timeToFade;
			float vol = Mathf.Lerp (fromVolume, toVolume, rate);
			bgm.volume = vol;

			yield return null;
		}
	}

	// フェードイン処理を中断します。
	private void stopFadeIn ()
	{
		// フェードインコルーチンが動いていれば止める
		if (fadeInCoroutine != null)
		{
			StopCoroutine(fadeInCoroutine);
		}
		fadeInCoroutine = null;
	}

	// フェードアウト処理を中断します。
	private void stopFadeOut ()
	{
		// フェードアウトコルーチンが動いていれば止める
		if (fadeOutCoroutine != null)
		{
			StopCoroutine(fadeOutCoroutine);
		}
		fadeOutCoroutine = null;
	}

    // デバッグ用操作パネルを表示
    //public bool DebugMode = true;       // デバッグモード
    //public void OnGUI()
    //{
    //    if (DebugMode)
    //    {
    //        //AudioClipが見つからなかった場合
    //        if (AudioClipDict.Count == 0)
    //        {
    //            GUI.Box(new Rect(10, 10, 200, 50), "BGM Manager(Debug Mode)");
    //            GUI.Label(new Rect(10, 35, 80, 20), "Audio clips not found.");
    //            return;
    //        }

    //        //枠
    //        GUI.Box(new Rect(10, 10, 200, 150 + this.AudioClipDict.Count * 25), "BGM Manager(Debug Mode)");
    //        int i = 0;
    //        GUI.Label(new Rect(20, 30 + i++ * 20, 180, 20), "Target Volume : " + this.TargetVolume.ToString("0.00"));
    //        GUI.Label(new Rect(20, 30 + i++ * 20, 180, 20), "Time to Fade : " + this.TimeToFade.ToString("0.00"));
    //        GUI.Label(new Rect(20, 30 + i++ * 20, 180, 20), "Crossfade Ratio : " + this.CrossFadeRatio.ToString("0.00"));

    //        i = 0;
    //        //再生ボタン
    //        foreach (AudioClip bgm in this.AudioClipDict.Values)
    //        {
    //            bool currentBgm = (this.CurrentAudioSource != null && this.CurrentAudioSource.clip == this.AudioClipDict[bgm.name]);

    //            if (GUI.Button(new Rect(20, 100 + i * 25, 40, 20), "Play"))
    //            {
    //                this.Play(bgm.name);
    //            }
    //            string txt = string.Format("[{0}] {1}", currentBgm ? "X" : "_", bgm.name);
    //            GUI.Label(new Rect(70, 100 + i * 25, 1000, 20), txt);

    //            i++;
    //        }

    //        //停止ボタン
    //        if (GUI.Button(new Rect(20, 100 + i++ * 25, 180, 20), "Stop"))
    //        {
    //            this.Stop();
    //        }
    //        if (GUI.Button(new Rect(20, 100 + i++ * 25, 180, 20), "Stop Immediately"))
    //        {
    //            this.StopImmediately();
    //        }
    //    }
    //}

}

// Unityオーディオ系コンポーネント
// 説明：
// Unityの基本的なサウンドコンポーネント
// AudioClip     = オーディオファイル（音源）
// AudioSource   = AudioClipを再生する機能（プレイヤー）
// AudioListener = スピーカー

