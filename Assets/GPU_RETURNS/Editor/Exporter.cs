//------------------------------------------------------------------------------
// UnityでProjectSettingsと共にunitypackageをExportする
// 使い方：
// １．Assets内に「Editor」フォルダを作成し、このスクリプトを保存。
// ２．メニューの「Assets」から「ExportWithSettings」を選択。
// ３．
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Exporter : MonoBehaviour
{
    [MenuItem("Assets/ExportWithSettings")] // メニューの場所と名前
    static void Export()
    {
        string[] assetPaths = {
            "Assets",       // Assetsフォルダ内すべて
            //"Assets/Scenes",            // Assetsフォルダ内に作成した任意のフォルダ
            "ProjectSettings/AudioManager.asset",
            "ProjectSettings/ClusterInputManager.asset",
            "ProjectSettings/DynamicsManager.asset",
            "ProjectSettings/EditorBuildSettings.asset",
            "ProjectSettings/GraphicsSettings.asset",
            "ProjectSettings/InputManager.asset",
            "ProjectSettings/NavMeshAreas.asset",
            "ProjectSettings/NetworkManager.asset",
            "ProjectSettings/Physics2DSettings.asset",
            "ProjectSettings/ProjectSettings.asset",
            "ProjectSettings/QualitySettings.asset",
            "ProjectSettings/TagManager.asset",
            "ProjectSettings/TimeManager.asset",
            "ProjectSettings/UnityConnectSettings.asset"
        };

        string packageName = Application.productName + ".unitypackage";

        AssetDatabase.ExportPackage(
            assetPaths,
            packageName,
            ExportPackageOptions.Interactive | ExportPackageOptions.Recurse
        );

    }
}