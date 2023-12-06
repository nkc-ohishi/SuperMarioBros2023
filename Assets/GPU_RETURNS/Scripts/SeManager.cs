//---------------------------------------------------------------------------------------------
// SeManager.cs
// 概要：SEを管理するクラス。シングルトンオブジェクトにし、ゲームを終了するまで破棄しない
// 元ネタ：https://naichilab.blogspot.com/
//---------------------------------------------------------------------------------------------
using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class SeManager : MonoBehaviour
{
	// リージョンディレクティブ　折りたたみ可能範囲指定
	#region Singleton

	// SeManagerインスタンス保存用変数
	private static SeManager instance;

	// Instanceプロパティの定義(シングルトン）
	public static SeManager Instance 
	{
		get
		{
			// インスタンスが空なら
			if (instance == null) 
			{
				// ヒエラルキーから SeManagerオブジェクトを探して取得
				instance = (SeManager)FindObjectOfType (typeof(SeManager));

				// ヒエラルキーに SeManagerオブジェクトがいなければエラーメッセージを出す
				if (instance == null) 
				{
					Debug.LogError (typeof(SeManager) + "is nothing");
				}
			}

			// 格納した SeManagerオブジェクトを返す
			return instance;
		}
	}
	#endregion Singleton

	public int MaxAudioSources = 10;							// 最大同時再生数
	[Range (0f, 1f)]public float Volume = 1.0f;					// SE再生音量
	private List<AudioSource> AudioSources = null;              // 同時再生させるためにオーディオソースを複数用意
	private Dictionary<string,AudioClip> AudioClipDict = null;	// 再生可能なSE(AudioClip)のリスト

	public void Awake ()
	{
		// 生成されたオブジェクトが同一のものでなければ破棄
		if (this != Instance) 
		{
			Destroy (gameObject);
			return;
		}

		// シーンが遷移しても、Destroyされない設定
		DontDestroyOnLoad(gameObject);

		// 同時再生させるためにオーディオソースを複数用意
		AudioSources = new List<AudioSource> ();

		//[Resources/Audio/SE]フォルダからSEを探してオーディオクリップに登録
		AudioClipDict = new Dictionary<string, AudioClip> ();
		foreach (AudioClip bgm in Resources.LoadAll<AudioClip>("Audio/SE")) 
		{
			AudioClipDict.Add (bgm.name, bgm);
		}

		// 有効なAudioListenerが一つも無い場合は生成する
		// 大体はMainCameraについてる
		if (FindObjectsOfType (typeof(AudioListener)).All (o => !((AudioListener)o).enabled)) 
		{
			gameObject.AddComponent<AudioListener> ();
		}
	}

	// SEを再生します。
	public void Play (string seName)
	{
		Play (seName, Volume, 1.0f);
	}

	// SEを再生します。
	public void Play (string seName, float volume, float pitch)
	{
		if (!AudioClipDict.ContainsKey (seName)) 
		{
			Debug.LogError (string.Format ("SE名[{0}]が見つかりません。", seName));  
			return;
		}
		if (volume < 0)	volume = 0;
		if (volume > 1)	volume = 1;

		// 再生中じゃないAudioSourceを探す
		AudioSource source = AudioSources.FirstOrDefault (s => !s.isPlaying);

		// 要素が見つからなかったらリストに追加する
		if (source == null) 
		{
			// オーディオソースの数が指定数を超えたら
			if (AudioSources.Count >= MaxAudioSources) 
			{
				Debug.LogWarning ("最大同時再生数を超えました。");
				return;
			}

			// オーディオソースコンポーネントを追加してリストに登録
			source = gameObject.AddComponent<AudioSource> ();
			source.playOnAwake = false;
			source.loop = false;
			AudioSources.Add (source);
		}

		// 見つけたオーディオソースにクリップを登録し、音量、ピッチをセットして再生
		source.clip = this.AudioClipDict [seName];
		source.volume = volume;
		source.pitch = pitch;
		source.Play ();
	}

	// すべてのSEをストップさせる
	public void StopImmediately ()
	{
		foreach (var source in AudioSources) 
		{
			source.Stop ();
		}
	}

	// 特定のSEを即停止
	public void StopImmediately (string seName)
	{
		foreach (var source in AudioSources) 
		{
			if (source.clip.name == seName) 
			{
					source.Stop ();
			}
		}
	}

    // デバッグ用操作パネルを表示
    //public bool DebugMode = true;                               // デバッグモード
    //public void OnGUI()
    //{
    //    if (this.DebugMode)
    //    {
    //        //AudioClipが見つからなかった場合
    //        if (this.AudioClipDict.Count == 0)
    //        {
    //            GUI.Box(new Rect(10, 10, 200, 50), "SE Manager(Debug Mode)");
    //            GUI.Label(new Rect(10, 35, 1000, 20), "Audio clips not found.");
    //            return;
    //        }

    //        //枠
    //        GUI.Box(new Rect(10, 10, 200, 120 + this.AudioClipDict.Count * 25), "SE Manager(Debug Mode)");
    //        int i = 0;
    //        GUI.Label(new Rect(20, 30 + i++ * 20, 180, 20), "Volume : " + this.Volume.ToString("0.00"));
    //        GUI.Label(new Rect(20, 30 + i++ * 20, 180, 20), "Max Play : " + this.MaxAudioSources.ToString("0"));

    //        i = 0;
    //        //再生ボタン
    //        foreach (AudioClip se in this.AudioClipDict.Values)
    //        {
    //            if (GUI.Button(new Rect(20, 80 + i * 25, 40, 20), "Play"))
    //            {
    //                Play(se.name);
    //            }
    //            string txt = string.Format("{0}", se.name);
    //            GUI.Label(new Rect(70, 80 + i * 25, 1000, 20), txt);
    //            i++;
    //        }

    //        //停止ボタン
    //        if (GUI.Button(new Rect(20, 80 + i++ * 25, 180, 20), "Stop"))
    //        {
    //            StopImmediately();
    //        }

    //        int playingSources = this.AudioSources.Count(s => s.isPlaying);
    //        if (playingSources == 1)
    //        {
    //            GUI.Label(new Rect(20, 80 + i * 25, 1000, 20), string.Format("{0} audio source is playing.", playingSources));
    //        }
    //        else if (playingSources > 1)
    //        {
    //            GUI.Label(new Rect(20, 80 + i * 25, 1000, 20), string.Format("{0} audio sources are playing.", playingSources));
    //        }
    //    }
    //}
}
