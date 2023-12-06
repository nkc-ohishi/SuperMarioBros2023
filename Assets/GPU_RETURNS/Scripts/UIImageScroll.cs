//-----------------------------------------------------------------------------
// UI-Image の UVスクロール手順
// １．UI-ImageオブジェクトをCanvasの子オブジェクトとして作成
// ２．任意の画像(Sprite)をImageコンポーネントのSourceImageにアタッチ
// ３．新規マテリアルを作成し、Shader をUI/defaultにする
// ４．３で作成したマテリアルをImageコンポーネントのMaterialにアタッチ
// ５．このスクリプトをUI-Imageオブジェクトにアタッチ
//-----------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageScroll : MonoBehaviour
{
    Image image;                                // イメージコンポーネント保存用
    Vector2 speed = new Vector2(0.1f, 0.1f);    // スクロールスピード

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        image.material.mainTextureOffset += Time.deltaTime * speed;
    }
}



