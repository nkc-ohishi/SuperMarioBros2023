//-----------------------------------------------------------------------------
// 機能　：タイルマップ - 塗ったタイルをすべて削除する
// 元ネタ：https://baba-s.hatenablog.com/entry/2019/08/23/220000
// 使い方：Inspector で Grid を右クリックして「Clear All Tiles」を選択
//-----------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapEditorUtils
{
    [MenuItem("CONTEXT/Grid/Clear All Tiles")]
    private static void ClearAllTilesFromGrid(MenuCommand menuCommand)
    {
        var grid = menuCommand.context as Grid;
        if (grid == null) return;
        var tilemap = grid.GetComponentInChildren<Tilemap>();
        if (tilemap == null) return;
        tilemap.ClearAllTiles();
    }

    [MenuItem("CONTEXT/Tilemap/Clear All Tiles")]
    private static void ClearAllTiles(MenuCommand menuCommand)
    {
        var tilemap = menuCommand.context as Tilemap;
        if (tilemap == null) return;
        tilemap.ClearAllTiles();
    }
}